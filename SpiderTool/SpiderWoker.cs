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
        private SpiderDetailViewModel? _spider;
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
                    _currentDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "wwwroot", "download", GenarteDirName());
                    _currentDir.GetDirectory();
                }
                return _currentDir;
            }
        }
        private HtmlDocument _currentDoc = new HtmlDocument();
        /// <summary>
        /// 在同一个爬虫任务内始终一致
        /// </summary>
        private string _rootUrl = string.Empty;
        private int _taskId;
        public int TaskId => _taskId;
        public string HostUrl => _rootUrl.GetHostUrl();

        readonly ISpiderService _service;
        readonly ISpiderProcessor _processor;

        public event EventHandler<int>? OnTaskStart;
        public event EventHandler<int>? OnTaskStatusChanged;
        public event EventHandler<SpiderWorker>? OnTaskComplete;
        public event EventHandler<string>? OnLog;
        public event EventHandler<SpiderWorker>? OnNewTask;
        public event EventHandler<string>? OnTaskCanceled;


        public SpiderWorker(int spiderId, ISpiderService service, string url)
        {
            _service = service;
            _processor = new DefaultSpiderProcessor(service);
            _rootUrl = url;

            _spider = _service.GetSpider(spiderId);
            if (Spider == null)
                throw new Exception($"spider {spiderId} not existed");
        }

        public SpiderWorker(int spiderId, ISpiderService service, string url, ISpiderProcessor processor)
        {
            _service = service;
            _processor = processor;
            _rootUrl = url;

            _spider = _service.GetSpider(spiderId);
            if (Spider == null)
                throw new Exception($"spider {spiderId} not existed");
        }

        public void CallLog(string logStr)
        {
            OnLog?.Invoke(this, logStr);
        }

        public void CallCancelTask(string logStr)
        {
            OnTaskCanceled?.Invoke(this, logStr);
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
        }

        public async Task Start(CancellationToken cancellationToken = default)
        {
            _taskId = _service.AddTask(new TaskEditDto
            {
                RootUrl = _rootUrl,
                SpiderId = Spider.Id,
                Status = (int)TaskType.NotEffective
            });
            OnTaskStart?.Invoke(this, _taskId);
            OnTaskStatusChanged?.Invoke(this, _taskId);

            await ProcessUrl(_rootUrl, true, cancellationToken);
            await CompleteTask();
        }

        public async Task CompleteTask()
        {
            _service.SetTaskStatus(_taskId, (int)TaskType.Completed);
            OnTaskComplete?.Invoke(this, this);
            OnTaskStatusChanged?.Invoke(this, _taskId);
            await SpiderUtility.MergeTextFileAsync(CurrentDir);
        }

        public async Task<string> LoadDocumentContent()
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

        public async Task ProcessUrl(string currentUrl, bool isRootUrl = true, CancellationToken cancellationToken = default)
        {
            _currentUrl = currentUrl;

            var documentContent = await LoadDocumentContent();
            _currentDoc.LoadHtml(documentContent);

            if (isRootUrl)
            {
                _service.UpdateTask(new TaskEditDto
                {
                    Id = _taskId,
                    Description = DocumentTitle,
                    Status = (int)TaskType.InProgress
                });
                OnTaskStatusChanged?.Invoke(this, _taskId);
            }

            await _processor.ProcessContentAsync(this, documentContent, Spider.TemplateList, cancellationToken);
            await MoveToNextPageAsync(cancellationToken);
        }

        private async Task MoveToNextPageAsync(CancellationToken cancellationToken = default)
        {
            if (cancellationToken.IsCancellationRequested)
            {
                OnTaskCanceled?.Invoke(this, "MoveToNextPage");
                OnLog?.Invoke(this, $"task {TaskId} canceled | from method MoveToNextPage ");
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
            if (!string.IsNullOrEmpty(DocumentTitle) && !SpiderUtility.InvalidFolderSymbol.Any(x => DocumentTitle.Contains(x)))
                return DocumentTitle;
            return $"task_{_taskId}";
        }
    }
}
