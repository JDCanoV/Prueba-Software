using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GastroByte.Dtos
{
    public class ReservaDto
    {
        public int id_reservacion { get; set; }
        public int id_usuario { get; set; }

         
        public string numero_personas { get; set; } = string.Empty;

        public string nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La fecha es obligatoria.")]

        public DateTime fecha { get; set; }

        [Required(ErrorMessage = "La hora es obligatoria.")]
        public string hora { get; set; } = string.Empty;
        public int id_estado { get; set; }
        public string mesa { get; set; } = string.Empty;
        public int Response { get; set; }
        public string Message { get; set; } = string.Empty;
        [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
        [EmailAddress(ErrorMessage = "Debe ser un correo electrónico válido.")]
        public string email { get; set; } = string.Empty;
        

        [Required(ErrorMessage = "El número de documento es obligatorio.")]

        public string documento { get; set; } = string.Empty;

        
    }
}
