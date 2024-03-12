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

        public async Task<User> Create(User entity)
        {
            Context.Set<User>().Add(entity);
            await Context.SaveChangesAsync();
            return entity;
            
        }

        public async Task Delete(User entity)
        {
            if (entity != null)
            {
                User? toDelete = Context.Set<User>().FirstOrDefault(x => x.Id == entity.Id);
                Context.Set<User>().Remove(toDelete);
                await Context.SaveChangesAsync();
            }
           
        }

        public async Task<List<UserResponse>> GetAll()
        {      
            
            var mapper = new Mapper(config);
            var user = Context.Set<User>().ToList();
            List<UserResponse> responseList = mapper.Map<List<UserResponse>>(user);
            return responseList;
        }

        public async Task<UserResponse> GetById(int id)
        {
            User? model = Context.Set<User>().FirstOrDefault(x => x.Id == id);
           // UserResponse us = new UserResponse(model);
            var mapper = new Mapper(config);
            UserResponse us = mapper.Map<UserResponse>(model);
            return us;
        }

        public async Task<User> Update(User entity)
        {
            Context.Update(entity);
            await Context.SaveChangesAsync();
            return entity;
        }
    }
}

