using System.Linq;
using System.Threading.Tasks;
using gym_project_business_logic.Model;
using gym_project_business_logic.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace gym_project_business_logic.Services
{
	public class CoachService : ICoachService
	{
		private ADatabaseConnection _connection;

		public CoachService(ADatabaseConnection databaseConnection)
		{
			this._connection = databaseConnection;
		}

		public async Task AddCoach(Coach coach)
		{
			if (!this.FindCoach(coach))
			{
				await this._connection.Coachs.AddAsync(coach);
                await this._connection.SaveChangesAsync();
			}
		}

        public async Task<bool> GetEmail(string emailAddress)
        {
            return !await _connection.Coachs.AnyAsync(u => u.Email == emailAddress);
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

                Coach? coach = this.GetCoach(LoginCoach);

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
