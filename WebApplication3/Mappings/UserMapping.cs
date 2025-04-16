using AutoMapper;
using WebApplication3.DTO;
using WebApplication3.Model;

namespace WebApplication3.Mappings
{
    public class UserMapping : Profile
    {
        public UserMapping() 
        {
            // UserModel map to -> UserResponseDTO
            CreateMap<UserModel, UserResponseDTO>();
            
        }
    }
}
