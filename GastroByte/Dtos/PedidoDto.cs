using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GastroByte.Dtos
{
    public class PedidoDto
    {
        public int id_pedido { get; set; }
        public int id_usuario { get; set; }
        public string tipo_pedido { get; set; } = string.Empty;
        public DateTime Fecha { get; set; }
        public DateTime Hora { get; set; }
        public decimal precio_total { get; set; }
        public string nombre { get; set; } = string.Empty;
        public string cedula { get; set; } = string.Empty;
        public string telefono { get; set; } = string.Empty;
        public string direccion { get; set; } = string.Empty;
        public string correo { get; set; } = string.Empty;
        public int id_estado { get; set; }
        public string Message { get; set; } = string.Empty;
        public int Response { get; set; }
    }
}
