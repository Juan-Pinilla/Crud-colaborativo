﻿using Microsoft.AspNetCore.Identity;

namespace Crud_colaborativo.Models
{
    public class Funcionario: IdentityUser
    {
        public string Nombre { get; set; }
        //public string Email { get; set; }
        public string Password { get; set; }
        
        public string? ContratoId { get; set; }
        public Contrato? Contrato { get; set; }
    }
}
