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
            //if (ModelState.IsValid)
            //{
            //    Funcionario usuario = new Funcionario
            //    {
            //        NormalizedEmail = funcionario.Email,
            //        Nombre = funcionario.Nombre,
            //        UserName = funcionario.Nombre,
            //        Email = funcionario.Email,
            //        Password = funcionario.Password
            //    };

            //    IdentityResult result = await _userManager.CreateAsync(usuario);

            //    if (result.Succeeded)
            //    {
            //        return RedirectToAction("Index", "Home");
            //    }
            //    else
            //        foreach (IdentityError error in result.Errors)
            //            ModelState.AddModelError(string.Empty, error.Description);
            //}
            //else
            //    _logger.LogError("Error Here", "Error inside auth");

            //return View(funcionario);
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
                //var result = await _userServiceRepository.CreateUser(user);

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
                    var pass = await _userManager.CheckPasswordAsync(user, password);

                    Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(user, password, false, false);

                    if (result.Succeeded)
                        return Redirect("/");
                         
                }
                ModelState.AddModelError(nameof(email), "Password o Email Invalido");
            }
            Console.WriteLine($"Error {ModelState.IsValid}");
            return View();
        }

        //[HttpPost]
        //[Route("Signup")]
        //public IActionResult Login(string userName, string password)
        //{
        //    if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
        //    {
        //        return RedirectToAction("Login");
        //    }

        //    ClaimsIdentity identity = null;
        //    bool isAuthenticated = false;

        //    if (userName == "admin" && password == "pass")
        //    {
        //        identity = new ClaimsIdentity(new[]
        //        {
        //            new Claim(ClaimTypes.Name, userName),
        //            new Claim(ClaimTypes.Role, "Admin")
        //        }, CookieAuthenticationDefaults.AuthenticationScheme);

        //        isAuthenticated = true;

        //        //var principal = new ClaimsPrincipal(identity);

        //        //var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        //        //return RedirectToAction("Index", "Home");
        //    }

        //    if (userName == "user" && password == "pass")
        //    {
        //        identity = new ClaimsIdentity(new[]
        //        {
        //            new Claim(ClaimTypes.Name, userName),
        //            new Claim(ClaimTypes.Role, "User")
        //        }, CookieAuthenticationDefaults.AuthenticationScheme);

        //        isAuthenticated = true;
        //    }

        //    if (isAuthenticated)
        //    {
        //        var principal = new ClaimsPrincipal(identity);
        //        var login = HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);
        //        return RedirectToAction("index", "Home");
        //    }

        //    return View();
        //}

        [HttpPost]
        public IActionResult Logout()
        {
            var login = HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        }
    }
}
