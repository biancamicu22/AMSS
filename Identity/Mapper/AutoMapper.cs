using AutoMapper;
using DataLibrary.DTO;
using DataLibrary.Models;

namespace Identity.Mapper
{
    public class AutoMapper : Profile
    {
        public AutoMapper()
        {
            CreateMap<User, UserDTO>();
        }
    }
}
