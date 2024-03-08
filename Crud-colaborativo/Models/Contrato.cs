using System.ComponentModel.DataAnnotations;

namespace Crud_colaborativo.Models
{
    public class Contrato
    {
        [Key]
        [Required(ErrorMessage = "El campo IdContrato es requerido.")]
        public string Id { get; set; }

        [Required(ErrorMessage = "El campo TipoCliente es requerido.")]
        public string? TipoCliente { get; set; }

        [Required(ErrorMessage = "El campo Empresa es requerido.")]
        public string? Empresa { get; set; }

        [Required(ErrorMessage = "El campo Referencia es requerido.")]
        public string? Referencia { get; set; }

        [Required(ErrorMessage = "El campo EstadoContrato es requerido.")]
        public string? EstadoContrato { get; set; }

        [Required(ErrorMessage = "El campo FechaInicio es requerido.")]
        public DateTime FechaInicio { get; set; }

        [Required(ErrorMessage = "El campo FechaFinalizacion es requerido.")]
        public DateTime FechaFinalizacion { get; set; }

        [Required(ErrorMessage = "El campo Socio es requerido.")]
        public string? Socio { get; set; }

        [Required(ErrorMessage = "El campo Gerente es requerido.")]
        public string? Gerente { get; set; }

        [Required(ErrorMessage = "El campo Senior es requerido.")]
        public string? Senior { get; set; }

        [Required(ErrorMessage = "El campo SocioParticipacion es requerido.")]
        public string? SocioParticipacion { get; set; }

        [Required(ErrorMessage = "El campo SocioComercial es requerido.")]
        public string? SocioComercial { get; set; }

        [Required(ErrorMessage = "El campo PropuestaContrato es requerido.")]
        public string? PropuestaContrato { get; set; }
    }
}
