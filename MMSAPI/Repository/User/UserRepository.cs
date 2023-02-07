using DataLibrary;
using DataLibrary.DTO;
using DataLibrary.Models;
using DataLibrary.StatePattern;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MMSAPI.Repository
{
    public class UserRepository : BaseRepository<User>, IUserRepository
    {
        public UserRepository(MMSContext dbContext) : base(dbContext) { }



     
        //public async Task<User> GetById(string id)
        //{
        //    return await Task.FromResult(DbContext.Users.Where(a => a.Id == id).FirstOrDefault());
        //}

        //public Task<User> UpdateUser(UserDTO userDTO)
        //{
        //    var value = DbContext.Users.Where(c => c.Id == userDTO.Id).FirstOrDefault();
        //    if (value != null)
        //    {
        //        value.UserName = string.IsNullOrEmpty(userDTO.UserName) ? value.UserName : userDTO.UserName;
        //        value.FirstName = string.IsNullOrEmpty(userDTO?.FirstName) ? value.FirstName : userDTO.FirstName;
        //        value.LastName = string.IsNullOrEmpty(userDTO?.LastName) ? value.LastName : userDTO.LastName;
        //        value.Email = string.IsNullOrEmpty(userDTO?.Email) ? value.Email : userDTO.Email;
        //        value.Birthdate = userDTO.Birthdate == null ? value.Birthdate : userDTO.Birthdate;
        //        DbContext.SaveChanges();
        //        return Task.FromResult(value);
        //    }
        //    else
        //    {
        //        return Task.FromResult(new User());
        //    }
        //}
    }
}
