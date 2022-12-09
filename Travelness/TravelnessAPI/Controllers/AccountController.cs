using AutoMapper;
using DataAccessLayer.Entities;
using DataAccessLayer.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TravelnessAPI.Managers;
using TravelnessAPI.Models.User;
using TravelnessAPI.Utils;

namespace TravelnessAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly UserManager userManager;
        private readonly IConfiguration configuration;
        private readonly IMapper mapper;

        public AccountController(IUnitOfWork unitOfWork, UserManager userManager, IConfiguration configuration, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.userManager = userManager;
            this.configuration = configuration;
            this.mapper = mapper;
        }

        [HttpPost("register")]
        public async Task Register([FromForm] UserRegisterViewModel model)
        {

            var url = new Uri(configuration["ClientUrl"]);
            var uri = new UriBuilder(
                url.Scheme,
                url.Host,
                url.Port,
                "emailConfirmation");
            try
            {
                userManager.Register(uri, model);
            }
            catch(ArgumentException e)
            {
                Response.StatusCode = 400;
                await Response.WriteAsync(e.Message);
            }
        }

        [HttpGet("confirmRegistration")]
        public void ConfirmRegistration(string username, string token)
        {
            if (token == null)
            {
                throw new Exception("token is null");
            }

            var user = unitOfWork.Users.Get(x => x.Username == username).FirstOrDefault();
            if (user == null)
            {
                throw new Exception("Invalid username");
            }

            if (user.IsConfirmed)
            {
                throw new Exception("User email is already registered");
            }

            var confirmation = userManager.IsOriginToken(user.Email, token);
            if (confirmation)
            {
                user.IsConfirmed = true;
                unitOfWork.Users.Update(user);
                unitOfWork.Commit();
            }
        }

        [HttpPost("token")]
        public async Task<IActionResult> Token([FromForm] UserLoginViewModel model)
        {
            var user = userManager.CheckPassword(model.Email, model.Password);
            if (user == null)
            {
                return BadRequest("Invalid username or password.");
            }

            var identity = GetIdentity(user);

            var now = DateTime.UtcNow;
            // создаем JWT-токен
            var jwt = new JwtSecurityToken(
                    issuer: AuthOptions.ISSUER,
                    audience: AuthOptions.AUDIENCE,
                    notBefore: now,
                    claims: identity.Claims,
                    expires: now.Add(TimeSpan.FromDays(AuthOptions.LIFETIME)),
                    signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            var response = new
            {
                access_token = encodedJwt,
                user = mapper.Map<UserResponseViewModel>(user)
            };
            return Ok(response);
        }

        private ClaimsIdentity GetIdentity(User user)
        {
            var claims = new List<Claim>
                {
                    new Claim("Id",user.Id.ToString()),
                    new Claim(ClaimTypes.Role, user.Role)
                };
            ClaimsIdentity claimsIdentity =
            new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            return claimsIdentity;
        }
    }
}
