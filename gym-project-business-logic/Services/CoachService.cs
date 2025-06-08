using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using gym_project_business_logic.Model;
using gym_project_business_logic.Model.Domains;
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

		public async Task<bool> GetEmail(string emailAddress)
		{
			return !await _connection.Coachs.AnyAsync(u => u.Email == emailAddress);
		}

		public async Task<Coach?> GetCoach(string login, string password)
		{
			Coach user = await this._connection.Coachs.FirstOrDefaultAsync(u => u.Login == login);
			if (user == null)
			{ return null; }

			string hashedPassword = PasswordHelper.HashPassword(password, user.Salt);

			return await this._connection.Coachs.FirstOrDefaultAsync(u => u.Login == login && u.Password == hashedPassword);
		}

		public async Task<bool> UpdateCoachAsync(int coachId, DTOCoach? newCoach)
		{
			var coach = await this._connection.Coachs.FindAsync(coachId);
			if (coach == null)
			{
				return false; // не найден
			}

			if (coach != null && newCoach != null)
			{
				coach.FullName = newCoach.FullName;
				coach.DateOfBirth = newCoach.DateOfBirth;
				coach.Email = newCoach.Email;
				coach.PhoneNumber = newCoach.PhoneNumber;
				coach.Gender = newCoach.Gender;
				coach.Specialization = newCoach.Specialization;
				coach.Status = newCoach.Status;
				coach.WorkingTime = newCoach.WorkingTime;

				try
				{
					await _connection.SaveChangesAsync();
				}
				catch (DbUpdateConcurrencyException)
				{
					if (!this._connection.Coachs.Any(w => w.Id == coachId))
					{
						return false;
					}
					throw;
				}
			}

			return true;
		}
	}
}
