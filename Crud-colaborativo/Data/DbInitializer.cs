using Crud_colaborativo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;

namespace Crud_colaborativo.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            
            var con1 = new Contrato { Id = "RV001", Empresa = "Contoso", EstadoContrato = EstadoContrato.Pendiente.ToString(), FechaFinalizacion = DateTime.Now, FechaInicio = DateTime.Now, Socio = "Someone", Gerente = "Roberto", Senior = "Marcos", SocioComercial = "Andres", SocioParticipacion = "Roberto", TipoCliente = "Grupal" };
            //var user1 = new Funcionario { NormalizedEmail = "T@M.COM", UserName = "t@m.com", Nombre = "andres", Password = "ewq321" };

            if (!context.Contratos.Any())
            {
                var contratos = new Contrato[]
                {
                    con1
                };
                context.Contratos.AddRange(contratos);
            }

            //if (!context.Funcionarios.Any())
            //{
            //    var funcionarios = new Funcionario[]
            //    {
            //        user1
            //    };
            //    context.Funcionarios.AddRange(funcionarios);
            //}

            context.SaveChanges();

        }

        public static class SeedData
        {
            static public async Task InitializeAsync(IServiceProvider service)
            {
                var roleManager = service.GetRequiredService<RoleManager<IdentityRole>>();
                await EnsureRolesAsync(roleManager);

                var userManager = service.GetRequiredService<UserManager<Funcionario>>();
                await EnsureTestAdmin(userManager);

            }

            private static async Task EnsureRolesAsync(RoleManager<IdentityRole> roleManager)
            {
                var alreayExists = await roleManager.RoleExistsAsync(Constants.Admin);
                if (alreayExists) return;
                await roleManager.CreateAsync(new IdentityRole(Constants.Admin));
            
            }

            private static async Task EnsureTestAdmin(UserManager<Funcionario> userManager)
            {
                var testAdmin = await userManager.Users.Where(x => x.UserName == "t@m.com").FirstOrDefaultAsync();
                if (testAdmin != null) return;

                Funcionario testFunc = new Funcionario
                {
                    Nombre = "Andres",
                    UserName = "t@m.com",
                    Email = "t@m.com",
                    Password = "ewq321"
                };

                await userManager.CreateAsync(testFunc, "ewq321");
                await userManager.AddToRoleAsync(testFunc, Constants.Admin);
            }
        }

        public static class Constants
        {
            public const string Admin = "Admin";
        }

    }
}
