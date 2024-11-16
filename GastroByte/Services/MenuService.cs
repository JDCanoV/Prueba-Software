using GastroByte.Dtos;
using GastroByte.Repositories;
using GastroByte.Utilities;
using System;
using System.Collections.Generic;

namespace GastroByte.Services
{
    public class MenuService
    {
        private readonly MenuRepository menuRepository;

        // Constructor
        public MenuService()
        {
            menuRepository = new MenuRepository(); // Asignación correcta a la variable de campo
        }

        // Método para crear una nueva reserva
        public MenuDto CreateMenu(MenuDto menuModel)
        {
            MenuDto responseMenuDto = new MenuDto();
            try
            {
                if (menuRepository.CreateMenu(menuModel) != 0)
                {
                    responseMenuDto.Response = 1;
                    responseMenuDto.Message = "Creación exitosa";
                }
                else
                {
                    responseMenuDto.Response = 0;
                    responseMenuDto.Message = "Algo pasó";
                }

                return responseMenuDto;
            }
            catch (Exception e)
            {
                responseMenuDto.Response = 0;
                responseMenuDto.Message = e.InnerException?.ToString() ?? e.Message; // Manejo de error
                return responseMenuDto;
            }
        }

        // Método para obtener todas las reservas
        public IEnumerable<MenuDto> GetAllMenus()
        {
            return menuRepository.GetAllMenus(); // Esto ahora debería funcionar correctamente
        }

        // Método para obtener una reserva por ID
        public MenuDto GetMenuById(int id)
        {
            return menuRepository.GetMenuById(id); // Este método también debería funcionar
        }

        // Método para actualizar una reserva
        public MenuDto UpdateMenu(MenuDto menuModel,int iduser)
        {
            return menuRepository.UpdateMenu(menuModel,iduser); // Asegúrate de que este método exista en tu repositorio
        }
        public bool DeleteMenu(int id)
        {
            return menuRepository.DeleteMenu(id);
        }

    }
}
