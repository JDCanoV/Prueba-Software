using GastroByte.Dtos;
using GastroByte.Services;
using GastroByte.Utilities;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;


namespace GastroByte.Controllers
{
    [AuthorizeRole(1)]
    public class AdministradorController : Controller
    {
        // Servicios necesarios para el controlador
        
        private readonly ReservaService _reservaService;
        private readonly UsuarioService _usuarioService;
        

        // Constructor donde se inicializan los servicios utilizados
        public AdministradorController()
        {
           
            _reservaService = new ReservaService();
            _usuarioService = new UsuarioService();
         
        }

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

        public ActionResult IndexUsuario()
        {
            var user = _usuarioService.GetAllUsuario();
            return View(user);
           
        }




        public ActionResult CreateUsuario()
        {
            UsuarioDto user = new UsuarioDto
            {
                Response = 0, // Inicializa Response en 0 o algún valor por defecto
                Message = string.Empty // Inicializa Message como una cadena vacía
            };
            return View(user);
        }

        [HttpPost]
       
        public ActionResult CreateUsuario(UsuarioDto newUser)
        {
            if (newUser == null)
            {
                newUser = new UsuarioDto();
                newUser.Message = "El modelo de usuario no se envió correctamente.";
                return View(newUser);
            }

            try
            {
                UsuarioService userService = new UsuarioService();
                UsuarioDto userResponse = userService.CreateUser(newUser);

                if (userResponse.Response == 1)
                {
                    return RedirectToAction("IndexUsuario", "Administrador");
                }
                else
                {
                    // Asegúrate de que `Message` tenga un valor
                    if (string.IsNullOrEmpty(userResponse.Message))
                    {
                        userResponse.Message = "Error al crear el usuario. Por favor, inténtalo nuevamente.";
                    }
                    return View(userResponse);
                }
            }
            catch (Exception ex)
            {
                // En caso de excepción, devuelves el modelo con un mensaje de error
                newUser.Message = "Ocurrió un error inesperado: " + ex.Message; // Muestra el mensaje de la excepción
                newUser.Response = 0;
                return View(newUser);
            }
        }

       

        // Acción para mostrar el formulario de reporte
        [HttpGet]
        public ActionResult ReporteReservas()
        {
            return View();
        }

        // Acción para obtener las reservas filtradas por fechas
        [HttpPost]
        public ActionResult ReporteReservas(DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio > fechaFin)
            {
                TempData["ErrorMessage"] = "La fecha de inicio no puede ser mayor que la fecha de fin.";
                return View();
            }

            var reservas = _reservaService.ObtenerReporteDeReservas(fechaInicio, fechaFin);
            return View("ListaReservas", reservas);
        }

        // Acción para generar y descargar el reporte en PDF
        [HttpPost]
        public ActionResult GenerarPDF(DateTime fechaInicio, DateTime fechaFin)
        {
            // Validar fechas
            if (fechaInicio > fechaFin)
            {
                TempData["ErrorMessage"] = "La fecha de inicio no puede ser mayor que la fecha de fin.";
                return RedirectToAction("ReporteReservas");
            }

            // Obtener las reservas en el rango de fechas
            var reservas = _reservaService.ObtenerReporteDeReservas(fechaInicio, fechaFin);

            // Verificar si hay reservas para exportar
            if (reservas == null || !reservas.Any())
            {
                TempData["ErrorMessage"] = "No hay reservas en el rango de fechas seleccionado.";
                return RedirectToAction("ReporteReservas");
            }

            // Crear el documento PDF
            Document pdfDoc = new Document(PageSize.A4, 25, 25, 30, 30);
            using (MemoryStream stream = new MemoryStream())
            {
                PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();

                // Título y fechas del reporte
                pdfDoc.Add(new Paragraph("Reporte de Reservas", new Font(Font.FontFamily.HELVETICA, 18, Font.BOLD)));
                pdfDoc.Add(new Paragraph($"Desde: {fechaInicio:dd/MM/yyyy} Hasta: {fechaFin:dd/MM/yyyy}", new Font(Font.FontFamily.HELVETICA, 12)));
                pdfDoc.Add(new Paragraph("\n"));

                // Crear una tabla con 6 columnas
                PdfPTable table = new PdfPTable(6)
                {
                    WidthPercentage = 100
                };
                table.SetWidths(new float[] { 10f, 15f, 20f, 25f, 15f, 15f });

                // Encabezados de columna
                var headerFont = new Font(Font.FontFamily.HELVETICA, 10, Font.BOLD);
                table.AddCell(new PdfPCell(new Phrase("ID Reservación", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Documento", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Nombre", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Correo Electrónico", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Fecha", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });
                table.AddCell(new PdfPCell(new Phrase("Hora", headerFont)) { HorizontalAlignment = Element.ALIGN_CENTER });

                // Añadir filas con datos de reservas con filas alternadas de diferente color
                var cellFont = new Font(Font.FontFamily.HELVETICA, 10);
                bool isEvenRow = false; // Para alternar el color de las filas

                foreach (var reserva in reservas)
                {
                    // Determinar color de fondo para la fila
                    BaseColor backgroundColor = isEvenRow ? new BaseColor(230, 230, 250) : BaseColor.WHITE; // Color lavanda claro para filas pares

                    // Crear celdas con el color de fondo correspondiente
                    table.AddCell(new PdfPCell(new Phrase(reserva.id_reservacion.ToString(), cellFont)) { BackgroundColor = backgroundColor });
                    table.AddCell(new PdfPCell(new Phrase(reserva.documento, cellFont)) { BackgroundColor = backgroundColor });
                    table.AddCell(new PdfPCell(new Phrase(reserva.nombre, cellFont)) { BackgroundColor = backgroundColor });
                    table.AddCell(new PdfPCell(new Phrase(reserva.email, cellFont)) { BackgroundColor = backgroundColor });
                    table.AddCell(new PdfPCell(new Phrase(reserva.fecha.ToString("dd/MM/yyyy"), cellFont)) { BackgroundColor = backgroundColor });
                    table.AddCell(new PdfPCell(new Phrase(reserva.hora, cellFont)) { BackgroundColor = backgroundColor });

                    // Alternar el color para la siguiente fila
                    isEvenRow = !isEvenRow;
                }

                pdfDoc.Add(table);
                pdfDoc.Close();

                // Devolver el PDF como archivo descargable
                return File(stream.ToArray(), "application/pdf", "Reporte_Reservas.pdf");
            }
        }

       
    }
}


