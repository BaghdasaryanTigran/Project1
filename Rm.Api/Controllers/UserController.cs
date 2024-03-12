
using Microsoft.AspNetCore.Mvc;
using Rm.DAL.Repositories.Interface;
using Rm.Model.Models;
using Rm.BLL.Interfaces;
using iText.Kernel.Pdf;
using System.Text;
using System.IO;
using System.Buffers.Text;
using iText.StyledXmlParser.Node;

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

            if (await Service.UploadImage(userId, image) == false)
            {
                return BadRequest();
            }
            return Ok();
        }
        [HttpGet]
        [Route("GetImage")]
        public IActionResult GetUserImage(int userId)
        {
            var image = Service.GetUserImage(userId);
            
            return File(image.Item1, image.Item2);
        }
        [HttpPost]
        [Route("UpdateImage")]
        public async Task<IActionResult> UpdateImage(int userId, IFormFile image)
        {
            if(await Service.UpdateImage(userId, image))
            {
                return Ok();
            }
            return Conflict();
        }

        [HttpPost]
        [Route("Image64")]
        public async Task<IActionResult> CreateBase64(int userId, IFormFile image)
        {
            if (await Service.UploadImageBase64(userId, image) == false)
            {
                return Conflict();
            }  
            return Ok();

        }

        [HttpPost]
        [Route("GetImage64")]
        public IActionResult GetImageBase64(int userId)
        {

            var result = Service.GetImage64(userId);
            return File(result.Item1, result.Item2);

        }
        [HttpPost]
        [Route("UpdateImage64")]
        public async Task<IActionResult> UpdateImage64(int userId, IFormFile image)
        {
            if (await Service.UpdateImage64(userId, image))
            {
                return Ok();
            }
            return NotFound();
        }

    }
}
