using Crud_colaborativo.Models;

namespace Crud_colaborativo.Repository
{
    public interface IUserServiceRepository
    {
        Task<bool> EmailExists(string email);

        Task<bool> CreateUser(Funcionario funcionario);
    }
}
