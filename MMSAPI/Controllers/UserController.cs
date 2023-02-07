using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataLibrary;
using DataLibrary.DTO;
using DataLibrary.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using MMSAPI.Models;
using MMSAPI.Repository;

namespace MMSAPI.Controllers
{
    [Route("api/v1/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public UserController(MMSContext dbContext,
                              UserManager<User> userManager,
                              SignInManager<User> signInManager,
                              RoleManager<IdentityRole> roleManager, IUserRepository userRepository)
        {
            DbContext = dbContext;
            UserManager = userManager;
            SignInManager = signInManager;
            RoleManager = roleManager;
            UserRepository = userRepository;
        }

        public IUserRepository UserRepository { get; }
        public MMSContext DbContext { get; }
        public UserManager<User> UserManager { get; }
        public SignInManager<User> SignInManager { get; }
        public RoleManager<IdentityRole> RoleManager { get; }

        [HttpGet]
        [Route("all")]
        public IActionResult GetAll()
        {
            try
            {
                var allUsers = UserRepository.GetAll();
                var usersDTO = allUsers.Select(u => UserDTO.FromModel(u)).ToList();
                return Ok(usersDTO);
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> Create([FromBody] UserDTO dto)
        {
            dto.Roles = "USER";
            var user = dto.ToModel();
            try
            {
                user.Id = Guid.NewGuid().ToString();
                var result = await UserManager.CreateAsync(user, dto.Password);
                if (result.Succeeded)
                {
                    await DbContext.SaveChangesAsync();

                    await SignInManager.SignInAsync(user, false);
                    var token = await GenerateJwtToken(dto.Email, user, false);
                    return Ok(new AuthResponse
                    {
                        email = user.Email,
                        jwt = token,
                        username = dto.UserName
                    });
                }
                else
                {
                    return Unauthorized("Datele nu sunt valide!");
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        [HttpGet]
        [Route("me")]
        [Authorize(Roles = "USER")]
        public async Task<ActionResult> GetMe()
        {
            var user = UserRepository.GetById(User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value);
            return Ok(UserDTO.FromModel(user));
        }

        [HttpPost]
        [Route("logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await UserManager.UpdateSecurityStampAsync(await UserManager.GetUserAsync(User));
                return Ok();
            }
            catch
            {
                return Ok();
            }
        }


        [HttpPost]
        [Route("session")]
        public async Task<ActionResult> Login([FromBody] AuthRequest model)
        {
            var userFromModel = await UserManager.FindByNameAsync(model.username);
            if (userFromModel == null) return BadRequest("Credentialele nu sunt invalide!");

            var loginResult = await SignInManager.PasswordSignInAsync(userFromModel.UserName, model.password, false, false);
            if (loginResult.Succeeded)
            {
                var appUser = userFromModel;
                var token = await GenerateJwtToken(model.username, appUser, false);
                return Ok(new AuthResponse
                {
                    username = appUser.UserName,
                    jwt = token,
                    email = appUser.Email
                });
            }

            return BadRequest("Credentialele nu sunt invalide!");
        }

        private async Task<string> GenerateJwtToken(string email, User user, bool? rememberMe)
        {
            var roles = user.Roles.Split(",");
            IdentityOptions _options = new IdentityOptions();

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("UserID", user.Id.ToString()),
            };
            foreach (var r in roles)
            {
                claims.Add(new Claim("ROLE", r.ToUpper()));
                claims.Add(new Claim(ClaimTypes.Role, r.ToUpper()));
            }

            var expires = DateTime.Now.AddMinutes(30);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyWithAMinimumOf16Charachters"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                "FamilyTasks",
                "FamilyTasks",
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPut]
        [Authorize(Roles = "USER")]
        public async Task<ActionResult> Edit([FromBody] UserDTO dto)
        {
            if (User == null) return Unauthorized();

            var userId = User.Claims.FirstOrDefault(c => c.Type == "UserID");
            dto.Id = userId.Value;
            var response = UserRepository.Edit(dto.ToModel());
            var newUser = UserRepository.GetById(dto.Id);

            if (string.IsNullOrEmpty(newUser.Email)) return BadRequest();

            return Ok(new AuthResponse
            {
                username = newUser.UserName,
                jwt = await GenerateJwtToken(newUser.Email, newUser, false),
                email = newUser.Email
            });
        }

        public class PasswordRequest
        {
            public string currentPassword { get; set; }
            public string newPassword { get; set; }
        }

        [HttpPut]
        [Route("me/password")]
        [Authorize(Roles = "USER")]
        public async Task<ActionResult> EditPassword([FromBody] PasswordRequest model)
        {
            try
            {
                var userId = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;

                var user = await UserManager.FindByIdAsync(userId);
                var loginResult = await SignInManager.PasswordSignInAsync(user.UserName, model.currentPassword, false, false);
                if (!loginResult.Succeeded)
                {
                    return BadRequest("Credentialele sunt invalide!");
                }
                var token = await UserManager.GeneratePasswordResetTokenAsync(user);

                var result = await UserManager.ResetPasswordAsync(user, token, model.newPassword);

                loginResult = await SignInManager.PasswordSignInAsync(user.UserName, model.newPassword, false, false);
                return Ok(new AuthResponse
                {
                    username = user.UserName,
                    jwt = await GenerateJwtToken(user.Email, user, false),
                    email = user.Email
                });
            }
            catch (Exception ex)
            {
                return BadRequest();
            }
        }

    }
}