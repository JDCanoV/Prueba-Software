using System.Collections.Generic;
using System.Web.Mvc;
using GastroByte.Dtos; // Asegúrate de tener la referencia correcta
using GastroByte.Services; // Asegúrate de tener la referencia correcta

namespace GastroByte.Controllers
{
    public class ReservasController : Controller
    {
        private readonly ReservaService _reservaService;

        public ReservasController()
        {
            _reservaService = new ReservaService(); // Asegúrate de inicializar tu servicio correctamente
        }

        // GET: Reservas
        public ActionResult Index()
        {
            IEnumerable<ReservaDto> reservas = _reservaService.GetAllReservas(); // Asegúrate de implementar este método
            return View(reservas);
        }

        // GET: Reservas/Edit/5
        public ActionResult Edit(int id)
        {
            ReservaDto reserva = _reservaService.GetReservaById(id); // Implementa este método para obtener una reserva por ID
            if (reserva == null)
            {
                return HttpNotFound();
            }
            return View(reserva);
        }

        // POST: Reservas/Edit/5
        [HttpPost]
        public ActionResult Edit(int id, ReservaDto reservaModel)
        {
            if (ModelState.IsValid)
            {
                var result = _reservaService.UpdateReserva(reservaModel); // Implementa este método para actualizar la reserva
                if (result.Response == 1)
                {
                    return RedirectToAction("Index");
                }
            }
            return View(reservaModel);
        }
    }
}
