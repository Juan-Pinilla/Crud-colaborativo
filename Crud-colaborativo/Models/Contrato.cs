using System.ComponentModel.DataAnnotations;

namespace Crud_colaborativo.Models
{
    public enum EstadoContrato
    {
        Pendiente,
        Aprobado,
        Rechazado
    }

    public class Contrato
    {
        [Key]
        [Required(ErrorMessage = "El campo IdContrato es requerido.")]
        public int Id { get; set; }

        public string? TipoCliente { get; set; }

        public string? Empresa { get; set; }

        public string? Referencia { get; set; }

        public EstadoContrato EstadoContrato { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFinalizacion { get; set; }

        public string? Socio { get; set; }

        public string? Gerente { get; set; }

        public string? Senior { get; set; }

        //Relaciones
        public virtual ICollection<Funcionario> Funcionarios { get; set; }
    }
}
