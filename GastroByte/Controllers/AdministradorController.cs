using GastroByte.Dtos;
using GastroByte.Services;
using System.Linq;
using System.Web.Mvc;

namespace GastroByte.Controllers
{
    public class AdministradorController : Controller
    {
        private readonly MenuService _menuService;
        private readonly ReservaService _reservaService;
        private readonly CarritoService _carritoService;

        public AdministradorController()
        {
            _menuService = new MenuService();
            _reservaService = new ReservaService();
            _carritoService = new CarritoService();
        }

        // ================================
        // Funcionalidades de Menú
        // ================================

        // Acción para mostrar la vista principal con la lista de menús
        public ActionResult IndexMenu()
        {
            var menu = _menuService.GetAllMenus();
            return View(menu);
        }

        // Acción GET para mostrar el formulario de creación de menú
        public ActionResult CreateMenu()
        {
            return View();
        }

        // Acción POST para crear un nuevo platillo
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

        // Acción para cargar los datos de un platillo específico para editar
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

        // Acción para mostrar la vista principal con la lista de reservas
        public ActionResult IndexReservas()
        {
            var reservas = _reservaService.GetAllReservas();
            return View(reservas);
        }

        // Acción para cargar los datos de una reserva específica para editar
        public ActionResult EditReserva(int id)
        {
            var reserva = _reservaService.GetReservaById(id);
            if (reserva == null)
            {
                return HttpNotFound();
            }
            return View(reserva);
        }

        // Acción para actualizar la reserva después de ser editada
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
        // Acción para ver el carrito
        public ActionResult IndexCarrito()
        {
            var productosEnCarrito = _carritoService.ObtenerProductosDelCarrito();
            return View(productosEnCarrito);
        }

        // Acción para agregar un producto al carrito mediante AJAX
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

        // Acción para limpiar el carrito mediante AJAX
        [HttpPost]
        public JsonResult LimpiarCarrito()
        {
            _carritoService.LimpiarCarrito();
            return Json(new { success = true, message = "Carrito limpiado" });
        }
        public ActionResult ConfirmarPago()
        {
            // Verificar si el usuario está autenticado
            if (Session["UserID"] == null)
            {
                // Redirigir al login si no está logueado, con una URL de retorno
                return RedirectToAction("Login", "Usuario", new { returnUrl = Url.Action("ConfirmarPago", "Administrador") });
            }

            // Obtener los productos del carrito
            var carrito = _carritoService.ObtenerProductosDelCarrito();
            var total = carrito.Sum(item => item.precio * item.cantidad);
            ViewBag.Total = total;

            return View(carrito);
        }


        [HttpPost]
        public JsonResult ProcesarPago()
        {
            // Aquí agregarías la lógica para procesar el pago real.
            // Simularemos el pago y luego limpiamos el carrito.
            _carritoService.LimpiarCarrito();
            return Json(new { success = true });
        }

    }
}