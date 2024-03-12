using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Crud_colaborativo.Models;

namespace Crud_colaborativo.Data
{
    public class Crud_colaborativoContext : DbContext
    {
        public Crud_colaborativoContext (DbContextOptions<Crud_colaborativoContext> options)
            : base(options)
        {
        }

        public DbSet<Crud_colaborativo.Models.Contrato> Contrato { get; set; } = default!;
    }
}
