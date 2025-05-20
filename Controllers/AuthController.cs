using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PrimeiraAPI.DTOs;
using PrimeiraAPI.Models;
using PrimeiraAPI.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace PrimeiraAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly ITokenService _tokenService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public AuthController(ITokenService tokenService, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            _tokenService = tokenService;
            _userManager = userManager;
            _roleManager = roleManager;
            _configuration = configuration;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModelDTO loginModelDTO)
        {
            var user = await _userManager.FindByNameAsync(loginModelDTO.UserName!); // Encontra o usuário

            // Valida o usuário e a senha fornecida
            if (user is not null && await _userManager.CheckPasswordAsync(user, loginModelDTO.Passsword!))
            {
                // Obtém os roles do usuário
                var userRoles = await _userManager.GetRolesAsync(user);

                // Cria uma lista das claims do usuário
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, loginModelDTO.UserName!),
                    new Claim(ClaimTypes.Email, loginModelDTO.Email!),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), 
                    
                };

                foreach(var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));  // Adiciona uma claim da lista feita para cada role do usuário.
                }

                // Gerando o token e passsando as claims
                var token = _tokenService.GenerateAccessToken(authClaims, _configuration);

                // Gerando o refresh token 
                var refreshToken = _tokenService.GenerateRefreshToken();

                // Obtém o tempo do refresh token e transforma para o tipo int 
                _ = int.TryParse(_configuration["JWT:RefreshTokenValidityInMinutes"], out int refreshTokenValidityInMinutes);


                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.Now.AddMinutes(refreshTokenValidityInMinutes);

                // Atualiza as informações do banco de dados. 
                await _userManager.UpdateAsync(user);

                // Retorna o token gerado, refresh token e a data de expiração.
                return Ok(new
                {
                    Token = new JwtSecurityTokenHandler().WriteToken(token),
                    RefreshToken = refreshToken,
                    Expiration = token.ValidTo
                });
            }

            return Unauthorized();
        }
    }
}
