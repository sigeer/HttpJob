﻿using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Polly;
using Polly.Extensions.Http;
using SpiderTool.Data;
using SpiderTool.Data.Dto.Spider;
using SpiderTool.Data.Dto.Tasks;
using SpiderTool.Data.IService;
using System.Web;
using Utility.Extensions;

namespace SpiderTool
{
    public class SpiderWorker
    {
        #region Event
        /// <summary>
        /// 初始化任务（插入数据，尚未执行）
        /// </summary>
        public event EventHandler<SpiderWorker>? OnTaskInit;
        /// <summary>
        /// 状态变更事件（Init,Start,Complete,Cancel）
        /// </summary>
        public event EventHandler<SpiderWorker>? OnTaskStatusChanged;
        /// <summary>
        /// 任务开始 （发起请求并获取数据，未处理响应内容）
        /// </summary>
        public event EventHandler<SpiderWorker>? OnTaskStart;
        /// <summary>
        /// 任务完成
        /// </summary>
        public event EventHandler<SpiderWorker>? OnTaskComplete;
        /// <summary>
        /// 创建子任务
        /// </summary>
        public event EventHandler<SpiderWorker>? OnNewTask;
        /// <summary>
        /// 任务取消
        /// </summary>
        public event EventHandler<SpiderWorker>? OnTaskCanceled;
        #endregion

        #region Property
        public SpiderWorker? ParentTask { get; private set; }
        public bool IsChildTask => ParentTask != null;
        public List<SpiderWorker> ChildrenTask { get; } = new List<SpiderWorker>();
        private string? _contextId;
        public string ContextId
        {
            get
            {
                if (string.IsNullOrEmpty(_contextId))
                    _contextId = Guid.NewGuid().ToString();
                return _contextId;
            }
        }
        private readonly SpiderDetailViewModel? _spider;
        protected SpiderDetailViewModel Spider
        {
            get
            {
                ArgumentNullException.ThrowIfNull(_spider);
                return _spider;
            }
        }
        public string DocumentTitle => HttpUtility.HtmlDecode(_currentDoc?.DocumentNode?.SelectSingleNode("//title")?.InnerText) ?? _rootUrl;
        private string? _description;
        public string Description 
        { 
            get 
            {
                if (string.IsNullOrEmpty(_description))
                {
                    _description = GenarteDirName();
                    var path = Path.Combine(Configs.BaseDir, _description);
                    if (Directory.Exists(path))
                        _description = $"{TaskId}_{GenarteDirName()}";
                }
                return _description;
            } 
        }

        private string? _currentDir;
        public string CurrentDir
        {
            get
            {
                if (string.IsNullOrEmpty(_currentDir))
                    _currentDir = Path.Combine(Configs.BaseDir, Description);
                return _currentDir;
            }
        }
        private readonly HtmlDocument _currentDoc = new HtmlDocument();
        /// <summary>
        /// 在同一个爬虫任务内始终一致
        /// </summary>
        private readonly string _rootUrl = string.Empty;
        private string _currentUrl = string.Empty;
        private int _taskId;
        public int TaskId => _taskId;
        public TaskType Status { get; set; }
        public string HostUrl => _rootUrl.GetHostUrl();
        #endregion Property

        #region Dependency
        readonly ISpiderService? _service;
        readonly ISpiderProcessor _processor;
        readonly ILogger<SpiderWorker> _logger;
        readonly WorkerController _control;
        #endregion Dependency

        public SpiderWorker(ILogger<SpiderWorker> logger, int spiderId, string url, ISpiderService service, ISpiderProcessor? processor = null)
        {
            Log.Logger = logger;
            ArgumentNullException.ThrowIfNull(service);

            _service = service;
            _processor = processor ?? new DefaultSpiderProcessor();
            _logger = logger;
            _control = _service.GetController();

            _rootUrl = url;

            _spider = _service.GetSpider(spiderId);
            if (Spider == null)
                throw new Exception($"spider {spiderId} not existed");

            _service.SetLinkedSpider(Spider);
        }

        /// <summary>
        /// 生成子爬虫
        /// </summary>
        /// <param name="spiderDetail"></param>
        /// <param name="url"></param>
        /// <param name="rootSpider"></param>
        /// <exception cref="Exception"></exception>
        public SpiderWorker(SpiderDetailViewModel spiderDetail, string url, SpiderWorker rootSpider)
        {
            _service = rootSpider._service;
            _processor = rootSpider._processor;
            _logger = rootSpider._logger;
            _control = rootSpider._service!.GetController();
            ParentTask = rootSpider;
            rootSpider.ChildrenTask.Add(this);

            _rootUrl = url;
            _spider = spiderDetail;
            if (Spider == null)
                throw new Exception($"spiderDetail not existed");

            _service?.SetLinkedSpider(Spider);
        }


