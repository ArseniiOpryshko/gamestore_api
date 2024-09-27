using GameStore.Dtos.UserDtos;
using GameStore.Models.Users;
using GameStore.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using GameStore.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.Extensions.Configuration;

namespace GameStore.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : Controller
    {
        private readonly IUserRepository repository;
        private readonly IConfiguration configuration;

        public UserController(IUserRepository repository, IConfiguration configuration)
        {
            this.repository = repository;
            this.configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<ActionResult<string>> Login([FromBody] LoginDto dto)
        {
            User? user = await repository.GetByEmail(dto.Email);

            if (user == null)
                return BadRequest("Invalid Email");

            if (!VerifyPassword(dto.Password, user.PasswordHash, user.PasswordSalt))
                return BadRequest("Invalid password");

            string jwt = CreateToken(user);

            HttpContext.Response.Cookies.Append("token", jwt,
                new CookieOptions
                {
                    Expires = DateTime.Now.AddDays(14),
                    HttpOnly = false,
                    Secure = true,
                    IsEssential = true,
                    SameSite = SameSiteMode.None
                });

            return Ok("Success");
        }

        [HttpPost("Register")]
        public async Task<ActionResult<string>> Register(RegistrationDto dto)
        {
            CreatePasswordHash(dto.Password, out byte[] passwordHash, out byte[] passwordSalt);

            User user = new User()
            {
                Email = dto.Email,
                Nickname = dto.Nickname,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
            await repository.Create(user);

            return Ok("User was created");
        }
        private bool VerifyPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
        private string CreateToken(User user)
        {
            var identity = GetIdentity(user);
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
               claims: identity.Claims,
               issuer: configuration["Jwt:Issuer"],
               audience: configuration["Jwt:Audience"],
               expires: now.Add(TimeSpan.FromHours(AuthOptions.LIFETIME)),
               signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha512Signature));

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            return encodedJwt;
        }
        private ClaimsIdentity GetIdentity(User user)
        {
            var claims = new List<Claim>
                {
                    new Claim("Id", Convert.ToString(user.Id)),
                    new Claim("Nickname", Convert.ToString(user.Nickname)),
                    new Claim("Email", Convert.ToString(user.Email)),
                    new Claim("CartId", Convert.ToString(user.Cart!.Id)),
                };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);

            return claimsIdentity;
        }
    }
}
