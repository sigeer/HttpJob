using AutoMapper;
using SpiderTool.DataBase;
using SpiderTool.Dto.Spider;
using SpiderTool.Dto.Tasks;

namespace SpiderTool.Data.Mapper
{
    /// <summary>
    /// Dto -> Entity -> ViewModel -> Dto
    /// </summary>
    public class SpiderProfile : Profile
    {
        public SpiderProfile()
        {
            CreateMap<DB_Spider, SpiderDetailViewModel>()
                .ForMember(a => a.HeaderStr, b => b.MapFrom(x => x.Headers));
            CreateMap<DB_Spider, SpiderListItemViewModel>();

            CreateMap<DB_Template, TemplateDetailViewModel>();

            CreateMap<DB_Task, TaskSimpleViewModel>();
            CreateMap<DB_Task, TaskListItemViewModel>();

            CreateMap<SpiderEditDto, DB_Spider>();
            CreateMap<TemplateEditDto, DB_Template>();
            CreateMap<TaskEditDto, DB_Task>();

            CreateMap<DB_ReplacementRule, ReplacementRuleDto>();
        }
    }
}
