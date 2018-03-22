namespace DDD.Users.Api.Controllers
{
    using Microsoft.AspNetCore.Mvc;
    using DDD.Infrastructure.Controllers;
    using static DDD.Users.Api.Configurations.Constants;
    using DDD.Users.Api.IControllers;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using System.Threading.Tasks;
    using DDD.Users.Api.Models;
    using DDD.Users.Domain.Entities;
    using Microsoft.Extensions.Logging;
    using System.Security.Claims;
    using System.IdentityModel.Tokens.Jwt;
    using System;
    using System.Linq;
    using Microsoft.IdentityModel.Tokens;
    using System.Text;
    using DDD.Modules;
    using System.Net;

    [Authorize, Route(UrlApiUsers)]
    public class AccountsController : BaseEmptyApiController, IAccountsController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IPasswordHasher<ApplicationUser> _passwordHasher;
        private readonly Settings _settings;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IPasswordHasher<ApplicationUser> passwordHasher,
            Settings settings,
            ILoggerFactory loggerFactory)
        {
            this._signInManager = signInManager;
            this._userManager = userManager;
            this._passwordHasher = passwordHasher;
            this._settings = settings;
            this._logger = loggerFactory.CreateLogger<AccountsController>();
        }

        [AllowAnonymous, HttpGet("Values")]
        public IActionResult GetValues()
        {
            return this.Ok(new[] { "value1", "value2" });
        }

        [HttpGet("Protected")]
        public IActionResult GetValuesProtected()
        {
            return this.Ok(new[] { "protectedvalue1", "protectedvalue2" });
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model) //, string returnUrl = null)
        {
            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(model.Username, model.Password, model.RememberLogin, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    return this.Ok(result);
                    //if (!string.IsNullOrWhiteSpace(returnUrl))
                    //{
                    //    //return this.Redirect(returnUrl);
                    //}
                    //else
                    //{
                    //    return this.Unauthorized();
                    //    //return this.RedirectToAction("Index", "Diagnostics", null);
                    //}
                }
                //if (result.RequiresTwoFactor)
                //{
                //    return RedirectToAction(nameof(LoginWith2fa), new { returnUrl, model.RememberLogin });
                //}
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");

                    return this.Unauthorized();
                    //return RedirectToAction(nameof(Lockout));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                    return this.Unauthorized();
                    //return View(model);
                }
            }

            // If we got this far, something failed, redisplay form
            //return View(model);
            return this.Unauthorized();
        }


        //[ValidateForm]
        [AllowAnonymous]
        [HttpPost("token")]
        public async Task<IActionResult> CreateToken([FromBody] LoginViewModel model)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(model.Username);
                if (user == null)
                {
                    return Unauthorized();
                }
                if (_passwordHasher.VerifyHashedPassword(user, user.PasswordHash, model.Password) == PasswordVerificationResult.Success)
                {
                    var userClaims = await _userManager.GetClaimsAsync(user);

                    var claims = new[]
                    {
                        new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                        new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                        new Claim(JwtRegisteredClaimNames.Email, user.Email)
                    }.Union(userClaims);

                    var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._settings.JwtSecurityToken.Key));
                    var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

                    var jwtSecurityToken = new System.IdentityModel.Tokens.Jwt.JwtSecurityToken(
                        issuer: this._settings.JwtSecurityToken.Issuer,
                        audience: this._settings.JwtSecurityToken.Audience,
                        claims: claims,
                        expires: DateTime.UtcNow.AddMinutes(5),
                        signingCredentials: signingCredentials
                        );
                    return Ok(new
                    {
                        Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                        Expiration = jwtSecurityToken.ValidTo,
                        Claims = jwtSecurityToken.Claims
                    });
                }
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError($"error while creating token: {ex}");
                return StatusCode((int)HttpStatusCode.InternalServerError, "error while creating token");
            }
        }

        [HttpPost("ValidateToken")]
        public IActionResult ValidateToken()
        {
            try
            {
                if (!Request.Headers.Any(x => x.Key == "Authorization"))
                {
                    return Unauthorized();
                }

                var authHeader = Request.Headers.FirstOrDefault(x => x.Key == "Authorization");
                if (string.IsNullOrWhiteSpace(authHeader.Value))
                {
                    return Unauthorized();
                }

                var authToken = authHeader.Value.ToString().Replace("Bearer ", string.Empty);

                if (string.IsNullOrWhiteSpace(authToken))
                {
                    return Unauthorized();
                }

                SecurityToken validatedToken;

                (new JwtSecurityTokenHandler()).ValidateToken(authToken, new TokenValidationParameters
                {
                    ValidIssuer = this._settings.JwtSecurityToken.Issuer,
                    ValidAudience = this._settings.JwtSecurityToken.Audience,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(this._settings.JwtSecurityToken.Key)),
                    ValidateLifetime = true
                }, out validatedToken);
                var jwtToken = (System.IdentityModel.Tokens.Jwt.JwtSecurityToken)validatedToken;

                return this.Ok(new
                {
                    Token = authToken,
                    Expiration = jwtToken.ValidTo,
                    claims = jwtToken.Claims
                });
            }
            catch (Exception ex)
            {
                _logger.LogError($"error while creating token: {ex}");
                return StatusCode((int)HttpStatusCode.InternalServerError, "error while creating token");
            }
        }
    }
}
