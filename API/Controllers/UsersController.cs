using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.Controllers
{
    // [ApiController]
    // [Route("api/[Controller]")]
    public class UsersController : BaseApiController
    {
        //private readonly DataContext _context;
        // public UsersController(DataContext context)
        // {
        //     _context = context;
        // }
        private readonly IUserRepository _userRepository;
        public IMapper _mapper { get; }

        public UsersController(IUserRepository userRepository, IMapper mapper)
        {
            this._mapper = mapper;
            this._userRepository = userRepository;

        }

        //Asynchronous Method
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers()
        {
            // var users = await _context.Users.ToListAsync();
            //return users;

            //return Ok(await _userRepository.GetUserAsync() );

            var users = await _userRepository.GetUserAsync();
            var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
            return Ok(usersToReturn);


        }

        // api/user/3



        //Authorize is removed for now, as there is no token store during the testing of api 
        //[Authorize]
        ///[HttpGet("{id}")]
        [HttpGet("{username}")]
        public async Task<ActionResult<MemberDto>> GetUsers(string username)
        {
            //return await _context.Users.FindAsync(id);

            // var user = await _userRepository.GetUserByUsernameAsync(username);
            // return _mapper.Map<MemberDto>(user);

            return  await _userRepository.GetMemberAsync(username);
        


        }



        //Synchronous Method
        // [HttpGet]
        // public ActionResult<IEnumerable<AppUser>> GetUsers(){
        //     var users = _context.Users.ToList();
        //     return users;
        // }

        // // api/user/3
        // [HttpGet("{id}")]
        // public ActionResult<AppUser> GetUser( int id){
        //     return  _context.Users.Find(id);

        // }
    }
}