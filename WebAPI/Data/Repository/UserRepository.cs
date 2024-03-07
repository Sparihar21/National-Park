using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Data.Repository.IRepository;
using WebAPI.Models;

namespace WebAPI.Data.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly AppSettings _appSettings;
        public UserRepository(ApplicationDbContext context, IOptions<AppSettings> appSettings)
        {
            _context = context;
            _appSettings = appSettings.Value;
        }

        public User Autehnticate(string userName, string Password)
        {
            var userInDb = _context.Users.FirstOrDefault(x => x.UserName == userName && x.Password==Password);
            if (userInDb == null) return null;
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.Secret);
            var tokenDescritor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name,userInDb.UserName),
                    new Claim(ClaimTypes.Role,userInDb.Role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescritor);
            userInDb.Token = tokenHandler.WriteToken(token);
            userInDb.Password = "";
            return userInDb;
        }

        public bool IsUniqueUserName(string userName)
        {
            var userInDb = _context.Users.FirstOrDefault(x => x.UserName == userName);
            if (userInDb == null) return true;return false;
        }

        public User Register(string userName, string Password)
        {
            User user = new User()
            {
                UserName = userName,
                Password = Password,
                Role = "Admin",
            };
            _context.Users.Add(user);
            _context.SaveChanges();
            return user;
        }
    }
}
