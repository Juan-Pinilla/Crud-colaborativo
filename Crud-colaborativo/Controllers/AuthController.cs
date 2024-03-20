using Crud_colaborativo.Models;
using Crud_colaborativo.Repository;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Crud_colaborativo.Controllers
{

    public class AuthController : Controller
    {

        private readonly IUserServiceRepository _userServiceRepository;

        private SignInManager<Funcionario> _signInManager;
        private UserManager<Funcionario> _userManager;

        private readonly ILogger<AuthController> _logger;

        public AuthController(IUserServiceRepository userServiceRepository, SignInManager<Funcionario> signInManager, UserManager<Funcionario> userManager, ILogger<AuthController> logger)
        {
            _userServiceRepository = userServiceRepository;
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }

        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Signup() => View();

        [HttpPost]
        public async Task<IActionResult> Signup(Funcionario funcionario)
        {
            if (ModelState.IsValid)
            {
                var emailExists = await _userServiceRepository.EmailExists(funcionario.Email);
                Console.WriteLine($"Error inside credint a new user {emailExists}");
                if (emailExists)
                {
                    ModelState.AddModelError("Error Creating the User", "Email Already Used");
                }
                Funcionario user = new Funcionario
                {
                    Nombre = funcionario.Nombre,
                    UserName = funcionario.Email,
                    Email = funcionario.Email,
                    Password = funcionario.Password,
                };
                var result = await _userManager.CreateAsync(user, user.Password);

                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else
                    ModelState.AddModelError("Error on the second Conditional", "Error Creating the User");
            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                Funcionario user = await _userManager.FindByEmailAsync(email);
                if (user != null)
                {
                    await _signInManager.SignOutAsync();

                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, password, false, false);

                    if (result.Succeeded)
                        return Redirect("/");
                         
                }
                ModelState.AddModelError(nameof(email), "Password o Email Invalido");
            }
            return View();
        }

        //    if (userName == "user" && password == "pass")
        //    {
        //        identity = new ClaimsIdentity(new[]
        //        {
        //            new Claim(ClaimTypes.Name, userName),
        //            new Claim(ClaimTypes.Role, "User")
        //        }, CookieAuthenticationDefaults.AuthenticationScheme);


        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
