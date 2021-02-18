using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;
        public UserRepository(DataContext _context, IMapper _mapper)
        {
            this._mapper = _mapper;
            this._context = _context;

        }
        public async Task<MemberDto> GetMemberAsync(string username)
        {
            return await _context.Users
                .Where(x => x.UserName == username)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider)
            .SingleOrDefaultAsync();
        }

        public Task<IEnumerable<MemberDto>> GetMembersAsync()
        {
            throw new System.NotImplementedException();
        }

        async Task<IEnumerable<AppUser>> IUserRepository.GetUserAsync()
        {
            return await _context.Users.Include(p => p.Photos).ToListAsync();
        }

        async Task<AppUser> IUserRepository.GetUserByIdAsync(int id)
        {
            return await _context.
                        Users.FindAsync(id);
        }

        async Task<AppUser> IUserRepository.GetUserByUsernameAsync(string username)
        {
            return await _context.Users
                         .Include(p => p.Photos)
                        .SingleOrDefaultAsync(x => x.UserName == username);
        }

        async Task<bool> IUserRepository.SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        void IUserRepository.update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }
    }
}