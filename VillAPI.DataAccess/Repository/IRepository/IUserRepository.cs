using VillaAPI.Models;
using VillaAPI.Models;
using VillaAPI.Models.Dto;

namespace VillAPI.DataAccess.Repository.IRepository
{
    public interface IUserRepository
    {
        bool IsUniqueUser(string username);

        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO);

        Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDTO);
    }
}
