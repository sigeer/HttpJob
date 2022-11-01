using HtmlAgilityPack;
using SpiderTool.Constants;
using SpiderTool.Dto.Spider;
using SpiderTool.Dto.Tasks;
using SpiderTool.IService;
using System.Web;
using Utility.Extensions;
using Utility.Http;

namespace SpiderTool
{
    public class SpiderWorker
    {
        private readonly SpiderDetailViewModel? _spider;
        protected SpiderDetailViewModel Spider
        {
            get
            {
                ArgumentNullException.ThrowIfNull(_spider);
                return _spider;
            }
        }
        private string DocumentTitle => HttpUtility.HtmlDecode(_currentDoc?.DocumentNode?.SelectSingleNode("//title")?.InnerText) ?? _rootUrl;
        /// <summary>
        /// 会经过下一页设置进行变换
        /// </summary>
        private string? _currentUrl;

        private string? _currentDir;
        public string CurrentDir
        {
            get
            {
                if (string.IsNullOrEmpty(_currentDir))
                {
                    _currentDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "download", $"{TaskId}_{GenarteDirName()}");
                    _currentDir.GetDirectory();
                }
                return _currentDir;
            }
        }
        private readonly HtmlDocument _currentDoc = new HtmlDocument();
        /// <summary>
        /// 在同一个爬虫任务内始终一致
        /// </summary>
        private readonly string _rootUrl = string.Empty;
        private int _taskId;
        public int TaskId => _taskId;
        public string HostUrl => _rootUrl.GetHostUrl();

        readonly ISpiderService? _service;
        readonly ISpiderProcessor _processor;

        /// <summary>
        /// 初始化任务（插入数据，尚未执行）
        /// </summary>
        public event EventHandler<int>? OnTaskInit;
        /// <summary>
        /// 状态变更事件（Init,Start,Complete）
        /// </summary>
        public event EventHandler<int>? OnTaskStatusChanged;
        /// <summary>
        /// 任务开始 （发起请求并获取数据，未处理响应内容）
        /// </summary>
        public event EventHandler<int>? OnTaskStart;
        /// <summary>
        /// 任务完成
        /// </summary>
        public event EventHandler<SpiderWorker>? OnTaskComplete;
        /// <summary>
        /// 日志
        /// </summary>
        public event EventHandler<string>? OnLog;
        /// <summary>
        /// 创建子任务
        /// </summary>
        public event EventHandler<SpiderWorker>? OnNewTask;
        /// <summary>
        /// 任务取消
        /// </summary>
        public event EventHandler<string>? OnTaskCanceled;


        public SpiderWorker(int spiderId, string url, ISpiderService service, ISpiderProcessor? processor = null)
        {
            _service = service ?? throw new Exception("ISpiderService不能为null");
            _processor = processor ?? new DefaultSpiderProcessor();
            _rootUrl = url;

            _spider = _service.GetSpider(spiderId);
            if (Spider == null)
                throw new Exception($"spider {spiderId} not existed");

            _service.SetLinkedSpider(Spider);
        }

        public SpiderWorker(SpiderDetailViewModel spiderDetail, string url, ISpiderService? service = null, ISpiderProcessor? processor = null)
        {
            _service = service;
            _processor = processor ?? new DefaultSpiderProcessor();
            _rootUrl = url;

            _spider = spiderDetail;
            if (Spider == null)
                throw new Exception($"spiderDetail not existed");

            _service?.SetLinkedSpider(Spider);
        }

        public void CallLog(string logStr)
        {
            OnLog?.Invoke(this, logStr);
        }

        public void MountChildTaskEvent(SpiderWorker childTask)
        {
            OnNewTask?.Invoke(this, childTask);
            childTask.OnLog += (obj, evt) =>
            {
                OnLog?.Invoke(obj, evt);
            };
            childTask.OnNewTask += (obj, evt) =>
            {
                OnNewTask?.Invoke(obj, evt);
            };
            childTask.OnTaskInit += (obj, evt) =>
            {
                OnTaskInit?.Invoke(obj, evt);
            };
            childTask.OnTaskStart += (obj, evt) =>
            {
                OnTaskStart?.Invoke(obj, evt);
            };
            childTask.OnTaskComplete += (obj, evt) =>
            {
                OnTaskComplete?.Invoke(obj, evt);
            };
            childTask.OnTaskStatusChanged += (obj, evt) =>
            {
                OnTaskStatusChanged?.Invoke(obj, evt);
            };
            childTask.OnTaskCanceled += (obj, evt) =>
            {
                OnTaskCanceled?.Invoke(obj, evt);
            };
        }

