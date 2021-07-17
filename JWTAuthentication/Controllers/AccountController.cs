namespace JWTAuthentication.Controllers
{
    using JWTAuthentication.Data;
    using JWTAuthentication.DTOs;
    using JWTAuthentication.Entities;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using System.Threading.Tasks;

    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public AccountController(DataContext dataContext)
        {
            _dataContext = dataContext;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AppUser>> Register(RegisterDTO registerDTO)
        {
            if(await UserExists(registerDTO.Username))
            {
                return BadRequest("UserName is Taken");
            }
             
            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerDTO.Username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDTO.Password)),
                PasswordSalt = hmac.Key
            };

            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            return user;
        } 

        [HttpPost("login")]
        public async Task<ActionResult<AppUser>> login(LoginDTO loginDTO)
        {
            var user = await _dataContext.Users.SingleOrDefaultAsync(x => x.UserName == loginDTO.Username);
            if (user == null) return Unauthorized("Invalid User");

            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));

            for(int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i]) return Unauthorized("Invalid Password");
            }

            return user;
        }
        private async Task<bool> UserExists(string username)
        {
            return await _dataContext.Users.AnyAsync(x=> x.UserName == username.ToLower());
        }


    }
}
