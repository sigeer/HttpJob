using HtmlAgilityPack;
using SpiderTool.Data.Dto.Spider;
using Utility.SimpleStringParse;

namespace SpiderTool
{
    public abstract class BaseSpiderProcessor : ISpiderProcessor
    {
        protected abstract Task ProcessObject(string savePath, SpiderWorker rootSpider, HtmlNodeCollection nodes, TemplateDetailViewModel rule, CancellationToken cancellationToken = default);
        protected abstract Task ProcessText(string savePath, SpiderWorker rootSpider, HtmlDocument pageDocument, TemplateDetailViewModel rule, CancellationToken cancellationToken = default);
        protected abstract Task ProcessJumpLink(SpiderWorker rootSpider, HtmlNodeCollection nodes, TemplateDetailViewModel rule, CancellationToken cancellationToken = default);
        protected abstract Task ProcessHtml(string savePath, SpiderWorker rootSpider, HtmlDocument pageDocument, TemplateDetailViewModel rule, CancellationToken cancellationToken = default);
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
                var savePath = templateRules.Count > 1 ? Path.Combine(rootSpider.CurrentDir, $"Rule{rule.Id.ToString()}") : rootSpider.CurrentDir;

                rule.ReplacementRules = FormatReplaceRulesDynamic(currentDoc.DocumentNode, rule.ReplacementRules);

                if (rule.Type == (int)TemplateTypeEnum.Object)
                {
                    var nodes = string.IsNullOrEmpty(rule.TemplateStr)
                        ? new HtmlNodeCollection(currentDoc.DocumentNode)
                        : currentDoc.DocumentNode.SelectNodes(rule.TemplateStr ?? "");
                    await ProcessObject(savePath, rootSpider, nodes, rule, cancellationToken);
                }
                if (rule.Type == (int)TemplateTypeEnum.Text)
                {
                    await ProcessText(savePath, rootSpider, currentDoc, rule, cancellationToken);
                }
                if (rule.Type == (int)TemplateTypeEnum.Html)
                {
                    await ProcessHtml(savePath, rootSpider, currentDoc, rule, cancellationToken);
                }
                if (rule.Type == (int)TemplateTypeEnum.JumpLink)
                {
                    var nodes = string.IsNullOrEmpty(rule.TemplateStr)
                        ? new HtmlNodeCollection(currentDoc.DocumentNode)
                        : currentDoc.DocumentNode.SelectNodes(rule.TemplateStr ?? "");
                    await ProcessJumpLink(rootSpider, nodes, rule, cancellationToken);
                }
            }
        }
        protected virtual StringTokenizer GetStringTokenizer()
        {
            return new StringTokenizer(
                new StringTokenTag(DefaultStringTokenProvider.Name_XPathHtml, SimpleStringTokenProvider.StartTag, SimpleStringTokenProvider.EndTag),
                new StringTokenTag(DefaultStringTokenProvider.Name_XPathText, SimpleStringTokenProvider.StartTag, SimpleStringTokenProvider.EndTag),
                new StringTokenTag(DefaultStringTokenProvider.Name_XPathAttr, SimpleStringTokenProvider.StartTag, SimpleStringTokenProvider.EndTag),
                new StringTokenTag(SimpleStringTokenProvider.Name_If, SimpleStringTokenProvider.StartTag, SimpleStringTokenProvider.EndTag),
                new StringTokenTag(SimpleStringTokenProvider.Name_NewLine)
            );
        }
        protected virtual StringTokenProvider GetStringTokenProvider(HtmlNode htmlNode)
        {
            return new DefaultStringTokenProvider(htmlNode);
        }
        protected virtual List<ReplacementRuleDto> FormatReplaceRulesDynamic(HtmlNode htmlNode, List<ReplacementRuleDto> rules)
        {
            var newList = new List<ReplacementRuleDto>();
            var tokenizer = GetStringTokenizer();
            var provider = GetStringTokenProvider(htmlNode);
            foreach (var rule in rules)
            {
                var oldToken = tokenizer.Parse(rule.ReplacementOldStr);
                var oldValue = provider.Serialize(oldToken);

                var newToken = tokenizer.Parse(rule.ReplacementNewlyStr);
                var newlyValue = provider.Serialize(newToken);

                newList.Add(new ReplacementRuleDto
                {
                    Id = rule.Id,
                    IgnoreCase = rule.IgnoreCase,
                    ReplacementOldStr = oldValue,
                    ReplacementNewlyStr = newlyValue
                });
            }
            return newList;
        }
    }

    public class DefaultStringTokenProvider : SimpleStringTokenProvider
    {
        readonly HtmlNode _htmlNode;
        public const string Name_XPathHtml = "$XHtml";
        public const string Name_XPathText = "$XText";
        public const string Name_XPathAttr = "$XAttr";

        public DefaultStringTokenProvider(HtmlNode htmlNode) : base()
        {
            _htmlNode = htmlNode;
        }

        [StringTokenName(Name_XPathHtml)]
        public string XPathHtml(string args)
        {
            return string.Join(Environment.NewLine, _htmlNode.SelectNodes(args).Select(x => x.InnerHtml));
        }

        [StringTokenName(Name_XPathText)]
        public string XPathText(string args)
        {
            return string.Join(Environment.NewLine, _htmlNode.SelectNodes(args).Select(x => x.InnerText));
        }

        [StringTokenName(Name_XPathAttr, ArgCount = 2)]
        public string XPathGetAttr(string args)
        {
            var argList = TryGetArgs(args);
            return _htmlNode.SelectSingleNode(argList[0]).GetAttributeValue(argList[1], null);
        }
    }
}
