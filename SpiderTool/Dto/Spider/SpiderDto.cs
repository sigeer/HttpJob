using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpiderTool.Dto.Spider
{
    public class SpiderDtoSetter
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
    public class SpiderDto: SpiderDtoSetter
    {
        public List<TemplateDto> TemplateList { get; set; } = new List<TemplateDto>();
        public TemplateDto? NextPageTemplate { get; set; }
        public List<SpiderHeaderDto> HeaderObject => JsonConvert.DeserializeObject<List<SpiderHeaderDto>>(Headers ?? "") ?? new List<SpiderHeaderDto>();
        public object? PostObj => string.IsNullOrEmpty(PostObjStr) ? null : JsonConvert.DeserializeObject(PostObjStr);

        public Dictionary<string, string> GetHeaders()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            HeaderObject.ForEach(x =>
            {
                dic.Add(x.Key, x.Value);
            });
            return dic;
        }
    }

    public class SpiderHeaderDto
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
