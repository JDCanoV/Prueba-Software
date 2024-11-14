using GastroByte.Dtos;
using GastroByte.Services;
using GastroByte.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GastroByte.Controllers
{
    [AuthorizeRole(3)]
    public class ClienteController : Controller
    {    // Servicios necesarios para el controlador
        private readonly MenuService _menuService;
   
    private readonly CarritoService _carritoService;
    private readonly PedidoService _pedidoService;

    // Constructor donde se inicializan los servicios utilizados
    public ClienteController()
    {
        _menuService = new MenuService();
        
        _carritoService = new CarritoService();
        _pedidoService = new PedidoService();
    }

        // GET: Cliente
        public ActionResult Index()
        {
            // Verifica si la sesión está activa y obtiene los datos
            if (Session["UserName"] != null && Session["UserRole"] != null)
            {
                ViewBag.UserName = Session["UserName"].ToString();
                ViewBag.UserRole = (int)Session["UserRole"];
            }
            else
            {
                return RedirectToAction("Login", "Usuario");
            }

            // Preserva TempData para que esté disponible en la vista
            TempData.Keep();
            return View();
        }

        public ActionResult IndexMenu()
        {
            var menu = _menuService.GetAllMenus();
            return View(menu);
        }

        public ActionResult CreateMenu()
        {
            return View();
        }



        // Acción POST para crear un nuevo platillo
       
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
                return RedirectToAction("Login", "Usuario", new { returnUrl = Url.Action("ConfirmarPago", "Cliente") });
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
                    return RedirectToAction("IndexCarrito", "Cliente");
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

        // GET: Cliente/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Cliente/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Cliente/Create
        [HttpPost]
        public ActionResult Create(FormCollection collection)
        {
            try
            {
                

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult CreateReserva()
        {
            // Crea y llena una única instancia de ReservaDto con los datos de la sesión
            var reserva = new ReservaDto
            {
                nombre = Session["UserName"] as string,
                documento = Session["UserDocumento"] as string,
                email = Session["UserCorreo"] as string,
                Response = 0,
                Message = string.Empty
            };

            return View(reserva);
        }


        // POST: Usuario/Create
        [HttpPost]
        public ActionResult CreateReserva(ReservaDto newRes)
        {
            if (newRes == null)
            {
                newRes = new ReservaDto();
                newRes.Message = "El modelo de usuario no se envió correctamente.";
                return View(newRes);
            }

            try
            {
                ReservaService reserService = new ReservaService();
                ReservaDto reserResponse = reserService.CreateReser(newRes);

                if (reserResponse.Response == 1)
                {
                    TempData["SuccessMessage"] = "Su reserva ha sido creada exitosamente.";
                    return RedirectToAction("Index");
                }
                else
                {
                    if (string.IsNullOrEmpty(reserResponse.Message))
                    {
                        reserResponse.Message = "Error al crear la reserva. Por favor, inténtelo nuevamente.";
                    }
                    return View(reserResponse);
                }
            }
            catch (Exception ex)
            {
                newRes.Message = "Ocurrió un error inesperado: " + ex.Message;
                newRes.Response = 0;
                return View(newRes);
            }
        }


        // GET: Cliente/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Cliente/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: Cliente/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Cliente/Delete/5
        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}
