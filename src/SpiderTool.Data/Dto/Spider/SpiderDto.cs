using System.Net.Http.Headers;
using System.Text.Json;
using Utility.Extensions;

namespace SpiderTool.Data.Dto.Spider
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
            return !string.IsNullOrEmpty(Name) && !string.IsNullOrEmpty(Method);
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
        public List<HeaderItem> GetHeaders()
        {
            return JsonSerializer.Deserialize<List<HeaderItem>>(HeaderStr ?? "{}") ?? new List<HeaderItem>();
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

    public class HeaderItem
    {
        public string Key { get; set; } = null!;
        public string Value { get; set; } = null!;
    }
}
