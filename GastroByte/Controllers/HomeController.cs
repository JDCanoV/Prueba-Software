using GastroByte.Dtos;
using GastroByte.Services;
using System;
using System.Web.Mvc;

namespace GastroByte.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home/Index
        public ActionResult Index() // Método que maneja las solicitudes GET a la acción Index.
        {
            return View(); // Retorna la vista asociada a la acción Index.
        }

        // POST: Home/Create
        [HttpPost]
        public ActionResult Create(ReservaDto newReserva) // Método que maneja la creación de reservas.
        {
            // Verifica si el modelo de reserva recibido es nulo.
            if (newReserva == null)
            {
                newReserva = new ReservaDto
                {
                    Message = "El modelo de reserva no se envió correctamente." // Mensaje de error
                };
                return View("Index", newReserva); // Retorna la vista Index con el modelo que contiene el mensaje de error.
            }

            try
            {
                ReservaService reservaService = new ReservaService(); // Inicializa el servicio de reservas
                ReservaDto reservaResponse = reservaService.CreateReservation(newReserva); // Intenta crear la reserva

                // Verifica la respuesta de la creación de la reserva
                if (reservaResponse.Response == 1) // Si la reserva fue creada exitosamente
                {
                    return RedirectToAction("Index"); // Redirige a la acción Index
                }
                else
                {
                    if (string.IsNullOrEmpty(reservaResponse.Message))
                    {
                        reservaResponse.Message = "Error al crear la reserva. Por favor, inténtalo nuevamente."; // Mensaje de error por defecto
                    }
                    return View("Index", reservaResponse); // Retorna la vista Index con el modelo que contiene el mensaje de error.
                }
            }
            catch (Exception ex)
            {
                newReserva.Message = "Ocurrió un error inesperado: " + ex.Message; // Mensaje de error
                newReserva.Response = 0; // Establece la respuesta a 0 para indicar error
                return View("Index", newReserva); // Retorna la vista Index con el modelo que contiene el mensaje de error.
            }
        }
    }
}
