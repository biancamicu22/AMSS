using AutoMapper;
using DataLibrary;
using DataLibrary.DTO;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace Identity.Repository
{
    public class UserRepository : IUserRepository
    {
        public MMSContext DbContext { get; }
        public IMapper _mapper;
        public UserRepository(MMSContext dbContext, IMapper mapper)
        {
            DbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<User> GetById(string id)
        {
            return await System.Threading.Tasks.Task.FromResult(DbContext.Users.Where(a => a.Id == id).FirstOrDefault());
        }

        public Task<User> UpdateUser(UserDTO userDTO)
        {
            var value = DbContext.Users.Where(c => c.Id == userDTO.id).FirstOrDefault();
            if (value != null)
            {
                value.UserName = string.IsNullOrEmpty(userDTO?.username) ? value.UserName : userDTO.username;
                value.FirstName = string.IsNullOrEmpty(userDTO?.firstName) ? value.FirstName : userDTO.firstName;
                value.LastName = string.IsNullOrEmpty(userDTO?.lastName) ? value.LastName : userDTO.lastName;
                value.Email = string.IsNullOrEmpty(userDTO?.email) ? value.Email : userDTO.email;
                value.Birthdate = userDTO.birthdate == null ? value.Birthdate : userDTO.birthdate;
                DbContext.SaveChanges();
                return System.Threading.Tasks.Task.FromResult(value);
            }
            else
            {
                return System.Threading.Tasks.Task.FromResult(new User());
            }
        }
    }
}
