using Rm.Model.Models;


namespace Rm.DAL.Repositories.Interface
{
    public interface IUserRepository
    {
        public Task<List<UserResponse>> GetAll();
        public Task<UserResponse> GetById(int id);
        public Task<User> Create(User entity);
        public Task<User> Update(User entity);
        public Task Delete(User entity);
    }
}
