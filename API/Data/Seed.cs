using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data
{
    public class Seed
    {
        private readonly DataContext context;
        public static async Task SeedUsers(DataContext context)
        {

            //if user is present in db, then do not seed the data
            if(await context.Users.AnyAsync()) return;
            //else
            var userData = await System.IO.File.ReadAllTextAsync("Data/UserSeedData.json");
            var users = JsonSerializer.Deserialize<List<AppUser>>(userData);

            foreach(var user in users){
                using var hmac = new HMACSHA512();
                user.UserName = user.UserName.ToLower();
                user.PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes("Password"));

                context.Users.Add(user);


            }
            await context.SaveChangesAsync();


        }
    }
}