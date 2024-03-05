using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Rm.BLL.Interfaces;
using Rm.DAL.Context;
using Rm.Model.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Rm.BLL
{
    public class TokenService : ITokenService
    {
        private readonly RmContext Context;
        private readonly IConfiguration Config;
        private readonly IUserService User;
        public TokenService(IConfiguration config,RmContext cotext, IUserService user)
        {
            Context = cotext;
            Config = config;
            User = user;
        }

        public List<string> TokenDecode(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var tk = handler.ReadToken(token);
            var tokenS = tk as JwtSecurityToken;
            var claims = tokenS.Claims.Select(x => x.Value).ToList();
           
            return claims;
            
        }

        public string TokenGenerator(string userLogin)
        {
            var user = Context.Set<User>().FirstOrDefault(x => x.Login == userLogin);
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier , user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Surname , user.Surname),
                new Claim(ClaimTypes.DateOfBirth, user.Age.ToString()),
                new Claim(ClaimTypes.Role , User.GetRole(user.RoleId))

            };
            var secToken = new JwtSecurityToken(Config["Jwt:Issuer"], Config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(60),
                signingCredentials: credentials);
            var token = new JwtSecurityTokenHandler().WriteToken(secToken);
            var jwt = new Token
            {
                Token1 = token,
                UserId = user.Id,
                CreationDate = DateTime.Now,
                ExpiryDate = DateTime.Now.AddMinutes(60)
            };
            Context.Set<Token>().Add(jwt);
            Context.SaveChanges();
            return token;
        }


    }
}
