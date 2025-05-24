using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace PrimeiraAPI.Services
{
    public class TokenService : ITokenService
    {
        public JwtSecurityToken GenerateAccessToken(IEnumerable<Claim> claims, IConfiguration configuration)
        {
            var key = configuration.GetSection("JWT").GetValue<string>("SecretKey") ?? throw new InvalidOperationException("Invalid secret key"); // Pegando a senha do JWT

            var privateKey = Encoding.UTF8.GetBytes(key); // Converte para bytes 

            var signingCredentials = new SigningCredentials(new SymmetricSecurityKey(privateKey), SecurityAlgorithms.HmacSha256Signature); // Cria a chave de assinatura    

            var tokenDescription = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims), // Adiciona os claims do usuário
                Expires = DateTime.UtcNow.AddMinutes(configuration.GetSection("JWT").GetValue<double>("TokenValidityInMinutes")),// Tempo de expiração do token
                Audience = configuration.GetSection("JWT").GetValue<string>("ValidAudience"),
                Issuer = configuration.GetSection("JWT").GetValue<string>("ValidIssuer"), // Emissor
                SigningCredentials = signingCredentials // Chave de assinatura
            };

            var tokenHandler = new JwtSecurityTokenHandler(); // Cria o manipulador de token para gerar o token
            var tokenCreate = tokenHandler.CreateJwtSecurityToken(tokenDescription); // Cria o token

            return tokenCreate;
        }

        public string GenerateRefreshToken()
        {
            var secureRadomBytes = new byte[128];

            using var random = RandomNumberGenerator.Create(); // Cria um gerador de números aleatórios

            random.GetBytes(secureRadomBytes); // Preenche o array com bytes aleatórios


            var refreshToken = Convert.ToBase64String(secureRadomBytes); // Converte os bytes para string

            return refreshToken; // Retorna o token

        }

        public ClaimsPrincipal GetPrincipalFromExpiredToken(string token, IConfiguration configuration)
        {

            var secretKey = configuration["JWT:SecretKey"] ?? throw new InvalidOperationException("Invalid key"); // Pega a chave secreta


            var tokenValidationParameters = new TokenValidationParameters   // Define o que será validado
            {
                ValidateAudience = false,
                ValidateIssuer = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey)),
                ValidateLifetime = false,
            };

            var tokenHandler = new JwtSecurityTokenHandler();  // Cria um manipulador

            var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken); // Valida o token, retorna o usuário e retorna um objeto JwtSecurityToken

            if (securityToken is not JwtSecurityToken jwtSecurityToken || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
            {
                throw new SecurityTokenException("Invalid token");
            }

            return principal;
        }
    }
}
