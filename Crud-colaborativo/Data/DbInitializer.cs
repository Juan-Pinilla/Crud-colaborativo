using Crud_colaborativo.Models;

namespace Crud_colaborativo.Data
{
    public class DbInitializer
    {
        public static void Initialize(ApplicationDbContext context)
        {
            
            var con1 = new Contrato { Empresa = "Contoso", EstadoContrato = EstadoContrato.Pendiente, FechaFinalizacion = DateTime.Now, FechaInicio = DateTime.Now, Socio = "Someone", Gerente = "Roberto", Senior = "Marcos" };
            var user1 = new Funcionario { Email = "em@m.com", Nombre = "name1" };

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
    }
}
