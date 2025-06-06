using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gym_project_business_logic.Model;
using gym_project_business_logic.Services.Interface;
using Microsoft.EntityFrameworkCore;

namespace gym_project_business_logic.Services
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private ADatabaseConnection _connection;

        public Repository(ADatabaseConnection connection)
        {
            this._connection = connection;
        }

        public async Task Add(TEntity entity)
        {
            await this._connection.Set<TEntity>().AddAsync(entity);
            await this._connection.SaveChangesAsync();
        }

        public async Task<bool> Delete(int id)
        {
            var gym = await this._connection.Set<TEntity>().FindAsync(id);
            if (gym == null) return false;

            this._connection.Set<TEntity>().Remove(gym);
            await this._connection.SaveChangesAsync();
            return true;
        }

        public async Task<IEnumerable<TEntity>> Get()
        {
            return await this._connection.Set<TEntity>().ToListAsync();
        }

        public async Task<TEntity> GetById(int id)
        {
            return await this._connection.Set<TEntity>().FindAsync(id);
        }
    }
}
