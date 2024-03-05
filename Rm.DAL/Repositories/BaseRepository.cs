
using Rm.DAL.Context;
using Rm.Model.Models;


namespace Rm.DAL
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseModel
    {
        private readonly RmContext Context;
        public BaseRepository(RmContext context)
        {
            Context = context;
        }   
        public T Create(T entity)
        {
            Context.Set<T>().Add(entity);
            Context.SaveChanges();
            return entity;
        }

        public void Delete(T entity)
        {
            T toDelete = Context.Set<T>().FirstOrDefault(x => x.Id == entity.Id);

            Context.Set<T>().Remove(toDelete);
            Context.SaveChanges();
        }

        public List<T> GetAll()
        {
            return Context.Set<T>().ToList();
        }

        public T GetById(int id)
        {
            T? model = Context.Set<T>().FirstOrDefault(x => x.Id == id);
            Context.SaveChanges();
            return model;
        }

        public T Update(T entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
            return entity;
        }
    }
}
