using System.Security.Claims;

namespace JwtTest.services
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken();
        ClaimsPrincipal GetPrincipleFromExpiredToken(string token);

    }
}

