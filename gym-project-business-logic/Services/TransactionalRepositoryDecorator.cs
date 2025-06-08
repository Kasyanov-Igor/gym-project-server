using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using gym_project_business_logic.Services.Interface;

namespace gym_project_business_logic.Services
{
	public class TransactionalRepositoryDecorator<T> : IRepository<T> where T : class
	{
		private readonly IRepository<T> _repository;
		private ADatabaseConnection _connection;

		public TransactionalRepositoryDecorator(ADatabaseConnection connection, IRepository<T> repository)
		{
			this._repository = repository;
			this._connection = connection;
		}

		public async Task Add(T entity)
		{
			await using var transaction = await this._connection.Database.BeginTransactionAsync();
			try
			{
				await this._repository.Add(entity);
				await transaction.CommitAsync();
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
				throw;
			}
		}

		public async Task<IEnumerable<T>> Get()
		{
			return await this._repository.Get();
		}

		public async Task<T> GetById(int id)
		{
			return await this._repository.GetById(id);
		}

		public async Task<bool> Delete(int id)
		{
			await using var transaction = await this._connection.Database.BeginTransactionAsync();
			try
			{
				var d = await this._repository.Delete(id);
				await transaction.CommitAsync();
				return d;
			}
			catch (Exception ex)
			{
				await transaction.RollbackAsync();
				Console.WriteLine($"Ошибка при сохранении: {ex.Message}");
				throw;
			}
		}
	}
}
