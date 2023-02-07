using DataLibrary.DTO;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Identity.Repository
{
    public interface IUserRepository
    {
        Task<User> UpdateUser(UserDTO userDTO);
        Task<User> GetById(string id);
    }
}
