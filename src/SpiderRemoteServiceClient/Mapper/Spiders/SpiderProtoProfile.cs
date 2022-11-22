using AutoMapper;
using SpiderService;
using SpiderTool.Data.Dto.Spider;
using SpiderTool.Data.Dto.Tasks;

namespace SpiderRemoteServiceClient.Mapper.Spiders
{
    /// <summary>
    /// client to server: normal dto to proto dto, proto viewmodel to normal viewmodel
    /// </summary>
    public class SpiderProtoProfile : Profile
    {
        public SpiderProtoProfile()
        {
            CreateMap<TaskEditDto, TaskProtoEditDto>();
            CreateMap<TaskProtoViewModel, TaskListItemViewModel>();
            CreateMap<TaskProtoSimpleViewModel, TaskSimpleViewModel>();

            CreateMap<SpiderEditDto, SpiderProtoEditDto>()
                .ForMember(x => x.NextPageId, opt => opt.MapFrom(y => y.NextPageTemplateId));
            CreateMap<SpiderProtoListItemViewModel, SpiderListItemViewModel>();
            CreateMap<SpiderProtoDetailViewModel, SpiderDetailViewModel>()
                .ForMember(y => y.HeaderStr, opt => opt.MapFrom(x => x.Headers));

            CreateMap<TemplateEditDto, TemplateProtoDto>()
                .ForMember(x => x.XPath, opt => opt.MapFrom(y => y.TemplateStr));

            CreateMap<TemplateDetailViewModel, TemplateProtoDto>()
                .ForMember(x => x.XPath, opt => opt.MapFrom(y => y.TemplateStr));
            CreateMap<TemplateProtoDto, TemplateDetailViewModel>()
                .ForMember(x => x.TemplateStr, opt => opt.MapFrom(y => y.XPath));
        }
    }
}
