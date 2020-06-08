using System;
using System.Security.Claims;
using System.Threading.Tasks;
using DatingApp.API.Data;
using DatingApp.API.Models;
using DatingApp.API.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Configuration;
using Microsoft.AspNetCore.Authorization;

namespace DatingApp.API.Controllers
{
    // [Authorize]
    [ApiController]
    [Route("api/[Controller]")]
    public class AuthController : ControllerBase
    {
        public IAuthRepository _repo { get; }
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo,IConfiguration config)
        {
            this._config = config;
            this._repo = repo;

        }
        [HttpPost("register")]
        public async Task<IActionResult> Register (UserForRegisterDto userForRegisterDto)
        {
            // if(!ModelState.IsValid)[FromBody]
            // {
            //     return BadRequest(ModelState);
            // }
            userForRegisterDto.UserName=userForRegisterDto.UserName.ToLower();
            if(await _repo.UserExsit(userForRegisterDto.UserName))
            {
                return BadRequest("User Name Alredy Exsit");
            }
            var user=new User
            {
              Username=userForRegisterDto.UserName,
              
            };
            var createUser=await _repo.Register(user,userForRegisterDto.Password);
            return StatusCode(201);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDto userForLoginDto)
        { 
            //  throw new Exception("moshkel login");
           var userLogin=await _repo.Login(userForLoginDto.Username.ToLower(),userForLoginDto.Password);
           if(userLogin==null)
           {
               return Unauthorized();
           }
           var claim=new []
           {
               new Claim(ClaimTypes.NameIdentifier,userLogin.Id.ToString()),
               new Claim(ClaimTypes.Name,userLogin.Username)
           };
             var key=new SymmetricSecurityKey (Encoding.UTF8.GetBytes(_config.GetSection("AppSettings:Token").Value));
             var creds=new SigningCredentials(key,SecurityAlgorithms.HmacSha256Signature);
             var tokrnDiscriptor=new SecurityTokenDescriptor
             {
                 Subject=new ClaimsIdentity(claim),
                 Expires=DateTime.Now.AddDays(1),
                 SigningCredentials=creds
             };
             var tokenHandler=new JwtSecurityTokenHandler();
             var token=tokenHandler.CreateToken(tokrnDiscriptor);
        return Ok(new {
           token=tokenHandler.WriteToken(token)
        });
        }
    

    }
}