using gym_project_business_logic.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace gym_project_business_logic.Services
{
	public class SqliteConnection : ADatabaseConnection
	{
		public const string _DATABASE_NAME = "../TopChat.db";

		protected override string ReturnConnectionString()
		{
			return $"Data Source={_DATABASE_NAME}";
		}

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			optionsBuilder
				.UseSqlite(this.ConnectionString);
		}
	}
}