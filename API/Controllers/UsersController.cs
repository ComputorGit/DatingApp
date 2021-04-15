using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Data;
using API.DTOs;
using API.Entities;
using API.Extensions;
using API.Helpers;
using API.Interfaces;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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

        //ERROR :- Uncomment this and multiple constructor is thrown by compiler
        // public UsersController(IMapper _mapper)
        // {
        //     this._mapper = _mapper;

        // }
        public IMapper _mapper { get; }
        private readonly IPhotoService photoService;

        public UsersController(IUserRepository userRepository, IMapper mapper, IPhotoService photoService)
        {
            this.photoService = photoService;
            this._mapper = mapper;
            this._userRepository = userRepository;

        }

        //Asynchronous Method
        [HttpGet]
        [Authorize]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery] UserParams userParams)
        {
            // var users = await _context.Users.ToListAsync();
            //return users;

            //return Ok(await _userRepository.GetUserAsync() );

            // var users = await _userRepository.GetUserAsync();
            // var usersToReturn = _mapper.Map<IEnumerable<MemberDto>>(users);
            // return Ok(usersToReturn);

            var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());
            userParams.CurrentUsername = user.UserName;

            if (string.IsNullOrEmpty(userParams.Gender))
                userParams.Gender = user.Gender == "male" ? "female" : "male";

            var users = await _userRepository.GetMembersAsync(userParams);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize, users.TotalCount, users.TotalPages);
            return Ok(users);


        }

        // api/user/3



        //Authorize is removed for now, as there is no token store during the testing of api 
        [Authorize]
        ///[HttpGet("{id}")]
        [HttpGet("{username}", Name = "GetUsers")]
        public async Task<ActionResult<MemberDto>> GetUsers(string username)
        {
            //return await _context.Users.FindAsync(id);

            // var user = await _userRepository.GetUserByUsernameAsync(username);
            // return _mapper.Map<MemberDto>(user);

            return await _userRepository.GetMemberAsync(username);



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

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto)
        {
            //it will give the username for the user, the api will fetch this by using the token that is helping to validate the user
            var username = User.GetUserName();
            var user = await _userRepository.GetUserByUsernameAsync(username);

            //_mapper will help to save the work like
            //User.City = MemberUpdateDto.City
            _mapper.Map(memberUpdateDto, user);

            _userRepository.update(user);
            if (await _userRepository.SaveAllAsync())
            {
                return NoContent();
            }
            return BadRequest("Failed to Update User");



        }

        //Photoupload functionality
        [HttpPost("add-photo")]
        public async Task<ActionResult<PhotoDTO>> AddPhoto(IFormFile file)
        {
            var user = await _userRepository.GetUserByUsernameAsync(User.GetUserName());
            var result = await photoService.AddPhotoAsync(file);

            if (result.Error != null) return BadRequest(result.Error.Message);

            var photo = new Photo
            {
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };

            if (user.Photos.Count == 0)
            {
                photo.IsMain = true;
            }

            user.Photos.Add(photo);

            if (await _userRepository.SaveAllAsync())
            {
                // return _mapper.Map<PhotoDTO>(photo);
                return CreatedAtRoute("GetUsers", new { username = user.UserName }, _mapper.Map<PhotoDTO>(photo));
            }


            return BadRequest("Problem adding photo");
        }
    }
}