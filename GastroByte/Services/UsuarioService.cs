using GastroByte.Dtos;
using GastroByte.Repositories;
using GastroByte.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GastroByte.Services
{
    public class UsuarioService
    {
        public UsuarioDto CreateUser(UsuarioDto userModel)
        {
            UsuarioDto responseUserDto = new UsuarioDto();
            UsuarioReposiyoty userReposiyoty = new UsuarioReposiyoty();
            try
            {
                // Primero, verifica si la contraseña es válida (no vacía)
                if (string.IsNullOrEmpty(userModel.contrasena))
                {
                    responseUserDto.Response = 0;
                    
                    return responseUserDto;
                }

                userModel.contrasena = EncryptUtility.HashPassword(userModel.contrasena);

                if (userReposiyoty.BuscarUsuario(userModel.nombre))
                {
                    responseUserDto.Response = 0;
                    responseUserDto.Message = "Usuario ya existe";
                }
                else
                {
                  
                    
                    if (userReposiyoty.CreateUser(userModel) != 0)
                    {
                        responseUserDto.Response = 1;
                        responseUserDto.Message = "Creacion exitosa";
                    }
                    else
                    {
                        responseUserDto.Response = 0;
                        responseUserDto.Message = "Algo paso";
                    }
                }

                return responseUserDto;
            }
            catch (Exception e)
            {
                responseUserDto.Response = 0;
                responseUserDto.Message = e.InnerException.ToString();
                return responseUserDto;
            }
        }

        
}
}