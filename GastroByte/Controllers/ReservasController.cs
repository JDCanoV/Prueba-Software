using GastroByte.Dtos;
using GastroByte.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

//nuevo
namespace GastroByte.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ReservaService _reservaService;

        public ReservasController()
        {
            _reservaService = new ReservaService();
        }

        // Acción para mostrar la vista principal con la lista de reservas
        public ActionResult Index()
        {
            var reservas = _reservaService.GetAllReservas();
            return View(reservas);
        }

        // Acción para cargar los datos de una reserva específica
        public ActionResult Edit(int id)
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
        public ActionResult Edit(ReservaDto reserva)
        {
            if (!ModelState.IsValid)
            {
                // Si el modelo no es válido, regresa a la vista de edición
                return View(reserva);
            }

            var updatedReserva = _reservaService.UpdateReserva(reserva);

            if (updatedReserva != null)
            {
                TempData["SuccessMessage"] = "Reserva actualizada correctamente.";
                return RedirectToAction("Index"); // Redirige a la lista de reservas
            }

            ModelState.AddModelError("", "Error al actualizar la reserva.");
            return View(reserva); // Regresa a la vista de edición en caso de error
        }
    }
}
