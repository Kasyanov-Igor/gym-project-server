using gym_project_business_logic.Model;
using Microsoft.EntityFrameworkCore;
using Model.Entities;

namespace gym_project_business_logic.Services.Interface
{
    public abstract class ADatabaseConnection : DbContext
	{
		protected abstract string ReturnConnectionString();
		protected string ConnectionString { get; private set; }

		public DbSet<Client> Clients => Set<Client>();
		public DbSet<Admin> Admins => Set<Admin>();
		public DbSet<Coach> Coachs => Set<Coach>();
		public DbSet<Workout> Workouts => Set<Workout>();
		public DbSet<Gym> Gyms => Set<Gym>();
		public DbSet<Training> Trainings => Set<Training>();

		public ADatabaseConnection()
		{
			this.ConnectionString = this.ReturnConnectionString();

			this.Database.EnsureCreated();
		}
	}
}