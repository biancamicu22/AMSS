using DataLibrary.DTO;
using DataLibrary.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace MMSAPI.Repository
{
    public interface IUserRepository : IBaseRepository<User>
    {
    }
}