        public async Task Start(CancellationToken cancellationToken = default)
        {
            _taskId = _service?.AddTask(new TaskEditDto
            {
                RootUrl = _rootUrl,
                SpiderId = Spider.Id,
                Status = (int)TaskType.NotEffective
            }) ?? -1;
            UpdateTaskStatus(TaskType.NotEffective);

            await ProcessUrl(_rootUrl, true, cancellationToken);
            await CompleteTask();
        }

        public async Task CompleteTask()
        {
            UpdateTaskStatus(TaskType.Completed);
            await SpiderUtility.MergeTextFileAsync(CurrentDir);
        }

        public void UpdateTaskStatus(TaskType taskStatus, string logStr = "")
        {
            switch (taskStatus)
            {
                case TaskType.NotEffective:
                    OnTaskInit?.Invoke(this, TaskId);
                    OnTaskStatusChanged?.Invoke(this, TaskId);
                    break;
                case TaskType.InProgress:
                    OnTaskStart?.Invoke(this, TaskId);
                    OnTaskStatusChanged?.Invoke(this, TaskId);
                    break;
                case TaskType.Completed:
                    _service?.SetTaskStatus(TaskId, (int)TaskType.Completed);
                    OnTaskComplete?.Invoke(this, this);
                    OnTaskStatusChanged?.Invoke(this, TaskId);
                    break;
                case TaskType.Canceled:
                    _service?.SetTaskStatus(TaskId, (int)TaskType.Canceled);
                    OnTaskCanceled?.Invoke(this, logStr);
                    OnTaskStatusChanged?.Invoke(this, TaskId);
                    break;
                default:
                    break;
            }
        }

        private async Task<string> LoadDocumentContent()
        {
            HttpResponseMessage res;
            var url = _currentUrl!.GetTotalUrl(HostUrl);
            if (Spider.Method == RequestMethod.POST)
                res = await HttpRequest.PostRawAsync(url, Spider.PostObj, Spider.GetHeaders());
            else
                res = await HttpRequest.GetRawAsync(url, Spider.GetHeaders());

            var responseStream = await res.Content.ReadAsStreamAsync();
            return responseStream.DecodeData(res.Content.Headers.ContentType?.CharSet);
        }

        private async Task ProcessUrl(string currentUrl, bool isRootUrl = true, CancellationToken cancellationToken = default)
        {
            _currentUrl = currentUrl;

            var documentContent = await LoadDocumentContent();
            _currentDoc.LoadHtml(documentContent);

            if (isRootUrl)
            {
                _service?.UpdateTask(new TaskEditDto
                {
                    Id = TaskId,
                    Description = DocumentTitle,
                    Status = (int)TaskType.InProgress
                });
                UpdateTaskStatus(TaskType.InProgress);
            }

            await _processor.ProcessContentAsync(this, documentContent, Spider.TemplateList, cancellationToken);
            await MoveToNextPageAsync(cancellationToken);
        }

        private async Task MoveToNextPageAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                UpdateTaskStatus(TaskType.Canceled, $"task {TaskId} canceled | from method MoveToNextPage ");
                return;
            }
            if (Spider.NextPageTemplate == null || string.IsNullOrEmpty(Spider.NextPageTemplate.TemplateStr))
                return;
            var nextPageNode = _currentDoc.DocumentNode.SelectSingleNode(Spider.NextPageTemplate.TemplateStr);
            if (nextPageNode != null)
            {
                var nextUrl = (nextPageNode.Attributes["href"] ?? nextPageNode.Attributes["data-href"])?.Value;
                if (!string.IsNullOrEmpty(nextUrl))
                    await ProcessUrl(nextUrl, false, cancellationToken);
            }
        }

        private string GenarteDirName()
        {
            if (!string.IsNullOrEmpty(DocumentTitle))
                return DocumentTitle.RenameFolder();
            return $"AutoGenerate";
        }
    }
}
