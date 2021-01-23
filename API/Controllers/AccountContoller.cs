using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly DataContext _context;
        private readonly ITokenService tokenService;

        public AccountController(DataContext context, ITokenService tokenService)
        {
            this.tokenService = tokenService;
            this._context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AppUser>>> GetAccount()
        {
            var users = await _context.Users.ToListAsync();
            return users;
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (await UserExists(registerDto.UserName))
            {
                // var modelState = "Username is already taken";
                // return new BadRequestObjectResult(modelState);
                return BadRequest("Username is already taken");
            }


            using var hmac = new HMACSHA512();
            var user = new AppUser
            {
                UserName = registerDto.UserName.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(registerDto.Password)),
                PasswordSalt = hmac.Key

            };
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return new UserDto{
                Username = user.UserName,
                Token = tokenService.CreateToken(user)
            };
        }
        
        // //For Querystring registration
        // [HttpPost("register1")]
        // public async Task<ActionResult<UserDto>> RegisterQuery(string UserName, string Password)
        // {
        //     if (await UserExists(UserName))
        //     {
        //         // var modelState = "Username is already taken";
        //         // return new BadRequestObjectResult(modelState);
        //         return BadRequest("Username is already taken");
        //     }


        //     using var hmac = new HMACSHA512();
        //     var user = new AppUser
        //     {
        //         UserName = UserName.ToLower(),
        //         PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(Password)),
        //         PasswordSalt = hmac.Key

        //     };
        //     _context.Users.Add(user);
        //     await _context.SaveChangesAsync();

        //     return new UserDto{
        //         Username = user.UserName,
        //         Token = tokenService.CreateToken(user)
        //     };
        // }

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> login(LoginDto loginDto)
        {
            var user = await _context.Users.SingleOrDefaultAsync(x => x.UserName == loginDto.UserName);
            if (user == null)
                return Unauthorized("Kindly sign up");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.Password));

            for (int i = 0; i < computedHash.Length; i++)
            {
                if (computedHash[i] != user.PasswordHash[i])
                    return Unauthorized("Invalid password");
            }
             return new UserDto{
                Username = user.UserName,
                Token = tokenService.CreateToken(user)
            };
        }



        private async Task<bool> UserExists(string username)
        {
            return await _context.Users.AnyAsync(x => x.UserName == username.ToLower());
        }

    }
}