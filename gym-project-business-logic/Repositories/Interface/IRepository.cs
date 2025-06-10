using System.Collections.Generic;
using System.Threading.Tasks;

namespace gym_project_business_logic.Repositories.Interface
{
    public interface IRepository<TEntity> where TEntity : class
    {
        public Task Add(TEntity entity);
        public Task<IEnumerable<TEntity>> Get();
        public Task<TEntity> GetById(int id);
        // TODO: Add update async
        public Task<bool> Delete(int id);
    }
}
