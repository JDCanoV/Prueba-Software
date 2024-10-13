using GastroByte.Dtos;
using GastroByte.Repositories;
using GastroByte.Utilities;
using System;

namespace GastroByte.Services
{
    public class ReservaService
    {
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
                }
                else
                {
                    responseReservaDto.Response = 0;
                    responseReservaDto.Message = "Algo paso";
                }

                return responseReservaDto;
            }
            catch (Exception e)
            {
                responseReservaDto.Response = 0;
                responseReservaDto.Message = e.InnerException.ToString();
                return responseReservaDto;
            }
        }


    }
}