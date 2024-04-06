using VillAPI.DataAccess.Data;
using VillaAPI.Models;
using VillaAPI.Models.Dto;
using VillAPI.DataAccess.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using VillaAPI.Models;

namespace MagicVilla_VillaAPI.Repository
{
    public class UserRepository : IUserRepository
    {

        private readonly ApplicationDbContext _context;

        private string secretKey;
        public UserRepository(ApplicationDbContext context, IConfiguration configuration)
        {
            _context=context;
            secretKey = configuration.GetValue<string>("ApiSettings:Secret");
        }
        public bool IsUniqueUser(string username)
        {
            var user = _context.LocalUsers.FirstOrDefault(l=>l.UserName == username);

            if (user == null)
            {
                return true;
            }
            return false;
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequestDTO)
        {
            var user= _context.LocalUsers.FirstOrDefault(l=>l.UserName.ToLower() == loginRequestDTO.UserName.ToLower()
            && l.Password == loginRequestDTO.Password);

            if (user == null)
            {

                return new LoginResponseDTO()
                {
                    Token = "",
                    User = null
                };
                
            }

            //if user found generate JWT Token

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secretKey);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
               {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
               }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);


            LoginResponseDTO loginResponseDTO = new LoginResponseDTO()
            {
                Token = tokenHandler.WriteToken(token),
                User = user

            };

            return loginResponseDTO;
        }

        public async Task<LocalUser> Register(RegistrationRequestDTO registrationRequestDTO)
        {
            LocalUser user = new()
            {
                Name = registrationRequestDTO.Name,
                Password = registrationRequestDTO.Password,
                UserName = registrationRequestDTO.UserName,
                Role=registrationRequestDTO.Role
            };


            _context.LocalUsers.Add(user);
            await _context.SaveChangesAsync();


            user.Password= string.Empty;

            return user;
        }
    }
}
