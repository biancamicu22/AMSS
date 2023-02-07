using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DataLibrary;
using DataLibrary.Models;
using Identity.Models;
using Identity.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Controllers
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


        [HttpPost]
        [Route("create")]
        public async Task<ActionResult> Create([FromBody] User model)
        {
            model.roles = "USER";
            var user = new User()
            {
                UserName = model.username,
                Email = model.email,
                FirstName = model.firstName,
                LastName = model.lastName,
                Birthdate = model.birthdate,
                Roles = model.roles
            };
            try
            {

                var result = await UserManager.CreateAsync(user, model.password);
                if (result.Succeeded)
                {
                    //await UserManager.AddToRoleAsync(user, model.roles);


                    await DbContext.SaveChangesAsync();

                    await SignInManager.SignInAsync(user, false);
                    var token = await GenerateJwtToken(model.email, user, false);
                    return Ok(new AuthResponse
                    {
                        email = user.Email,
                        jwt = token,
                        username = model.username
                    });
                }
                else
                {

                    foreach (var err in result.Errors)
                    {
                        //resp.ErrorMessage.Add(new CustomErrorMessage { Code = err.Code, Message = err.Description });
                    }
                    return ServiceResponseModel<object>.NotValid("Datele nu sunt valide!").toHttpResponse();
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
            var user = await UserRepository.GetById(User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value);
            return Ok(new UserDTO
            {
                username = user.UserName,
                firstName = user.FirstName,
                lastName = user.LastName,
                birthdate = (DateTime)user.Birthdate,
                email = user.Email,
                id = user.Id
            });
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
            if (userFromModel == null) return ServiceResponseModel<object>.bad(ErrorResponse.InvalidCredentials()).toHttpResponse();

            var loginResult = await SignInManager.PasswordSignInAsync(userFromModel.UserName, model.password, false, false);
            if (loginResult.Succeeded)
            {
                var appUser = userFromModel;// userManager.Users.SingleOrDefault(u => u.Email == model.EmailAddress);
                var token = await GenerateJwtToken(model.username, appUser, false);
                return Ok(new AuthResponse
                {
                    username = appUser.UserName,
                    jwt = token,
                    email = appUser.Email
                });
            }

            return ServiceResponseModel<object>.bad(ErrorResponse.InvalidCredentials()).toHttpResponse();
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

            var expires = DateTime.Now.AddMinutes(30);// Convert.ToDouble(applicationSettings.JWT_ExpireDays));
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("SecretKeyWithAMinimumOf16Charachters"));// applicationSettings.JWT_Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                "FamilyTasks",//applicationSettings.JWT_Issuer,
                "FamilyTasks",//applicationSettings.JWT_Issuer,
                claims,
                expires: expires,
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        [HttpPut]
        [Route("me")]
        [Authorize(Roles = "USER")]
        public async Task<ActionResult> Edit([FromBody] UserDTO model)
        {
            model.id = User.Claims.FirstOrDefault(c => c.Type == "UserID")?.Value;
            var response = await UserRepository.UpdateUser(model);
            if (string.IsNullOrEmpty(response.Email)) return BadRequest();
            return Ok(new AuthResponse
            {
                username = response.UserName,
                jwt = await GenerateJwtToken(response.Email, response, false),
                email = response.Email
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
                    return ServiceResponseModel<object>.bad(ErrorResponse.InvalidCredentials()).toHttpResponse();
                }
                var token = await UserManager.GeneratePasswordResetTokenAsync(user);

                var result = await UserManager.ResetPasswordAsync(user, token, model.newPassword);

                loginResult = await SignInManager.PasswordSignInAsync(user.UserName, model.newPassword, false, false);
                if (!loginResult.Succeeded)
                {
                    return ServiceResponseModel<object>.bad(ErrorResponse.InvalidCredentials()).toHttpResponse();
                }
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
        [HttpGet("test")]
        public string Test()
        {
            return "Test";
        }


    }
}