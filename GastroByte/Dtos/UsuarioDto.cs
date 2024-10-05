using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GastroByte.Dtos
{
    public class UsuarioDto
    {
        public int id_usuario { get; set; }
        public string nombre { get; set; } = string.Empty;

        public string apellido { get; set; } = string.Empty;

        public string tipo_documento { get; set; } = string.Empty;

        public string numero_documento { get; set; } = string.Empty;

        public string telefono { get; set; } = string.Empty;

        public string dirreccion { get; set; } = string.Empty;

        public string correo_electronico { get; set; } = string.Empty;

        public string contrasena { get; set; } = string.Empty;

        public int id_estado { get; set; }
        public int Response { get; set; }
        public string Message { get; set; } = string.Empty;
    
    
    }




    }
