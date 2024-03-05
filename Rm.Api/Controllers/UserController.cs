
using Microsoft.AspNetCore.Mvc;
using Rm.DAL.Repositories.Interface;
using Rm.Model.Models;
using Rm.BLL.Interfaces;
using iText.Kernel.Pdf;
using System.Text;
using System.IO;
using System.Buffers.Text;

namespace Rm.Api.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository Repository;
        private readonly IUserService Service;
        public UserController(IUserService service, IUserRepository repository)
        {
            Repository = repository;
            Service = service;

        }


        [HttpGet]
        [Route("GetRoleById")]
        public string GetRoleById(int userId)
        {
            if (Service.IsUserExistById(userId))
            {
                string role = Service.GetRoleByUserId(userId);
                return role;
            }
            return "User Not Found";
        }

        [HttpGet]
        [Route("GetRole")]
        public string GetRole(short roleId)
        {
            string role = Service.GetRole(roleId);
            return role;
        }



        [HttpGet]
        [Route("GetAll")]
        public IEnumerable<UserResponse> GetAll()
        {
            return Repository.GetAll();
        }


        [HttpGet]
        [Route("GetById")]
        public UserResponse Get(int id)
        {
            if (Service.IsUserExistById(id))
            {
                return Repository.GetById(id);
            }
            return null;
        }


        [HttpPost]
        public IActionResult Create(User user)
        {
            if (Service.IsUserExist(user))
            {
                return Conflict("Already Exist");
            }
            var us = new User
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Surname = user.Surname,
                Age = user.Age,
                Login = user.Login,
                Password = Service.HashPassword(user.Password),
                RoleId = user.RoleId,

            };
            Repository.Create(us);
            return Ok("User added");
        }


        [HttpPut]
        public IActionResult Update(int userIdForUpdate, User userUpdate)
        {

            if (Service.IsUserExistById(userIdForUpdate) && Service.IsUserLoginExist(userUpdate) == false)
            {
                Repository.Update(userUpdate);
                return Ok("User Updated");
            }
            return Conflict("User Not Found or Already Exist");
        }


        [HttpDelete]
        public IActionResult Delete(User user)
        {
            if (Service.IsUserExist(user, true))
            {
                Repository.Delete(user);
                return Ok("Delete Successful");
            }
            return Conflict("User does not Exist");
        }
        [HttpPost]
        [Route("UserImage")]
        public async Task<IActionResult> UploadImage(int userId, IFormFile image)
        {
            if (Service.IsUserExistById(userId) == false)
            {
                return BadRequest();
            }
            await Service.UploadImage(userId, image);
            return Ok();
        }
        [HttpGet]
        [Route("GetImage")]
        public IActionResult GetUserImage(int userId)
        {
            var image = Service.GetUserImage(userId);
            
            return File(image.Item1, image.Item2);
        }

        //stexic sax nayel
        [HttpPost]
        [Route("hhhhhh")]
        public IActionResult HHH(IFormFile a)
        {
           string b = Service.UploadBase64(a);
            return Ok(b);

        }
     
        [HttpPost]
        [Route("GetBase")]
        public IActionResult BBB(string a )
        {



            byte[] array = Convert.FromBase64String(a);
           
            return File(array, GetFileExtension(a));


        }
        private static string GetFileExtension(string base64String)
        {
            var data = base64String.Substring(0, 5);

            switch (data.ToUpper())
            {
                case "IVBOR":
                    return "png";
                case "/9J/4":
                    return "image/jpeg";
                case "AAAAF":
                    return "mp4";
                case "JVBER":
                    return "pdf";
                case "AAABA":
                    return "ico";
                case "UMFYI":
                    return "rar";
                case "E1XYD":
                    return "rtf";
                case "U1PKC":
                    return "txt";
                case "MQOWM":
                case "77U/M":
                    return "srt";
                default:
                    return string.Empty;
            }
        }

    }
}
