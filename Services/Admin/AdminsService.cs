using AutoMapper;
using LibraryManagement.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using LibraryManagement.Areas.Admin.Models;
using LibraryManagement.DTOs.Admin;

namespace LibraryManagement.Services.Admin
{
    public class AdminsService
    {
        private readonly IMongoCollection<AdminModel> _adminsCollection;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AdminsService(
            IOptions<LMDSettings> libraryManagementDatabaseSettings,
            IMapper mapper,
            IConfiguration configuration)
        {
            var mongoClient = new MongoClient(libraryManagementDatabaseSettings.Value.ConnectionString);
            var database = mongoClient.GetDatabase(libraryManagementDatabaseSettings.Value.DatabaseName);
            _adminsCollection = database.GetCollection<AdminModel>(
                libraryManagementDatabaseSettings.Value.Collections.Admins);
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<AdminModel> RegisterAsync(AdminDTO adminDto)
        {
            var admin = _mapper.Map<AdminModel>(adminDto);
            admin.Password = BCrypt.Net.BCrypt.HashPassword(adminDto.Password);

            await _adminsCollection.InsertOneAsync(admin);
            return admin;
        }

        public async Task<string> LoginAsync(string email, string password)
        {
            var admin = await _adminsCollection.Find(a => a.Email == email).FirstOrDefaultAsync();

            if (admin == null || !BCrypt.Net.BCrypt.Verify(password, admin.Password))
            {
                return null;
            }

            return GenerateJwtToken(admin);
        }

        private string GenerateJwtToken(AdminModel admin)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Name, admin.AdminName),
                    new Claim(ClaimTypes.Role, admin.Role)
                }),
                Expires = DateTime.UtcNow.AddHours(1),
                Issuer = _configuration["Jwt:Issuer"],
                Audience = _configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}