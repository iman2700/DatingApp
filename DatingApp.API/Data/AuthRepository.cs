using System.Reflection.Metadata;
using System.Security.AccessControl;
using System;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using DatingApp.API.Models;

namespace DatingApp.API.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly DataContext context;
         
          public AuthRepository(DataContext _context)
          {
           context=_context;
          }
          public async Task<User>Login(string username,string password)
         {
            var user = await context.Users.FirstOrDefaultAsync(x => x.Username == username);
            if (user == null)
            return null;

            if (!VarifyPasswordHash(password,user.PasswordHash,user.PasswoedSalt))
              return null;

            return user;
         }

        private bool VarifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
          using(var hamc=new System.Security.Cryptography.HMACSHA512(passwordSalt))
         {
          var computHash=hamc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
          for (int i=0;i< computHash.Length;i++)
          {
            if(computHash[i]!=passwordHash[i])
             return false;
          }
          return true;
         } 
        }

        public async Task<User>Register(User user,string password)
        {
            byte[] passwordHash ;
            byte[] passwordSalt ;
            CreatePasswordHash(password,out passwordHash,out passwordSalt);
            user.PasswordHash=passwordHash;
            user.PasswoedSalt=passwordSalt;
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
            return user;
        }
     
        private void CreatePasswordHash(string password,out byte[]  passwordHash,out byte[] passwordSalt)
        {
          
         using(var hamc=new System.Security.Cryptography.HMACSHA512())
         {
           passwordSalt=hamc.Key;
           passwordHash=hamc.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
         }
        }
        public async Task<bool> UserExsit(string username)
        {
          if(await context.Users.AnyAsync(x=>x.Username==username))
          return true;

          return false;
      
        } 
        
    }
}