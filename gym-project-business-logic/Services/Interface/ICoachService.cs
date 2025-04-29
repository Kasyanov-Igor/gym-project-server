using gym_project_business_logic.Model;

namespace gym_project_business_logic.Services.Interface
{
    public interface ICoachService
    {
        public Coach? GetCoach(string login);

        public bool Registration(Coach user);

        public bool FindCoach(Coach user);

        public bool DeleteCoach(string LoginCoach);
    }
}