        public void MountChildTaskEvent(SpiderWorker childTask)
        {
            _logger.LogInformation($"创建子任务: {childTask.TaskId}");
            OnNewTask?.Invoke(this, childTask);

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

        public async Task Start()
        {
            try
            {
                _taskId = _service?.AddTask(new TaskEditDto
                {
                    RootUrl = _rootUrl,
                    SpiderId = Spider.Id,
                    Status = (int)TaskType.NotEffective
                }) ?? -1;
                var tokenSource = _control.GetOrAdd(TaskId);
                UpdateTaskStatus(TaskType.NotEffective);

                await ProcessUrl(_rootUrl, true, tokenSource.Token);
                await CompleteTask(tokenSource.Token);
            }
            catch (OperationCanceledException cancelException)
            {
                UpdateTaskStatus(TaskType.Canceled, cancelException.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.ToString());
            }
        }

        public async Task CompleteTask(CancellationToken cancellationToken = default)
        {
            UpdateTaskStatus(TaskType.Completed);
            await SpiderUtility.MergeTextFileAsync(CurrentDir, cancellationToken);
            if (Directory.Exists(CurrentDir))
            {
                _logger.LogInformation($"文件保存在：<file://{CurrentDir}>");
            }
        }

        public void UpdateTaskStatus(TaskType taskStatus, string logStr = "")
        {
            if (Status == taskStatus)
                return;

            Status = taskStatus;
            switch (taskStatus)
            {
                case TaskType.NotEffective:
                    OnTaskInit?.Invoke(this, this);
                    OnTaskStatusChanged?.Invoke(this, this);
                    _logger.LogInformation($"添加任务：{TaskId} {_rootUrl}");
                    break;
                case TaskType.InProgress:
                    _service?.UpdateTask(new TaskEditDto
                    {
                        Id = TaskId,
                        Description = Description,
                        Status = (int)TaskType.InProgress
                    });
                    OnTaskStart?.Invoke(this, this);
                    OnTaskStatusChanged?.Invoke(this, this);
                    _logger.LogInformation($"开始任务：{TaskId} {_currentUrl}");
                    break;
                case TaskType.Completed:
                    _service?.SetTaskStatus(TaskId, (int)TaskType.Completed);
                    OnTaskComplete?.Invoke(this, this);
                    OnTaskStatusChanged?.Invoke(this, this);
                    _logger.LogInformation($"完成任务：{TaskId}");
                    break;
                case TaskType.Canceled:
                    _service?.SetTaskStatus(TaskId, (int)TaskType.Canceled);
                    OnTaskCanceled?.Invoke(this, this);
                    OnTaskStatusChanged?.Invoke(this, this);
                    _logger.LogInformation($"取消任务：{TaskId} {logStr}");
                    _control.Return(TaskId);
                    break;
                default:
                    break;
            }
        }

        public static async Task<string> RequestDocumentContent(string url, SpiderDetailViewModel spiderConfig, CancellationToken cancellationToken = default)
        {
            var requestConfig = new HttpRequestMessage();
            requestConfig.RequestUri = new Uri(url);
            if (!string.IsNullOrEmpty(spiderConfig.Method))
                requestConfig.Method = new HttpMethod(spiderConfig.Method);
            if (!string.IsNullOrEmpty(spiderConfig.PostObjStr))
                requestConfig.Content = new StringContent(spiderConfig.PostObjStr);

            var headerObj = spiderConfig.GetHeaders();
            foreach (var item in headerObj)
            {
                requestConfig.Headers.TryAddWithoutValidation(item.Key, item.Value);
            }

            using HttpClient httpClient = new HttpClient();
            httpClient.Timeout = TimeSpan.FromSeconds(60);
            var policy = HttpPolicyExtensions.HandleTransientHttpError().RetryAsync(3, onRetry: (outcome, retryNumber, context) =>
            {
                Log.Logger.LogError($"Retry {retryNumber} for {outcome.Result?.RequestMessage?.RequestUri} due to {outcome.Result?.StatusCode}");
            });
            var res = await policy.ExecuteAsync(async (ctx) =>
             {
                 return await httpClient.SendAsync(requestConfig, ctx);
             }, cancellationToken);
            var resStream = await res.Content.ReadAsStreamAsync(cancellationToken);
            return resStream.DecodeData(res.Content.Headers.ContentType?.CharSet);

        }

        private async Task ProcessUrl(string currentUrl, bool isRootUrl = true, CancellationToken cancellationToken = default)
        {
            _currentUrl = currentUrl.GetTotalUrl(HostUrl);
            _logger.LogInformation($"即将访问：{_currentUrl}");
            var documentContent = await RequestDocumentContent(_currentUrl, Spider, cancellationToken);
            _currentDoc.LoadHtml(documentContent);

            if (isRootUrl)
                UpdateTaskStatus(TaskType.InProgress);

            await _processor.ProcessContentAsync(this, documentContent, Spider.TemplateList, cancellationToken);
            await MoveToNextPageAsync(cancellationToken);
        }

        private async Task MoveToNextPageAsync(CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (Spider.NextPageTemplate == null || string.IsNullOrEmpty(Spider.NextPageTemplate.TemplateStr))
                return;

            var nextPageNode = _currentDoc.DocumentNode.SelectSingleNode(Spider.NextPageTemplate.TemplateStr);
            if (nextPageNode != null)
            {
                var nextUrl = (nextPageNode.Attributes[Spider.NextPageTemplate.ReadAttribute ?? "href"] ?? nextPageNode.Attributes["href"] ?? nextPageNode.Attributes["data-href"])?.Value;
                if (!string.IsNullOrEmpty(nextUrl))
                    await ProcessUrl(nextUrl, false);
            }
        }

        private string GenarteDirName()
        {
            if (!string.IsNullOrEmpty(DocumentTitle))
                return DocumentTitle.RemoveInvalidSymbolForFile();

            return $"AutoGenerate_{Spider.Id}";
        }
    }
}
