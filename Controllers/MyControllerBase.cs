using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using RestApiNet5.Data.Models;
using RestApiNet5.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace RestApiNet5.Controllers
{
    public class MyControllerBase<T> : ControllerBase
    {
        private User _auth;
        public User AuthUser
        {
            get
            {
                if (HttpContext.User.Identity.IsAuthenticated && _auth == null)
                {
                    var claim = HttpContext.User.Claims.Where(x => x.Type == ClaimTypes.PrimarySid).First();
                    _auth = DbContext.Users.Find(claim.Value);
                }

                return _auth;
            }
        }

        public ILogger<T> Logger { get; set; }
        public AppDbContext DbContext { get; }

        public MyControllerBase(AppDbContext dbContext, ILogger<T> logger)
        {
            Logger = logger;
            DbContext = dbContext;
        }

        [NonAction]
        public void Authorize<TResource>(Func<User, TResource, bool> policy, TResource resource)
        {
            if (!policy.Invoke(AuthUser, resource))
            {
                throw new UnauthorizedAccessException();
            }
        }

        [NonAction]
        public void Authorize(Func<User, bool> policy)
        {
            if (!policy.Invoke(AuthUser))
            {
                throw new UnauthorizedAccessException();
            }
        }
    }

    public static class MyControllerBaseExtensions
    {
        public static string GenerateJwtToken<T>(this MyControllerBase<T> controller, IConfiguration _configuration, User user)
        {
            var claims = new Claim[]
            {
                new Claim(ClaimTypes.PrimarySid, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.Name),
                new Claim(ClaimTypes.Role, user.Role),
            };

            var secretBytes = Convert.FromBase64String(_configuration["Jwt:Key"]);
            var securityKey = new SymmetricSecurityKey(secretBytes);
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

            var securityTokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:Expires"])),
                SigningCredentials = signingCredentials
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var securityToken = jwtTokenHandler.CreateToken(securityTokenDescriptor);

            return jwtTokenHandler.WriteToken(securityToken);
        }
    }
}