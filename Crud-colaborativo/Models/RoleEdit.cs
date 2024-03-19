using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Crud_colaborativo.Models
{
    public class RoleEdit
    {
            public IdentityRole Role { get; set; }
            public IEnumerable<Funcionario> Members { get; set; }
            public IEnumerable<Funcionario> NonMembers { get; set; }
    }
}
