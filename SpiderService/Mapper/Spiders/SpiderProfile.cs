using AutoMapper;
using SpiderTool.Dto.Spider;
using SpiderTool.Dto.Tasks;

namespace SpiderService.Mapper.Spiders
{
    /// <summary>
    /// server to client: proto dto to normal dto, normal viewmodel to proto viewmodel
    /// </summary>
    public class SpiderProfile : Profile
    {
        public SpiderProfile()
        {
            CreateMap<TaskProtoEditDto, TaskEditDto>();
            CreateMap<TaskListItemViewModel, TaskProtoViewModel>();
            CreateMap<TaskSimpleViewModel, TaskProtoSimpleViewModel>();

            CreateMap<SpiderProtoEditDto, SpiderEditDto>()
                .ForMember(x => x.NextPageTemplateId, opt => opt.MapFrom(y => y.NextPageId));
            CreateMap<SpiderListItemViewModel, SpiderProtoListItemViewModel>();
            CreateMap<SpiderDetailViewModel, SpiderProtoDetailViewModel>()
                .ForMember(x => x.NextPageId, opt => opt.MapFrom(y => y.NextPageTemplate == null ? 0 : y.NextPageTemplate.Id))
                .ForMember(x => x.Headers, opt => opt.MapFrom(y => y.HeaderStr));

            CreateMap<TemplateEditDto, TemplateProtoDto>()
                .ForMember(x => x.XPath, opt => opt.MapFrom(y => y.TemplateStr));
            CreateMap<TemplateProtoDto, TemplateEditDto>()
                .ForMember(x => x.TemplateStr, opt => opt.MapFrom(y => y.XPath));

            CreateMap<TemplateDetailViewModel, TemplateProtoDto>()
                .ForMember(x => x.XPath, opt => opt.MapFrom(y => y.TemplateStr));
            CreateMap<TemplateProtoDto, TemplateDetailViewModel>()
                .ForMember(x => x.TemplateStr, opt => opt.MapFrom(y => y.XPath));
        }
    }
}
