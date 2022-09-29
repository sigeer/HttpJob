﻿using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    public class SpiderDetailViewModel: SpiderListItemViewModel
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
        public List<TemplateEditDto> TemplateList { get; set; } = new List<TemplateEditDto>();
        public TemplateEditDto? NextPageTemplate { get; set; }
        public object? PostObj => string.IsNullOrEmpty(PostObjStr) ? null : JsonConvert.DeserializeObject(PostObjStr);
        public Dictionary<string, string> GetHeaders()
        {
            Dictionary<string, string> dic = new Dictionary<string, string>();
            var headerObj = JsonConvert.DeserializeObject<List<SpiderHeaderDto>>(HeaderStr ?? "") ?? new List<SpiderHeaderDto>();
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
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
