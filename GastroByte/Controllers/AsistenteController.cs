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
    [AuthorizeRole(2)]
    public class AsistenteController : Controller
    {
        private readonly MenuService _menuService;
        private readonly ReservaService _reservaService;

        public AsistenteController()
        {
            _menuService = new MenuService();
            _reservaService = new ReservaService();
           
           
        }
        // GET: Asistente
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

            return View();
            
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
                    return RedirectToAction("IndexMenu", "Asistente");
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
            int id = (int)Session["UserID"];
            var updatedMenu = _menuService.UpdateMenu(menu,id);

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
                // Depuración: Verifica los errores en el ModelState
                var errors = ModelState.Values.SelectMany(v => v.Errors);
                foreach (var error in errors)
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                return View(reserva);
            }
            int id= (int)Session["UserID"];
            var updatedReserva = _reservaService.UpdateReserva(reserva,id);
            if (updatedReserva != null)
            {
                TempData["SuccessMessage"] = "Reserva actualizada correctamente.";
                return RedirectToAction("IndexReservas");
            }

            ModelState.AddModelError("", "Error al actualizar la reserva.");
            return View(reserva);
        }



    }
}