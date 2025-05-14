using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PrimeiraAPI.Services
{
    public interface ITokenService
    {

        // Cria um JWT com base nos claims do usuário e configurações do appsettings.
        JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration configuration);

        // Gera uma string aleatória segura, usada para renovar o token quando o AccessToken expirar.
        string GenerateRefreshToken();

        // Extrai as informações do usuário de um token expirado, validando a assinatura sem validar o tempo.
        ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration configuration);
    }
}
