using HtmlAgilityPack;
using SpiderTool.Data.Dto.Spider;
using Utility.SimpleStringParse;

namespace SpiderTool
{
    public abstract class BaseSpiderProcessor : ISpiderProcessor
    {
        protected abstract Task ProcessObject(string savePath, SpiderWorker rootSpider, HtmlNodeCollection nodes, TemplateDetailViewModel rule, CancellationToken cancellationToken = default);
        protected abstract Task ProcessText(string savePath, SpiderWorker rootSpider, HtmlNodeCollection nodes, TemplateDetailViewModel rule, CancellationToken cancellationToken = default);
        protected abstract Task ProcessJumpLink(SpiderWorker rootSpider, HtmlNodeCollection nodes, TemplateDetailViewModel rule, CancellationToken cancellationToken = default);
        protected abstract Task ProcessHtml(string savePath, SpiderWorker rootSpider, HtmlNodeCollection nodes, TemplateDetailViewModel rule, CancellationToken cancellationToken = default);
        public virtual async Task ProcessContentAsync(SpiderWorker rootSpider, string documentContent, List<TemplateDetailViewModel> templateRules, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            if (templateRules.Count == 0)
            {
                await SpiderUtility.SaveTextAsync(rootSpider.CurrentDir, documentContent);
                return;
            }

            var currentDoc = new HtmlDocument();
            currentDoc.LoadHtml(documentContent);

            for (int i = 0; i < templateRules.Count; i++)
            {
                cancellationToken.ThrowIfCancellationRequested();

                var rule = templateRules[i];
                var nodes = string.IsNullOrEmpty(rule.TemplateStr)
                    ? new HtmlNodeCollection(currentDoc.DocumentNode)
                    : currentDoc.DocumentNode.SelectNodes(rule.TemplateStr ?? "");
                if (nodes == null || nodes.Count == 0)
                    continue;

                var savePath = templateRules.Count > 1 ? Path.Combine(rootSpider.CurrentDir, $"Rule{rule.Id.ToString()}") : rootSpider.CurrentDir;

                rule.ReplacementRules = FormatReplaceRulesDynamic(currentDoc.DocumentNode, rule.ReplacementRules);

                if (rule.Type == (int)TemplateTypeEnum.Object)
                {
                    await ProcessObject(savePath, rootSpider, nodes, rule, cancellationToken);
                }
                if (rule.Type == (int)TemplateTypeEnum.Text)
                {
                    await ProcessText(savePath, rootSpider, nodes, rule, cancellationToken);
                }
                if (rule.Type == (int)TemplateTypeEnum.Html)
                {
                    await ProcessHtml(savePath, rootSpider, nodes, rule, cancellationToken);
                }
                if (rule.Type == (int)TemplateTypeEnum.JumpLink)
                {
                    await ProcessJumpLink(rootSpider, nodes, rule, cancellationToken);
                }
            }
        }
        protected virtual StringTokenizer GetStringTokenizer()
        {
            return new StringTokenizer(
                new StringTokenTag(DefaultStringTokenProvider.Name_XPath, DefaultStringTokenProvider.StartTag, DefaultStringTokenProvider.EndTag),
                new StringTokenTag(DefaultStringTokenProvider.Name_If, DefaultStringTokenProvider.StartTag, DefaultStringTokenProvider.EndTag),
                new StringTokenTag(DefaultStringTokenProvider.Name_NewLine)
            );
        }
        protected virtual StringTokenProvider GetStringTokenProvider(HtmlNode htmlNode)
        {
            return new DefaultStringTokenProvider(htmlNode);
        }
        protected virtual List<ReplacementRuleDto> FormatReplaceRulesDynamic(HtmlNode htmlNode, List<ReplacementRuleDto> rules)
        {
            var tokenizer = GetStringTokenizer();
            var provider = GetStringTokenProvider(htmlNode);
            foreach (var rule in rules)
            {
                var oldToken = tokenizer.Parse(rule.ReplacementOldStr);
                var oldValue = provider.Serialize(oldToken);

                var newToken = tokenizer.Parse(rule.ReplacementNewlyStr);
                var newlyValue = provider.Serialize(newToken);

                rule.ReplacementOldStr = oldValue;
                rule.ReplacementNewlyStr = newlyValue;
            }
            return rules;
        }
    }

    public class DefaultStringTokenProvider : StringTokenProvider
    {
        readonly HtmlNodeNavigator _navigator;
        public const string Name_XPath = "$XPath";
        public const string Name_NewLine = "$NewLine";
        public const string Name_If = "$If";
        public const char StartTag = '(';
        public const char EndTag = ')';

        public DefaultStringTokenProvider(HtmlNode htmlNode) : base()
        {
            _navigator = (HtmlNodeNavigator)htmlNode.CreateNavigator();
        }

        [StringTokenName(Name_XPath)]
        public string XPath(string args)
        {
            return _navigator.SelectSingleNode(args)?.Value ?? string.Empty;
        }

        [StringTokenName(Name_NewLine)]
        public string NewLine(string args)
        {
            return Environment.NewLine;
        }
        [StringTokenName(Name_If)]
        public string If(string args)
        {
            var argList = args.Split(',');
            if (argList.Length != 4)
                return args;

            return argList[0] == argList[1] ? argList[2] : argList[3];
        }
    }
}
