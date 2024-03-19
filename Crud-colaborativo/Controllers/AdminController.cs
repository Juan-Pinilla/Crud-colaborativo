using Crud_colaborativo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Crud_colaborativo.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<Funcionario> _userManager;
        private IPasswordHasher<Funcionario> _passwordHasher;
        public AdminController(UserManager<Funcionario> usrMgr, IPasswordHasher<Funcionario> passwordHasher) 
        {
            _userManager = usrMgr;
            _passwordHasher = passwordHasher;
        }
        private void Errors(IdentityResult result)
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("", error.Description);
        }

        public IActionResult Index()
        {
            return View(_userManager.Users);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Funcionario user)
        {
            if(ModelState.IsValid)
            {
                Funcionario usuario = new Funcionario
                {
                    UserName = user.Nombre,
                    Nombre = user.Nombre,
                    Email = user.Email,
                    Password = user.Password
                };

                IdentityResult result = await _userManager.CreateAsync(usuario);

                if (result.Succeeded)
                {
                    RedirectToAction("Index");
                } else
                {
                    foreach (IdentityError error in result.Errors)
                        ModelState.AddModelError("", error.Description);
                }
            }
            return View(user);
        }

        public async Task<IActionResult> Delete(string id)
        {
            Funcionario user = await _userManager.FindByIdAsync(id);
            if (user != null)
            {
                IdentityResult result = await _userManager.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                    Errors(result);

            } else
                ModelState.AddModelError("", "User Not Found");

            return View("Index", _userManager.Users);
        }
    }
}
