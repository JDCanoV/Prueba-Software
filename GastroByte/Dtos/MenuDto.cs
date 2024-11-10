using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GastroByte.Dtos
{
    public class MenuDto
    {
        public int id_platillo { get; set; }
        public int id_categoria { get; set; }
        public string nombre_platillo { get; set; } = string.Empty;
        public string descripcion { get; set; } = string.Empty;
        public int precio { get; set; }
        public int cantidad { get; set; }
        public int Response { get; set; }
        public string Message { get; set; } = string.Empty;
        public int id_estado { get; set; }
    }
}
