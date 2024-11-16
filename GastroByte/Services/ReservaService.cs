using GastroByte.Dtos;
using GastroByte.Repositories;
using GastroByte.Utilities;
using System;
using System.Collections.Generic;

namespace GastroByte.Services
{
    public class ReservaService
    {
        private readonly ReservaRepository reserReposiyoty;

        // Constructor
        public ReservaService()
        {
            reserReposiyoty = new ReservaRepository(); // Asignación correcta a la variable de campo
        }

        // Método para crear una nueva reserva
        public ReservaDto CreateReser(ReservaDto reserModel)
        {
            ReservaDto responseReservaDto = new ReservaDto();
            try
            {
                if (reserReposiyoty.CreateReser(reserModel) != 0)
                {
                    responseReservaDto.Response = 1;
                    responseReservaDto.Message = "Creación exitosa";
                    EmailConfigUtility emailUtility = new EmailConfigUtility();
                    string destinatario = reserModel.email;
                    string asunto = "Confirmación de Reserva";
                    string mensaje = "Estimado cliente, adjuntamos su confirmación de reserva.";
                    emailUtility.EnviarCorreoConPDF(destinatario, asunto, mensaje, reserModel);

                }
                else
                {
                    responseReservaDto.Response = 0;
                    responseReservaDto.Message = "Algo pasó";
                }

                return responseReservaDto;
            }
            catch (Exception e)
            {
                responseReservaDto.Response = 0;
                responseReservaDto.Message = e.InnerException?.ToString() ?? e.Message; // Manejo de error
                return responseReservaDto;
            }
        }

        // Método para obtener todas las reservas
        public IEnumerable<ReservaDto> GetAllReservas()
        {
            return reserReposiyoty.GetAllReservas(); // Esto ahora debería funcionar correctamente
        }

        public IEnumerable<ReservaDto> ObtenerReporteDeReservas(DateTime fechaInicio, DateTime fechaFin)
        {
            return reserReposiyoty.GetReservasPorFechas(fechaInicio, fechaFin);
        }


        // Método para obtener una reserva por ID
        public ReservaDto GetReservaById(int id)
        {
            return reserReposiyoty.GetReservaById(id); // Este método también debería funcionar
        }


        // Método para actualizar una reserva
        public ReservaDto UpdateReserva(ReservaDto reservaModel, int userId)
        {
            return reserReposiyoty.UpdateReserva(reservaModel,userId); // Asegúrate de que este método exista en tu repositorio
        }
    }
}
