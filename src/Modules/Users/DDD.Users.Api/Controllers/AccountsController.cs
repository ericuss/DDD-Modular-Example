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

    [Authorize, Route(UrlApiUsers)]
    public class AccountsController : BaseEmptyApiController, IAccountsController
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountsController> _logger;

        public AccountsController(
            SignInManager<ApplicationUser> signInManager,
            ILoggerFactory loggerFactory)
        {
            this._signInManager = signInManager;
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

    }
}
