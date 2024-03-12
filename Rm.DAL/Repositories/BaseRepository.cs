
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
        public async Task<T> Create(T entity)
        {
            Context.Set<T>().Add(entity);
            await Context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(T entity)
        {
            T toDelete = Context.Set<T>().FirstOrDefault(x => x.Id == entity.Id);

            Context.Set<T>().Remove(toDelete);
            await Context.SaveChangesAsync();
        }

        public List<T> GetAll()
        {
            return Context.Set<T>().ToList();
        }

        public async Task<T> GetById(int id)
        {
            T? model = Context.Set<T>().FirstOrDefault(x => x.Id == id);
            
            return model;
        }

        public async Task<T> Update(T entity)
        {
            Context.Update(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
    }
}
