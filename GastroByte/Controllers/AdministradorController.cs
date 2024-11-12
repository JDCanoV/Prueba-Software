using GastroByte.Dtos;
using GastroByte.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace GastroByte.Controllers
{
    public class AdministradorController : Controller
    {
        // Servicios necesarios para el controlador
        private readonly MenuService _menuService;
        private readonly ReservaService _reservaService;
        private readonly CarritoService _carritoService;
        private readonly PedidoService _pedidoService;

        // Constructor donde se inicializan los servicios utilizados
        public AdministradorController()
        {
            _menuService = new MenuService();
            _reservaService = new ReservaService();
            _carritoService = new CarritoService();
            _pedidoService = new PedidoService();
        }

        // ================================
        // Funcionalidades de Menú
        // ================================

        public ActionResult IndexMenu()
        {
            var menu = _menuService.GetAllMenus();
            return View(menu);
        }

        public ActionResult CreateMenu()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreateMenu(MenuDto menu)
        {
            if (ModelState.IsValid)
            {
                var createdMenu = _menuService.CreateMenu(menu);
                if (createdMenu.Response == 1)
                {
                    TempData["SuccessMessage"] = "Platillo creado correctamente.";
                    return RedirectToAction("IndexMenu", "Administrador");
                }
                ModelState.AddModelError("", createdMenu.Message);
            }
            return View(menu);
        }

        public ActionResult EditMenu(int id)
        {
            var menu = _menuService.GetMenuById(id);
            if (menu == null)
            {
                return HttpNotFound();
            }
            return View(menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditMenu(MenuDto menu)
        {
            if (!ModelState.IsValid)
            {
                return View(menu);
            }

            var updatedMenu = _menuService.UpdateMenu(menu);
            if (updatedMenu != null)
            {
                TempData["SuccessMessage"] = "Menu actualizado correctamente.";
                return RedirectToAction("IndexMenu");
            }

            ModelState.AddModelError("", "Error al actualizar el menu.");
            return View(menu);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteMenu(int id)
        {
            var isDeleted = _menuService.DeleteMenu(id);
            if (isDeleted)
            {
                TempData["SuccessMessage"] = "Platillo eliminado correctamente.";
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el platillo.";
            }
            return RedirectToAction("IndexMenu");
        }

        // ================================
        // Funcionalidades de Reservas
        // ================================

        public ActionResult IndexReservas()
        {
            var reservas = _reservaService.GetAllReservas();
            return View(reservas);
        }

        public ActionResult EditReserva(int id)
        {
            var reserva = _reservaService.GetReservaById(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            return View(reserva);
        }

        [HttpPost]
        public ActionResult EditReserva(ReservaDto reserva)
        {
            if (!ModelState.IsValid)
            {
                return View(reserva);
            }

            var updatedReserva = _reservaService.UpdateReserva(reserva);
            if (updatedReserva != null)
            {
                TempData["SuccessMessage"] = "Reserva actualizada correctamente.";
                return RedirectToAction("IndexReservas");
            }

            ModelState.AddModelError("", "Error al actualizar la reserva.");
            return View(reserva);
        }

        public ActionResult IndexCarrito()
        {
            var productosEnCarrito = _carritoService.ObtenerProductosDelCarrito();
            return View(productosEnCarrito);
        }

        [HttpPost]
        public JsonResult AgregarAlCarrito(MenuDto producto)
        {
            _carritoService.AgregarAlCarrito(producto);
            return Json(new { success = true, message = "Producto agregado al carrito" });
        }

        [HttpPost]
        public JsonResult QuitarDelCarrito(int id_platillo)
        {
            _carritoService.QuitarDelCarrito(id_platillo);
            return Json(new { success = true, message = "Producto quitado del carrito" });
        }

        [HttpPost]
        public JsonResult LimpiarCarrito()
        {
            _carritoService.LimpiarCarrito();
            return Json(new { success = true, message = "Carrito limpiado" });
        }

        // Método para calcular el precio total del carrito
        private decimal CalcularPrecioTotal()
        {
            decimal total = 0;
            var carrito = _carritoService.ObtenerProductosDelCarrito();

            if (carrito != null)
            {
                total = carrito.Sum(p => p.precio * p.cantidad);
            }

            return total;
        }

        public ActionResult ConfirmarPago()
        {
            if (Session["UserID"] == null)
            {
                return RedirectToAction("Login", "Usuario", new { returnUrl = Url.Action("ConfirmarPago", "Administrador") });
            }

            var carrito = _carritoService.ObtenerProductosDelCarrito();
            if (carrito == null || !carrito.Any())
            {
                TempData["ErrorMessage"] = "El carrito está vacío.";
                return RedirectToAction("IndexCarrito");
            }

            ViewBag.Carrito = carrito;

            decimal total = CalcularPrecioTotal();

            var pedido = new PedidoDto
            {
                nombre = Session["UserName"] as string,
                cedula = Session["UserDocumento"] as string,
                telefono = Session["UserTelefono"] as string,
                correo = Session["UserCorreo"] as string,
                precio_total = total
            };

            return View(pedido);
        }

        [HttpPost]
        public ActionResult ProcesarPago(PedidoDto model)
        {
            if (model.tipo_pedido == "domicilio" && string.IsNullOrEmpty(model.direccion))
            {
                ModelState.AddModelError("direccion", "La dirección es obligatoria para pedidos a domicilio.");
            }

            if (!ModelState.IsValid)
            {
                return View("ConfirmarPago", model);
            }

            decimal totalCalculado = CalcularPrecioTotal();
            model.precio_total = totalCalculado;

            try
            {
                var resultado = _pedidoService.CreatePedido(model);
                if (resultado != null && resultado.Response == 1)
                {
                    _carritoService.LimpiarCarrito();
                    TempData["SuccessMessage"] = "El pago se procesó correctamente y el carrito fue limpiado.";
                    return RedirectToAction("IndexCarrito", "Administrador");
                }
                else
                {
                    ModelState.AddModelError("", resultado?.Message ?? "Hubo un error al guardar el pedido en la base de datos.");
                    return View("ConfirmarPago", model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Hubo un problema al procesar el pago: " + ex.Message);
                return View("ConfirmarPago", model);
            }
        }
    }
}
