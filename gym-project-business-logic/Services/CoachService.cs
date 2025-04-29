using System.Linq;
using gym_project_business_logic.Model;
using gym_project_business_logic.Services.Interface;

namespace gym_project_business_logic.Services
{
	public class CoachService : ICoachService
	{
		private ADatabaseConnection _connection;

		public CoachService(ADatabaseConnection databaseConnection)
		{
			this._connection = databaseConnection;
		}

		public bool Registration(Coach coach)
		{
			if (!this.FindCoach(coach))
			{

				this._connection.Coachs.Add(coach);
				this._connection.SaveChanges();

				return true;
			}

			return false;
		}

		public bool FindCoach(Coach coach)
		{
			if (this._connection.Coachs.Any(usr =>
			usr.Login == coach.Login))
			{
				return true;
			}

			return false;
		}

		public Coach? GetCoach(string login)
		{
			return this._connection.Coachs.Where(u => u.Login == login).FirstOrDefault();
		}

        public bool DeleteCoach(string LoginCoach)
        {

                Coach coach = this.GetCoach(LoginCoach);

                if (coach != null)
                {
                    if (this.FindCoach(coach))
                    {
                        this._connection.Coachs.Remove(coach);
                        this._connection.SaveChanges();

                        return true;
                    }
                }
           
            return false;
        }
    }
}
