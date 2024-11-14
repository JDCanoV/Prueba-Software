using GastroByte.Dtos;
using GastroByte.Repositories;
using GastroByte.Utilities;
using System;
using System.Collections.Generic;

namespace GastroByte.Services
{
    public class PedidoService
    {
        private readonly PedidoRepository pedidoRepository;

        // Constructor
        public PedidoService()
        {
            pedidoRepository = new PedidoRepository(); // Asignación correcta a la variable de campo
        }

        // Método para crear una nueva reserva
        public PedidoDto CreatePedido(PedidoDto pedidoModel)
        {
            PedidoDto responsePedidoDto = new PedidoDto();
            try
            {
                if (pedidoRepository.CreatePedido(pedidoModel) != 0)
                {
                    responsePedidoDto.Response = 1;
                    responsePedidoDto.Message = "Creación exitosa";
                }
                else
                {
                    responsePedidoDto.Response = 0;
                    responsePedidoDto.Message = "Algo pasó";
                }

                return responsePedidoDto;
            }
            catch (Exception e)
            {
                responsePedidoDto.Response = 0;
                responsePedidoDto.Message = e.InnerException?.ToString() ?? e.Message; // Manejo de error
                return responsePedidoDto;
            }
        }

        // Método para obtener todas las reservas
        public IEnumerable<PedidoDto> GetAllMenus()
        {
            return pedidoRepository.GetAllPedidos(); // Esto ahora debería funcionar correctamente
        }

        // Método para obtener una reserva por ID
        public PedidoDto GetMenuById(int id)
        {
            return pedidoRepository.GetPedidoById(id); // Este método también debería funcionar
        }

        // Método para actualizar una reserva
        public PedidoDto UpdateMenu(PedidoDto menuModel)
        {
            return pedidoRepository.UpdatePedido(menuModel); // Asegúrate de que este método exista en tu repositorio
        }
        public bool DeleteMenu(int id)
        {
            return pedidoRepository.DeletePedido(id);
        }

    }
}
