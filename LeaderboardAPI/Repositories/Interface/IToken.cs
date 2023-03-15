using LeaderboardAPI.Entities;
using LeaderboardAPI.ViewModels.Input;
using System.IdentityModel.Tokens.Jwt;

namespace LeaderboardAPI.Repositories.Interface
{
    public interface IToken
    {
        string GenerateJwtToken(string customerCode, string roleType);
    }
}
