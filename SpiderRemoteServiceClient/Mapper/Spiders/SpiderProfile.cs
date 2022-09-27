﻿using AutoMapper;
using SpiderService;
using SpiderTool.Dto.Spider;
using SpiderTool.Dto.Tasks;

namespace SpiderRemoteServiceClient.Mapper.Spiders
{
    public class SpiderProfile : Profile
    {
        public SpiderProfile()
        {
            CreateMap<TaskSetter, TaskProtoDto>();
            CreateMap<TaskProtoDto, TaskDto>();
            CreateMap<SpiderDtoSetter, SpiderEditProtoDto>()
                .ForMember(x => x.Templates, opt =>
                {
                    opt.MapFrom(y => y.Templates);
                });

            CreateMap<TemplateDto, TemplateProtoDto>();
            CreateMap<TemplateProtoDto, TemplateDto>();
        }
    }
}