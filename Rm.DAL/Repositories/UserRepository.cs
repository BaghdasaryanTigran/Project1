using AutoMapper;
using Rm.DAL.Context;
using Rm.DAL.Repositories.Interface;
using Rm.Model.Models;


namespace Rm.DAL.Repositories
{
    public class UserRepository : IUserRepository
    {
       private readonly MapperConfiguration config = new MapperConfiguration(cfg =>

              cfg.CreateMap<User, UserResponse>()
            );
        private readonly RmContext Context;
        public UserRepository(RmContext context)
        {
            Context = context;
        }

        public User Create(User entity)
        {
            Context.Set<User>().Add(entity);
            Context.SaveChanges();
            return entity;
            
        }

        public void Delete(User entity)
        {
            if (entity != null)
            {
                User? toDelete = Context.Set<User>().FirstOrDefault(x => x.Id == entity.Id);
                Context.Set<User>().Remove(toDelete);
                Context.SaveChanges();
            }
           
        }

        public List<UserResponse> GetAll()
        {      
            
            var mapper = new Mapper(config);
            var user = Context.Set<User>().ToList();
            List<UserResponse> responseList = mapper.Map<List<UserResponse>>(user);
            return responseList;
        }

        public UserResponse GetById(int id)
        {
            User? model = Context.Set<User>().FirstOrDefault(x => x.Id == id);
           // UserResponse us = new UserResponse(model);
            var mapper = new Mapper(config);
            UserResponse us = mapper.Map<UserResponse>(model);
            Context.SaveChanges();
            return us;
        }

        public User Update(User entity)
        {
            Context.Update(entity);
            Context.SaveChanges();
            return entity;
        }
    }
}

