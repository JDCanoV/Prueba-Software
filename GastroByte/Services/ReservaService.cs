using GastroByte.Dtos;
using GastroByte.Repositories;
using GastroByte.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace GastroByte.Services
{
    public class ReservaService
    {
        
            public ReservaDto CreateReser(ReservaDto reserModel)
            {
                ReservaDto responseReservaDto = new ReservaDto();
                ReservaRepository reserReposiyoty = new ReservaRepository();
                try
                {
                    

                    


                        if (reserReposiyoty.CreateReser(reserModel) != 0)
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





