using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Rm.BLL.Interfaces;
using Rm.DAL.Context;
using Rm.Model.Models;
using System.Security.Cryptography;
using System.Text;
using System.IO.Compression;
using Microsoft.Extensions.Logging.Abstractions;

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
            if(IsUserExistById(userId) || user.ImagePath != null) 
            {
                return false;
            }
            string fileName = $"Img{userId}.jpg";
            string path = Path.Combine($"D:\\Rem\\Images" , fileName) ;
            user.ImagePath = path ;
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
        public async Task<bool> UpdateImage(int userId, IFormFile image)
        {
            var user = Context.Users.FirstOrDefault(x => x.Id == userId);
            if (File.ReadAllBytes(user.ImagePath) == null)
            {
                return false;
            }
            File.Delete(user.ImagePath);
            using (var stream = new FileStream(user.ImagePath, FileMode.CreateNew))
            {
              
               await image.CopyToAsync(stream);
            }
            return true;

        }
    
     
      
        ////////////////////////////////////////////////////////////////////////////////
        
    
        public async Task<bool> UploadImageBase64(int userId,IFormFile file)
        {
            var user = Context.Users.FirstOrDefault(x => x.Id == userId);
            if (IsUserExistById(userId) == false || user.ImageByte != null)
            {
                return false;
            }

            using (MemoryStream memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                //string base64 = Convert.ToBase64String(memoryStream.ToArray());
                byte[] bytes = memoryStream.ToArray();
                user.ImageByte = bytes;
                await Context.SaveChangesAsync();

                return true;
            }
        }

       
        public (byte[],string) GetImage64(int userId)
        {

            var user = Context.Users.FirstOrDefault(x => x.Id == userId);
            if (user.ImageByte == null)
            {
                return (null, null);
            }

           
            //string base64  = Convert.ToBase64String(user.ImageByte);
            string extension = GetImageByteType(user.ImageByte);
             return (user.ImageByte, extension);

        }
        public async Task<bool> UpdateImage64(int userId, IFormFile image)
        {
            var user = Context.Users.FirstOrDefault(x => x.Id == userId);
            if (user.ImageByte == null)
            {
                return false;
            }
            using (MemoryStream memoryStream = new MemoryStream())
            {
                await image.CopyToAsync(memoryStream);
                byte[] bytes = memoryStream.ToArray();
                user.ImageByte = bytes;
            }
            Context.Update(user);
            await Context.SaveChangesAsync();
            return true;
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
                    return null;
            }

        }
        private static string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "image/png";
                case "/9J/4":
                    return "image/jpeg";
                default:
                    return string.Empty;
            }
        }
        private string GetImageByteType(byte[] imageBytes)
        {
  
            if (imageBytes.Length >= 2 && imageBytes[0] == 0xFF && imageBytes[1] == 0xD8)
            {
                return "image/jpeg";
            }
       
            if (imageBytes.Length >= 8 &&
                imageBytes[0] == 0x89 && imageBytes[1] == 0x50 &&
                imageBytes[2] == 0x4E && imageBytes[3] == 0x47 &&
                imageBytes[4] == 0x0D && imageBytes[5] == 0x0A &&
                imageBytes[6] == 0x1A && imageBytes[7] == 0x0A)
            {
                return "image/png";
            }
            if (imageBytes.Length >= 8 &&
                imageBytes[0] == 0x00 && imageBytes[1] == 0x00 &&
                imageBytes[2] == 0x01 && imageBytes[3] == 0x00)
            {
                return "image/x-icon"; // .ico file type
            }
            return null;
        }

    }
}
