using GastroByte.Dtos;
using GastroByte.Repositories;
using GastroByte.Utilities;
using System;
<<<<<<< Updated upstream
=======
using System.Collections.Generic;
>>>>>>> Stashed changes

namespace GastroByte.Services
{
    public class ReservaService
    {
<<<<<<< Updated upstream
        public ReservaDto CreateReservation(ReservaDto reservaModel)
        {
            ReservaDto responseReservaDto = new ReservaDto();
            ReservaRepository reservaRepository = new ReservaRepository();
            try
            {
                if (reservaRepository.CreateReservation(reservaModel) != 0)
                {
                    responseReservaDto.Response = 1;
                    responseReservaDto.Message = "Creacion exitosa";
=======
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
>>>>>>> Stashed changes
                }
                else
                {
                    responseReservaDto.Response = 0;
<<<<<<< Updated upstream
                    responseReservaDto.Message = "Algo paso";
=======
                    responseReservaDto.Message = "Algo pasó";
>>>>>>> Stashed changes
                }

                return responseReservaDto;
            }
            catch (Exception e)
            {
                responseReservaDto.Response = 0;
<<<<<<< Updated upstream
                responseReservaDto.Message = e.InnerException.ToString();
=======
                responseReservaDto.Message = e.InnerException?.ToString() ?? e.Message; // Manejo de error
>>>>>>> Stashed changes
                return responseReservaDto;
            }
        }

<<<<<<< Updated upstream

    }
}
=======
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
>>>>>>> Stashed changes
