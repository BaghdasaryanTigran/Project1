using Rm.Model.Models;


namespace Rm.DAL
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        public List<T> GetAll();
        public Task<T> GetById(int id);
        public Task<T> Create(T entity);
        public Task<T> Update(T entity);
        public Task Delete (T entity);
    }
}
