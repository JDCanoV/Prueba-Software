using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Net;
using System.Net.Mail;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.draw;

namespace GastroByte.Utilities
{
    public class EmailConfigUtility
    {
        private SmtpClient cliente;
        private MailMessage email;
        private string Host = "smtp.gmail.com";
        private int Port = 587;
        private string User = "gastrobytesoftware@gmail.com";
        private string Password = "deaolobgjvdbpsrk";
        private bool EnabledSSL = true;

        public EmailConfigUtility()
        {
            cliente = new SmtpClient(Host, Port)
            {
                EnableSsl = EnabledSSL,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(User, Password)
            };
        }

        public void EnviarCorreoConPDF(string destinatario, string asunto, string mensaje, string reservaInfo)
        {
            var pdfData = GenerarPDF(reservaInfo);
            email = new MailMessage(User, destinatario, asunto, mensaje);
            email.IsBodyHtml = true;
            email.Attachments.Add(new Attachment(new MemoryStream(pdfData), "Reserva_Gastrobyte.pdf"));
            cliente.Send(email);
        }

        private byte[] GenerarPDF(string reservaInfo)
        {
            using (var memoryStream = new MemoryStream())
            {
                Document doc = new Document(PageSize.A5, 20, 20, 30, 30); // Tamaño A5 para un estilo elegante de ticket
                PdfWriter.GetInstance(doc, memoryStream);
                doc.Open();

                // Agregar el título de la reserva
                var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 16, BaseColor.BLACK);
                var normalFont = FontFactory.GetFont(FontFactory.HELVETICA, 12, BaseColor.BLACK);

                // Encabezado con el nombre del restaurante
                Paragraph header = new Paragraph("Gastrobyte", titleFont)
                {
                    Alignment = Element.ALIGN_CENTER
                };
                doc.Add(header);

                // Subtítulo de la reserva
                Paragraph subtitle = new Paragraph("Confirmación de Reservación", normalFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingAfter = 10
                };
                doc.Add(subtitle);

                // Separador de línea
                doc.Add(new LineSeparator(1f, 100f, BaseColor.GRAY, Element.ALIGN_CENTER, -1));

                // Detalles de la reserva en formato de tabla
                PdfPTable table = new PdfPTable(2);
                table.WidthPercentage = 90;
                table.SetWidths(new float[] { 1, 2 }); // Tamaño relativo de las columnas

                // Agregar filas
                AgregarFila(table, "Fecha:", "25/10/2024", normalFont);
                AgregarFila(table, "Hora:", "18:00", normalFont);
                AgregarFila(table, "Mesa:", "5", normalFont);
                AgregarFila(table, "Cliente:", reservaInfo, normalFont);

                doc.Add(table);

                // Mensaje final
                Paragraph footer = new Paragraph("Gracias por reservar con nosotros.", normalFont)
                {
                    Alignment = Element.ALIGN_CENTER,
                    SpacingBefore = 20
                };
                doc.Add(footer);

                doc.Close();
                return memoryStream.ToArray();
            }
        }

        // Método para agregar filas a la tabla con formato
        private void AgregarFila(PdfPTable table, string campo, string valor, Font font)
        {
            PdfPCell cellCampo = new PdfPCell(new Phrase(campo, font))
            {
                Border = Rectangle.NO_BORDER,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            table.AddCell(cellCampo);

            PdfPCell cellValor = new PdfPCell(new Phrase(valor, font))
            {
                Border = Rectangle.NO_BORDER,
                PaddingBottom = 5,
                HorizontalAlignment = Element.ALIGN_LEFT
            };
            table.AddCell(cellValor);
        }
    }
}