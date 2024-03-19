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

        [Required(ErrorMessage = "El campo Tipo Cliente es requerido.")]
        public string? TipoCliente { get; set; }

        public string? Empresa { get; set; }

        public string? Referencia { get; set; }

        [Required(ErrorMessage = "El campo Estado Contrato es requerido.")]
        public string? EstadoContrato { get; set; }

        [Required(ErrorMessage = "El campo Fecha Inicio es requerido.")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "El campo Fecha Finalizacion es requerido.")]
        public DateTime FechaFinalizacion { get; set; }

        public string? Socio { get; set; }

        public string? Gerente { get; set; }

        public string? Senior { get; set; }

        [Required(ErrorMessage = "El campo Socio Participacion es requerido.")]
        public string? SocioParticipacion { get; set; }

        [Required(ErrorMessage = "El campo Socio Comercial es requerido.")]
        public string? SocioComercial { get; set; }

        public string? PropuestaContrato { get; set; }
    }
}
