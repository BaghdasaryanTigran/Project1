using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Rm.BLL.Interfaces;
using Rm.DAL.Context;
using Rm.Model.Models;
using System.Security.Cryptography;
using System.Text;
namespace Rm.BLL
{
    public class UserService : IUserService
    {
        private readonly RmContext Context;
        public UserService(RmContext context)
        {
            Context = context;
           
        }
        public string HashPassword(string password )
        {
            using (SHA256 sha256 = SHA256.Create())
            {
                
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                StringBuilder sb = new StringBuilder();
                foreach (var item in hashBytes)
                {
                    sb.Append(item.ToString("x2"));
                }
                return sb.ToString();
            }
        }       
        public bool IsUserExist(User user, bool includePassword = false)
        {
            var count = 0;
            if (includePassword)
            {
                 count = Context.Set<User>().Count(x => x.Login == user.Login && x.Password == HashPassword(user.Password));
            }
            else
            {
                 count = Context.Set<User>().Count(x =>  x.Login == user.Login);
            }
            if (count > 0)
            {
                return true;
            }
            return false;
        }
        public bool IsUserExistById(int id)
        {
            var count = Context.Set<User>().Count(x => x.Id == id);
            if (count > 0)
            {
                return true;
            }
            return false;
        }
        public string GetRole(short roleId)
        {
            var role = Context.Set<Role>().FirstOrDefault(x => x.Id == roleId );
            return role.Role1;
        }
        public string GetRoleByUserId(int userId)
        {
            var user = Context.Set<User>().Include(x => x.Role).FirstOrDefault(x => x.Id == userId);
           
            return user.Role.Role1;
        }
        public bool IsUserLoginExist(User user)
        {
            var count = Context.Set<User>().Count(x => x.Login == user.Login);

            if(count > 0)
            {
                return true;
            }
            return false;
        }
        public async Task<bool> UploadImage(int userId, IFormFile image)
        {
            var user = Context.Users.FirstOrDefault(x => x.Id == userId);   
            string fileName = $"Img{userId}.jpg";
            string path = Path.Combine($"D:\\Rem\\Images" , fileName) ;
            using (var stream = new FileStream(path, FileMode.Create))
            {
               await image.CopyToAsync(stream);
            }
            user.ImagePath = path ;
            await Context.SaveChangesAsync();
            return true;
        }
        
        public (byte[], string) GetUserImage(int userId)
        {
            var user = Context.Users.FirstOrDefault(x => x.Id == userId);
            if (string.IsNullOrEmpty(user.ImagePath))
            {
               throw new ArgumentNullException();
            }
            var imageBytes = File.ReadAllBytes(user.ImagePath);
            var imageType = GetContentType(user.ImagePath);
            return (imageBytes,imageType);
        }
        private string GetContentType(string imagePath)
        {
            
            var extension = Path.GetExtension(imagePath).Trim();
            switch (extension)
            {
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                default:
                    return "application/octet-stream";
            }

        }
        public string UploadBase64(IFormFile file)
        {
            

            using (MemoryStream memoryStream = new MemoryStream())
            {
                file.CopyTo(memoryStream);
                byte[] bytes = memoryStream.ToArray();
                string base64String = Convert.ToBase64String(bytes);
                return base64String;
            }
        }

    }
}
