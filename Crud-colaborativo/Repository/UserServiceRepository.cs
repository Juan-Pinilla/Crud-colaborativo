using Crud_colaborativo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Crud_colaborativo.Repository
{
    public class UserServiceRepository: IUserServiceRepository
    {
        private readonly UserManager<Funcionario> _userManager;


        private readonly ILogger<UserServiceRepository> _logger;

        public UserServiceRepository(UserManager<Funcionario> userManager, ILogger<UserServiceRepository> logger) 
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> EmailExists(string email)
        {
            try
            {
                var user = await _userManager.Users.FirstOrDefaultAsync(x => x.Email == email);

                if (user is null)
                {
                    return false;
                }
                else
                    return true;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public async Task<bool> CreateUser(Funcionario funcionario)
        {
            try
            {
                var user = await _userManager.CreateAsync(funcionario, funcionario.Password);
                if (user.Succeeded)
                {
                    return user.Succeeded;
                } else
                {
                    foreach (var error in user.Errors)
                    {
                        _logger.LogError($"Error creating the user {error.Description}");
                    }
                    return false;
                }
                    
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error creating the user {ex.Message}");
                return false;
            }
        }
    }
}
