using Ecommerce.Application.Common.Interfaces;
using Ecommerce.Application.Common.Utility;
using Ecommerce.Application.Services.Interface;
using Ecommerce.Domain.Entities;
using ECommerce.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace ECommerce.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailService _emailService;
        
        public AccountController(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager, IEmailService emailService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _emailService = emailService;
        }
        public IActionResult Login(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            LoginVM loginVM = new()
            {
                RedirectUrl = returnUrl,
            };
            return View(loginVM);
        }
        public IActionResult AccessDenied()
        {
            return View();
        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        public IActionResult Register(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            //if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            //{
            //    _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).Wait();
            //    _roleManager.CreateAsync(new IdentityRole(SD.Role_Customer)).Wait();
            //}

            RegisterVM registerVM = new()
            {
                RoleList = _roleManager.Roles.Select(r => new SelectListItem
                {
                    Text = r.Name,
                    Value = r.Name
                }),
                RedirectUrl = returnUrl,
            };

            return View(registerVM);
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (ModelState.IsValid)
            {
                ApplicationUser user = new()
                {
                    Name = registerVM.Name,
                    Email = registerVM.Email,
                    PhoneNumber = registerVM.PhoneNumber,
                    NormalizedEmail = registerVM.Email.ToUpper(),
                    EmailConfirmed = true,
                    UserName = registerVM.Email,
                    CreatedOn = DateTime.Now
                };

                var result = await _userManager.CreateAsync(user, registerVM.Password);
                if (result.Succeeded)
                {
                    if (!string.IsNullOrEmpty(registerVM.Role))
                    {
                        await _userManager.AddToRoleAsync(user, registerVM.Role);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, SD.Role_Customer);
                    }
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    if (string.IsNullOrEmpty(registerVM.RedirectUrl))
                    {
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        return LocalRedirect(registerVM.RedirectUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            registerVM.RoleList = _roleManager.Roles.Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Name
            });
            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(loginVM.Email, loginVM.Password, loginVM.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByEmailAsync(loginVM.Email);
                    if (await _userManager.IsInRoleAsync(user, SD.Role_Admin))
                    {
                        return RedirectToAction("Index", "Dashboard");
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(loginVM.RedirectUrl))
                        {
                            return RedirectToAction("Index", "Home");
                        }
                        else
                        {
                            return LocalRedirect(loginVM.RedirectUrl);
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("", "Invalid login attempt.");
                }
            }
            return View(loginVM);
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordVM forgotPasswordVM)
        {
            if (ModelState.IsValid)
            {
                var user = _userManager.Users.FirstOrDefault(u => u.Email.ToLower() == forgotPasswordVM.Email.ToLower());
                if (user == null)
                {
                    // Don't reveal if the email is invalid
                    TempData["error"] = "The email is invalid";
                    return RedirectToAction(nameof(ForgotPassword));
                }
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);

                // Create a reset link
                var resetLink = Url.Action("ResetPassword", "Account", new { token, email = forgotPasswordVM.Email }, Request.Scheme);

                // TODO: Send the reset link via email
                _emailService.SendEmail(forgotPasswordVM.Email, "Password Reset",$"<a href='{resetLink}'>Click here to reset your password</a>");

                TempData["resetLink"] = resetLink;

            }
            return RedirectToAction(nameof(ForgotPasswordConfirmation));
        }

        public IActionResult ForgotPasswordConfirmation()
        {
            return View();
        }
        public IActionResult ResetPassword(string token, string email)
        {
            ResetPasswordVM resetPasswordVM = new()
            {
                Token = token,
                Email = email
            };
            return View(resetPasswordVM);
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordVM  resetPasswordVM)
        {
            if (!ModelState.IsValid)
                return View(resetPasswordVM);

            var user = _userManager.Users.FirstOrDefault(u => u.Email.ToLower() == resetPasswordVM.Email.ToLower());
            if (user == null)
            {
                // Don't reveal if the user doesn't exist
                return RedirectToAction("ResetPasswordConfirmation", new { message = "failed"});
            }

            var result = await _userManager.ResetPasswordAsync(user, resetPasswordVM.Token, resetPasswordVM.NewPassword);
            if (result.Succeeded)
            {
                return RedirectToAction("ResetPasswordConfirmation", new { message = "success" });
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(resetPasswordVM);
        }

        public IActionResult ResetPasswordConfirmation(string message)
        {
            if (message== "success")
                message = "You have succesfully changed the password, plesae try login with new password!.";
            else
                message = "Due to some technical glitch password has not changed, plesae try again.";

            return View(message);
        }

    }
}
