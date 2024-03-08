using Crud_colaborativo.Models;
using Microsoft.EntityFrameworkCore;

namespace Crud_colaborativo.Data
{
	public class ApplicationDbContext : DbContext
	{
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Contrato> Contratos { get; set; }
    }
}
