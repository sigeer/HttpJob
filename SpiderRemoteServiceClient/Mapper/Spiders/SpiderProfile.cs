using AutoMapper;
using SpiderService;
using SpiderTool.Dto.Spider;
using SpiderTool.Dto.Tasks;

namespace SpiderRemoteServiceClient.Mapper.Spiders
{
    /// <summary>
    /// client to server: normal dto to proto dto, proto viewmodel to normal viewmodel
    /// </summary>
    public class SpiderProfile : Profile
    {
        public SpiderProfile()
        {
            CreateMap<TaskEditDto, TaskProtoEditDto>();
            CreateMap<TaskProtoViewModel, TaskListItemViewModel>();
            CreateMap<TaskProtoSimpleViewModel, TaskSimpleViewModel>();

            CreateMap<SpiderEditDto, SpiderProtoEditDto>()
                .ForMember(x => x.NextPageId, opt => opt.MapFrom(y => y.NextPageTemplateId));
            CreateMap<SpiderProtoListItemViewModel, SpiderListItemViewModel>();
            CreateMap<SpiderProtoDetailViewModel, SpiderDetailViewModel>()
                .ForMember(y => y.NextPageTemplate == null ? 0 : y.NextPageTemplate.Id, opt => opt.MapFrom(x => x.NextPageId))
                .ForMember(y => y.HeaderStr, opt => opt.MapFrom(x => x.Headers));

            CreateMap<TemplateEditDto, TemplateProtoDto>()
                .ForMember(x => x.XPath, opt => opt.MapFrom(y => y.TemplateStr));
        }
    }
}
