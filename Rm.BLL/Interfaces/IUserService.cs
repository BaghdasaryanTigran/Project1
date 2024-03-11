using Microsoft.AspNetCore.Http;
using Rm.Model.Models;


namespace Rm.BLL.Interfaces
{
    public interface IUserService
    {
        public string GetRole(short roleId);
        public string GetRoleByUserId(int userId);
        public bool IsUserExist(User user, bool includePassword = false);
        public bool IsUserExistById(int id);
        public bool IsUserLoginExist(User user);
        public  string HashPassword(string password);
        public Task<bool> UploadImage(int userId, IFormFile image);
        public (byte[], string) GetUserImage(int userId);
        public Task<bool> UploadImageBase64(int userId, IFormFile file);
        public (byte[], string) GetImage64(int userId);
    }
}
