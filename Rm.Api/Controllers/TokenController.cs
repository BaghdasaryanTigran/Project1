using Microsoft.AspNetCore.Mvc;
using Rm.BLL.Interfaces;
using Rm.Model.Models;

namespace Rm.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService Token;
        private readonly IUserService Service;
        public TokenController(ITokenService token, IUserService service)
        { 
            Token = token;
            Service = service;
        }


        [HttpPost]
        [Route("User Validation")]
        public IActionResult Validate(User user)
        {
            if (Service.IsUserExist(user , true))
            {
                var token = Token.TokenGenerator(user.Login);
                return Ok($"Token : {token}");
            }
            return Conflict("User Not Found");

        }

        [HttpGet]
        [Route("TokenClaims")]
        public List<string> GetTokenClaims(string token)
        {
            var claims =  Token.TokenDecode(token);
            return claims;
        }

    }
}
