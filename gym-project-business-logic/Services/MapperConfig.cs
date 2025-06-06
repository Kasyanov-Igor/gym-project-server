using AutoMapper;
using gym_project_business_logic.Model;
using gym_project_business_logic.Model.Domains;
using Model.Entities;

namespace gym_project_business_logic.Services
{
	public class MapperConfig : Profile
	{
		public MapperConfig()
		{
			CreateMap<DTOCoach, Coach>();
            CreateMap<DTOLogin, Coach>();
            CreateMap<DTOLogin, Admin>();
            CreateMap<DTOWorkout, Workout>();
			CreateMap<AdminDto, Admin>();
            CreateMap<DTOClient, Client>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "Client"));
        }
		public IMapper CreateMapper()
		{
			var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperConfig>());
			return config.CreateMapper();
		}
	}
}
