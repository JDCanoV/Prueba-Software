using System.Collections.Generic;
using System.Linq;
using System.Web;
using GastroByte.Dtos;

namespace GastroByte.Services
{
    public class CarritoService
    {
        private List<MenuDto> carrito;

        public CarritoService()
        {
            this.carrito = this.ObtenerCarrito();
        }

        public void AgregarAlCarrito(MenuDto producto)
        {
            // Verificar si el producto ya está en el carrito
            var productoExistente = this.carrito.FirstOrDefault(p => p.id_platillo == producto.id_platillo);

            if (productoExistente != null)
            {
                // Si el producto existe, incrementa la cantidad
                productoExistente.cantidad += 1;
            }
            else
            {
                // Si el producto no existe, agrega el producto con cantidad 1
                producto.cantidad = 1;
                this.carrito.Add(producto);
            }

            this.GuardarCarrito(); // Guardar el carrito actualizado en la sesión
        }

        public void QuitarDelCarrito(int idPlatillo)
        {
            this.carrito.RemoveAll(p => p.id_platillo == idPlatillo);
            this.GuardarCarrito();
        }

        public List<MenuDto> ObtenerProductosDelCarrito()
        {
            return this.carrito;
        }

        public void LimpiarCarrito()
        {
            this.carrito.Clear();
            this.GuardarCarrito();
        }

        private bool ProductoEnCarrito(int idPlatillo)
        {
            return this.carrito.Exists(p => p.id_platillo == idPlatillo);
        }

        private void GuardarCarrito()
        {
            HttpContext.Current.Session["carrito"] = this.carrito;
        }

        private List<MenuDto> ObtenerCarrito()
        {
            var carrito = HttpContext.Current.Session["carrito"] as List<MenuDto>;
            return carrito ?? new List<MenuDto>();
        }
    }
}
