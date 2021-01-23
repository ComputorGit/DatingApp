using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using API.Entities;
using API.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace API.Services
{
    public class TokenService : ITokenService
    {

        private readonly SymmetricSecurityKey _key;
        //Symmetricsecuritykey will use one key for encryption and decryption
        //https and ssl use two key public and privated i.e assymetricsecuritykey
        public TokenService(IConfiguration config)
        {
            _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));
        }
        public string CreateToken(AppUser user) //generate Token
        {
           var claims = new List<Claim>{
               new Claim(JwtRegisteredClaimNames.NameId, user.UserName)
           };

           var creds = new SigningCredentials(_key,SecurityAlgorithms.HmacSha512Signature);
           var tokenDescriptor = new SecurityTokenDescriptor{
               Subject = new ClaimsIdentity(claims),
               Expires = DateTime.Now.AddDays(7),
               SigningCredentials = creds
            
           };

           var tokenHandler = new JwtSecurityTokenHandler();

           var token = tokenHandler.CreateToken(tokenDescriptor);

           return tokenHandler.WriteToken(token);
        }
    }
}