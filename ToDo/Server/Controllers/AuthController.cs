using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ToDo.Shared;

namespace ToDo.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthController : ControllerBase
    {
        //登录
        [HttpPost]
        public UserDto Login(LoginDto dto)
        {
            //模拟获得Token
            var jwtToken = GetToken(dto.UserName);

            return new UserDto {Name = dto.UserName, Token = jwtToken};
        }

        //获得用户，当页面客户端页面刷新时调用以获得用户信息
        [HttpGet]
        public UserDto GetUser()
        {
            if (User.Identity.IsAuthenticated) //如果Token有效
            {
                var name = User.Claims.First(x => x.Type == ClaimTypes.Name).Value; //从Token中拿出用户ID
                //模拟获得Token
                var jwtToken = GetToken(name);

                return new UserDto {Name = name, Token = jwtToken};
            }

            return new UserDto {Name = null, Token = null};
        }

        public string GetToken(string name)
        {
            //此处加入账号密码验证代码

            var claims = new[]
            {
                new(ClaimTypes.Name, name),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("123456789012345678901234567890123456789"));
            var expires = DateTime.Now.AddDays(30);
            var token = new JwtSecurityToken(
                "guetServer",
                "guetClient",
                claims,
                DateTime.Now,
                expires,
                new SigningCredentials(key, SecurityAlgorithms.HmacSha256));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}