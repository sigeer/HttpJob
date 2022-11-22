using AutoMapper;
using SpiderTool.Data.Dto.Spider;
using SpiderTool.Data.Dto.Tasks;

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
            CreateMap<TaskListItemViewModel, TaskProtoViewModel>()
                .ForMember(x => x.Id, opt => opt.MapFrom(x => x.Id))
                .ForMember(x => x.RootUrl, opt => opt.MapFrom(x => x.RootUrl))
                .ForMember(x => x.Description, opt => opt.MapFrom(x => x.Description))
                .ForMember(x => x.SpiderId, opt => opt.MapFrom(x => x.SpiderId))
                .ForMember(x => x.Status, opt => opt.MapFrom(x => x.Status));
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
