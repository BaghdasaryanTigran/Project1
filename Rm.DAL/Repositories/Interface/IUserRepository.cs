using Rm.Model.Models;


namespace Rm.DAL.Repositories.Interface
{
    public interface IUserRepository
    {
        public List<UserResponse> GetAll();
        public UserResponse GetById(int id);
        public User Create(User entity);
        public User Update(User entity);
        public void Delete(User entity);
    }
}
