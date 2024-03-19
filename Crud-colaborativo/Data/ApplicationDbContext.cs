using Crud_colaborativo.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Crud_colaborativo.Data
{
	public class ApplicationDbContext : IdentityDbContext<Funcionario>
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Contrato> Contratos { get; set; }
		public DbSet<Funcionario> Funcionarios { get; set; }
    }
}
