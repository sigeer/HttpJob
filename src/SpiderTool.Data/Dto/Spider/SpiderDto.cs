using System.Text.Json;
using Utility.Extensions;

namespace SpiderTool.Dto.Spider
{
    public class SpiderEditDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        /// <summary>
        /// "POST" or "GET"
        /// </summary>
        public string? Method { get; set; }
        /// <summary>
        /// object => json
        /// </summary>
        public string? PostObjStr { get; set; }
        /// <summary>
        /// <see cref="List{SpiderHeaderDto}"/> => json
        /// </summary>
        public string? Headers { get; set; }

        public int? NextPageTemplateId { get; set; }
        public List<int> Templates { get; set; } = new List<int>();

        public bool FormValid()
        {
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Method) && Templates.Count > 0;
        }
    }
    public class SpiderListItemViewModel
    {
        public int Id { get; set; }
        public string? Name { get; set; }
    }

    public class SpiderDetailViewModel : SpiderListItemViewModel
    {
        public string? Description { get; set; }
        /// <summary>
        /// "POST" or "GET"
        /// </summary>
        public string? Method { get; set; }
        /// <summary>
        /// object => json
        /// </summary>
        public string? PostObjStr { get; set; }
        /// <summary>
        /// <see cref="List{SpiderHeaderDto}"/> => json
        /// </summary>
        public string? HeaderStr { get; set; }
        public List<TemplateDetailViewModel> TemplateList { get; set; } = new List<TemplateDetailViewModel>();
        public TemplateDetailViewModel? NextPageTemplate { get; set; }
        public object? PostObj => string.IsNullOrEmpty(PostObjStr) ? null : PostObjStr.ToJson();
        public Dictionary<string, string> GetHeaders()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var headerObj = JsonSerializer.Deserialize<List<SpiderHeaderDto>>(HeaderStr ?? "[]") ?? new List<SpiderHeaderDto>();
            headerObj.ForEach(x =>
            {
                dic.Add(x.Key, x.Value);
            });
            return dic;
        }

        public SpiderEditDto ToEditModel()
        {
            return new SpiderEditDto
            {
                Id = Id,
                Name = Name,
                Description = Description,
                Headers = HeaderStr,
                Method = Method,
                NextPageTemplateId = NextPageTemplate?.Id,
                PostObjStr = PostObjStr,
                Templates = TemplateList.Select(x => x.Id).ToList()
            };
        }
    }

    public class SpiderHeaderDto
    {
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
    }
}
