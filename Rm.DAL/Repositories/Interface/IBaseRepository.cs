using Rm.Model.Models;


namespace Rm.DAL
{
    public interface IBaseRepository<T> where T : BaseModel
    {
        public List<T> GetAll();
        public T GetById(int id);
        public T Create(T entity);
        public T Update(T entity);
        public void Delete (T entity);
    }
}
