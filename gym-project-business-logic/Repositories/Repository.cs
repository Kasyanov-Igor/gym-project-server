using System.Collections.Generic;
using System.Threading.Tasks;
using gym_project_business_logic.Services.Interface;
using Microsoft.EntityFrameworkCore;
using Repositories.Interface;

namespace Repositories
{
	public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
	{
		private ADatabaseConnection _connection;

		public Repository(ADatabaseConnection connection)
		{
			_connection = connection;
		}

		public async Task Add(TEntity entity)
		{
			await _connection.Set<TEntity>().AddAsync(entity);
			await _connection.SaveChangesAsync();
		}

		public async Task<bool> Delete(int id)
		{
			var gym = await _connection.Set<TEntity>().FindAsync(id);
			if (gym == null) return false;

			_connection.Set<TEntity>().Remove(gym);
			await _connection.SaveChangesAsync();
			return true;
		}

		public async Task<IEnumerable<TEntity>> Get()
		{
			return await _connection.Set<TEntity>().ToListAsync();
		}

		public async Task<TEntity> GetById(int id)
		{
			return await _connection.Set<TEntity>().FindAsync(id);
		}
	}
}
