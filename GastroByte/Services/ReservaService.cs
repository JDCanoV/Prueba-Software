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

        // Método para obtener una reserva por ID
        public ReservaDto GetReservaById(int id)
        {
            return reserReposiyoty.GetReservaById(id); // Este método también debería funcionar
        }

        // Método para actualizar una reserva
        public ReservaDto UpdateReserva(ReservaDto reservaModel)
        {
            return reserReposiyoty.UpdateReserva(reservaModel); // Asegúrate de que este método exista en tu repositorio
        }
    }
}
