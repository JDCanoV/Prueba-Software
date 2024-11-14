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
    public class HomeController : Controller
    {
      
        public ActionResult Index()
        {
            ReservaDto reser = new ReservaDto
            {
                Response = 0, // Inicializa Response en 0 o algún valor por defecto
                Message = string.Empty // Inicializa Message como una cadena vacía
            };
            return View(reser);
        }
        // POST: Usuario/Create
        [HttpPost]
        public ActionResult Index(ReservaDto newRes)
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
                    reserResponse.Message = "esta melito.";
                    return RedirectToAction("Index");
                }
                else
                {
                    // Asegúrate de que `Message` tenga un valor
                    if (string.IsNullOrEmpty(reserResponse.Message))
                    {
                        reserResponse.Message = "Error al crear el usuario. Por favor, inténtalo nuevamente.";
                        
                    }
                    return View(reserResponse);
                }
            }
            catch (Exception ex)
            {
                // En caso de excepción, devuelves el modelo con un mensaje de error
                newRes.Message = "Ocurrió un error inesperado: " + ex.Message; // Muestra el mensaje de la excepción
                newRes.Response = 0;
                return View(newRes);
            }
        }

        public ActionResult AccessDenied()
        {
            return View();
        }

        [AuthorizeRole(2)]
        public ActionResult About()
        {
            DBContextUtility dbUtility = new DBContextUtility();
            bool isConnected = dbUtility.Connect();  // Intenta conectar a la base de datos

            ViewBag.ConnectionStatus = isConnected ? "Todo Melo" : "No hay conexión";  // Envía el estado de la conexión a la vista
            dbUtility.Disconnect();

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}