using AutoMapper;
using SpiderTool.DataBase;
using SpiderTool.Dto.Spider;

namespace SpiderTool.Data.Mapper
{
    /// <summary>
    /// client to server: normal dto to proto dto, proto viewmodel to normal viewmodel
    /// </summary>
    public class SpiderProfile : Profile
    {
        public SpiderProfile()
        {
            CreateMap<DB_Spider, SpiderDetailViewModel>();
        }
    }
}
