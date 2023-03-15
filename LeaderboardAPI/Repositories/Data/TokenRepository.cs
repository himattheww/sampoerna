using LeaderboardAPI.Entities;
using LeaderboardAPI.Repositories.Interface;
using LeaderboardAPI.ViewModels.Output;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace LeaderboardAPI.Repositories.Data
{
    public class TokenRepository : IToken
    {
        private readonly LeaderboardContext _context;
        public IConfiguration? _configuration;
        public TokenRepository(IConfiguration configuration, LeaderboardContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public string GenerateJwtToken(string customerCode, string roleType)
        {
            var claims = new List<Claim>
                {
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(ClaimTypes.NameIdentifier,customerCode),
                    new Claim(ClaimTypes.Role,roleType)
                };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var creds = new SigningCredentials(
                key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(20),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
