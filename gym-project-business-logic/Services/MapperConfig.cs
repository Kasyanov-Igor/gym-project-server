using AutoMapper;
using gym_project_business_logic.Model;
using gym_project_business_logic.Model.Domains;

namespace gym_project_business_logic.Services
{
	public class MapperConfig : Profile
	{
		public MapperConfig()
		{
			CreateMap<DTOCoach, Coach>();
		}
		public IMapper CreateMapper()
		{
			var config = new MapperConfiguration(cfg => cfg.AddProfile<MapperConfig>());
			return config.CreateMapper();
		}
	}
}
