using Identity.Api.Contracts.v1;
using Identity.Api.Models;
using Identity.Api.ViewModels.Account;
using IdentityModel;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Identity.Api.Controllers
{
    [ApiController]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IIdentityServerInteractionService _interaction;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AccountController> _logger;

        public AccountController(
            SignInManager<ApplicationUser> signInManager,
            UserManager<ApplicationUser> userManager,
            IIdentityServerInteractionService interaction,
            IConfiguration configuration,
            ILogger<AccountController> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _interaction = interaction;
            _configuration = configuration;
            _logger = logger;
        }

        [Route(ApiRoutes.V1.Account.Login)]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Dictionary<string, IEnumerable<string>>),(int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);

                if (user == null)
                {
                    return NotFound();
                }

                if (await _userManager.CheckPasswordAsync(user, model.Password))
                {
                    var tokenLifetime = _configuration.GetValue("TokenLifetimeMinutes", 120);

                    var props = new AuthenticationProperties
                    {
                        ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(tokenLifetime),
                        AllowRefresh = true
                    };

                    if (model.RememberMe)
                    {
                        var permanentTokenLifetime = _configuration.GetValue("PermanentTokenLifetimeDays", 365);

                        props.ExpiresUtc = DateTimeOffset.UtcNow.AddDays(permanentTokenLifetime);
                        props.IsPersistent = true;
                    };

                    await _signInManager.SignInAsync(user, props);

                    return Ok();
                }
                ModelState.AddModelError("", "Invalid username or password.");
            }
            return BadRequest(ModelState.ToErrorsDictionary());
        }


        [Route(ApiRoutes.V1.Account.Logout)]
        [HttpPost]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType((int)HttpStatusCode.Unauthorized)]
        public async Task<IActionResult> Logout()
        {
            var idp = User?.FindFirst(JwtClaimTypes.IdentityProvider)?.Value;

            if (idp == null)
            {
                return Unauthorized();
            }

            await HttpContext.SignOutAsync();

            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);

            HttpContext.User = new ClaimsPrincipal(new ClaimsIdentity());

            return Ok();
        }

        [Route(ApiRoutes.V1.Account.Register)]
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Dictionary<string, string>),(int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Errors.Any())
                {
                    return Ok();
                }
                else
                {
                    return BadRequest(result.Errors.ToErrorsDictionary());
                }
            }
            return BadRequest(ModelState.ToErrorsDictionary());

        }
    }
}
