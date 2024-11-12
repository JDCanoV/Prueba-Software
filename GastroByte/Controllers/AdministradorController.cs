using GastroByte.Dtos;
using GastroByte.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

        // Acción para mostrar la vista principal con la lista de menús
        public ActionResult IndexMenu()
        {
            var menu = _menuService.GetAllMenus(); // Obtiene todos los menús desde el servicio
            return View(menu); // Devuelve la vista con la lista de menús
        }

        // Acción GET para mostrar el formulario de creación de un nuevo platillo
        public ActionResult CreateMenu()
        {
            return View(); // Devuelve la vista de creación de menú
        }

        // Acción POST para crear un nuevo platillo
        [HttpPost]
        [ValidateAntiForgeryToken] // Protege contra ataques CSRF
        public ActionResult CreateMenu(MenuDto menu)
        {
            if (ModelState.IsValid) // Verifica si el modelo es válido
            {
                var createdMenu = _menuService.CreateMenu(menu); // Llama al servicio para crear el menú
                if (createdMenu.Response == 1) // Si la creación es exitosa
                {
                    TempData["SuccessMessage"] = "Platillo creado correctamente."; // Mensaje de éxito
                    return RedirectToAction("IndexMenu", "Administrador"); // Redirige al listado de menús
                }
                ModelState.AddModelError("", createdMenu.Message); // Muestra el mensaje de error si algo salió mal
            }
            return View(menu); // Devuelve la vista con el menú ingresado (en caso de error)
        }

        // Acción para cargar los datos de un platillo específico para editar
        public ActionResult EditMenu(int id)
        {
            var menu = _menuService.GetMenuById(id); // Obtiene el menú por ID
            if (menu == null)
            {
                return HttpNotFound(); // Si no existe el menú, muestra un error 404
            }
            return View(menu); // Devuelve la vista de edición con los datos del menú
        }

        // Acción POST para actualizar un menú
        [HttpPost]
        [ValidateAntiForgeryToken] // Protección contra CSRF
        public ActionResult EditMenu(MenuDto menu)
        {
            if (!ModelState.IsValid) // Si el modelo no es válido, vuelve a mostrar la vista
            {
                return View(menu);
            }

            var updatedMenu = _menuService.UpdateMenu(menu); // Llama al servicio para actualizar el menú

            if (updatedMenu != null)
            {
                TempData["SuccessMessage"] = "Menu actualizado correctamente."; // Mensaje de éxito
                return RedirectToAction("IndexMenu"); // Redirige al listado de menús
            }

            ModelState.AddModelError("", "Error al actualizar el menu."); // Muestra el mensaje de error
            return View(menu); // Devuelve la vista con el menú ingresado (en caso de error)
        }

        // Acción para eliminar un platillo
        [HttpPost]
        [ValidateAntiForgeryToken] // Protección contra CSRF
        public ActionResult DeleteMenu(int id)
        {
            var isDeleted = _menuService.DeleteMenu(id); // Llama al servicio para eliminar el platillo
            if (isDeleted)
            {
                TempData["SuccessMessage"] = "Platillo eliminado correctamente."; // Mensaje de éxito
            }
            else
            {
                TempData["ErrorMessage"] = "Error al eliminar el platillo."; // Mensaje de error
            }
            return RedirectToAction("IndexMenu"); // Redirige al listado de menús
        }

        // ================================
        // Funcionalidades de Reservas
        // ================================

        // Acción para mostrar la vista principal con la lista de reservas
        public ActionResult IndexReservas()
        {
            var reservas = _reservaService.GetAllReservas(); // Obtiene todas las reservas desde el servicio
            return View(reservas); // Devuelve la vista con la lista de reservas
        }

        // Acción para cargar los datos de una reserva específica para editar
        public ActionResult EditReserva(int id)
        {
            var reserva = _reservaService.GetReservaById(id); // Obtiene la reserva por ID
            if (reserva == null)
            {
                return HttpNotFound(); // Si no existe la reserva, muestra un error 404
            }
            return View(reserva); // Devuelve la vista de edición con los datos de la reserva
        }

        // Acción para actualizar una reserva
        [HttpPost]
        public ActionResult EditReserva(ReservaDto reserva)
        {
            if (!ModelState.IsValid) // Si el modelo no es válido, vuelve a mostrar la vista
            {
                return View(reserva);
            }

            var updatedReserva = _reservaService.UpdateReserva(reserva); // Llama al servicio para actualizar la reserva

            if (updatedReserva != null)
            {
                TempData["SuccessMessage"] = "Reserva actualizada correctamente."; // Mensaje de éxito
                return RedirectToAction("IndexReservas"); // Redirige al listado de reservas
            }

            ModelState.AddModelError("", "Error al actualizar la reserva."); // Muestra el mensaje de error
            return View(reserva); // Devuelve la vista con los datos de la reserva (en caso de error)
        }

        // Acción para ver los productos en el carrito
        public ActionResult IndexCarrito()
        {
            var productosEnCarrito = _carritoService.ObtenerProductosDelCarrito(); // Obtiene los productos del carrito
            return View(productosEnCarrito); // Devuelve la vista con los productos en el carrito
        }

        // Acción para agregar un producto al carrito mediante AJAX
        [HttpPost]
        public JsonResult AgregarAlCarrito(MenuDto producto)
        {
            _carritoService.AgregarAlCarrito(producto); // Llama al servicio para agregar el producto al carrito
            return Json(new { success = true, message = "Producto agregado al carrito" }); // Respuesta de éxito en formato JSON
        }

        // Acción para quitar un producto del carrito mediante AJAX
        [HttpPost]
        public JsonResult QuitarDelCarrito(int id_platillo)
        {
            _carritoService.QuitarDelCarrito(id_platillo); // Llama al servicio para quitar el producto del carrito
            return Json(new { success = true, message = "Producto quitado del carrito" }); // Respuesta de éxito en formato JSON
        }

        // Acción para limpiar el carrito mediante AJAX
        [HttpPost]
        public JsonResult LimpiarCarrito()
        {
            _carritoService.LimpiarCarrito(); // Llama al servicio para limpiar el carrito
            return Json(new { success = true, message = "Carrito limpiado" }); // Respuesta de éxito en formato JSON
        }

        // Acción para confirmar el pago
        // Acción para confirmar el pago
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
            var total = carrito.Sum(item => item.precio * item.cantidad);

            // Asegúrate de que el total es el esperado
            Console.WriteLine("Total calculado en ConfirmarPago: " + total);

            var pedido = new PedidoDto
            {
                nombre = Session["UserName"] as string,
                cedula = Session["UserDocumento"] as string,
                telefono = Session["UserTelefono"] as string,
                correo = Session["UserCorreo"] as string,
                precio_total = total.ToString()  // Convierte a string antes de asignar
            };
            return View(pedido);
        }

        [HttpPost]
        public ActionResult ProcesarPago(PedidoDto model)
        {
            // Imprimir el valor de precio_total recibido
            Console.WriteLine("Precio Total Recibido en ProcesarPago: " + model.precio_total);

            if (model.tipo_pedido == "domicilio" && string.IsNullOrEmpty(model.direccion))
            {
                ModelState.AddModelError("direccion", "La dirección es obligatoria para pedidos a domicilio.");
            }

            if (!ModelState.IsValid)
            {
                return View("ConfirmarPago", model);
            }

            model.precio_total = CalcularPrecioTotal().ToString();

            try
            {
                var resultado = _pedidoService.CreatePedido(model);

                if (resultado != null)
                {
                    _carritoService.LimpiarCarrito();
                    TempData["SuccessMessage"] = "El pago se procesó correctamente y el carrito fue limpiado.";
                    return RedirectToAction("IndexCarrito", "Administrador");
                }
                else
                {
                    ModelState.AddModelError("", "Hubo un error al guardar el pedido en la base de datos.");
                    return View("ConfirmarPago", model);
                }
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Hubo un problema al procesar el pago: " + ex.Message);
                return View("ConfirmarPago", model);
            }
        }

        private decimal CalcularPrecioTotal()
        {
            decimal total = 0;
            if (ViewBag.Carrito != null)
            {
                foreach (var item in ViewBag.Carrito as IEnumerable<MenuDto>)
                {
                    total += item.precio * item.cantidad;
                }
            }
            Console.WriteLine("Total calculado en CalcularPrecioTotal: " + total);
            return total;
        }
    }
}