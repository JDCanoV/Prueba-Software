using GastroByte.Dtos;
using GastroByte.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GastroByte.Repositories
{
    public class PedidoRepository
    {
        public int CreatePedido(PedidoDto pedido)
        {
            int comando = 0;
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            string SQL = "INSERT INTO Gastrobyte.dbo.[Pedidos] (tipo_pedido, precio_total, nombre, cedula, telefono, direccion, correo)" +
                         "VALUES (@tipo_pedido, @precio_total, @nombre, @cedula, @telefono, @direccion, @correo);";

            using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
            {
                command.Parameters.AddWithValue("@tipo_pedido", pedido.tipo_pedido);
                command.Parameters.AddWithValue("@precio_total", pedido.precio_total);
                command.Parameters.AddWithValue("@nombre", pedido.nombre);
                command.Parameters.AddWithValue("@cedula", pedido.cedula);
                command.Parameters.AddWithValue("@telefono", pedido.telefono);
                command.Parameters.AddWithValue("@direccion", string.IsNullOrEmpty(pedido.direccion) ? DBNull.Value : (object)pedido.direccion);
                command.Parameters.AddWithValue("@correo", pedido.correo);

                try
                {
                    comando = command.ExecuteNonQuery();  // Aquí obtenemos el número de filas afectadas
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            connection.Disconnect();
            return comando;  // Regresamos el número de filas afectadas (int)
        }

        public IEnumerable<PedidoDto> GetAllPedidos()
        {
            List<PedidoDto> pedidos = new List<PedidoDto>();
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            string SQL = "SELECT * FROM Gastrobyte.dbo.[Pedidos]";

            using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        pedidos.Add(new PedidoDto
                        {
                            id_pedido = (int)reader["id_platillo"],
                            nombre = reader["nombre_platillo"].ToString(),
                            cedula = reader["descripcion"].ToString(),
                        });
                    }
                }
            }

            connection.Disconnect();
            return pedidos;
        }

        public PedidoDto GetPedidoById(int id)
        {
            PedidoDto pedido = null;
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            string SQL = "SELECT * FROM Gastrobyte.dbo.[Pedidos] WHERE id_pedido = @id";

            using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
            {
                command.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        pedido = new PedidoDto
                        {
                            id_pedido = (int)reader["id_platillo"],
                            nombre = reader["nombre_platillo"].ToString(),
                            cedula = reader["descripcion"].ToString(),
                        };
                    }
                }
            }

            connection.Disconnect();
            return pedido;
        }

        public PedidoDto UpdatePedido(PedidoDto PedidoModel)
        {
            PedidoDto updatedPedido = null;
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            string SQL = "UPDATE Gastrobyte.dbo.[Pedidos] SET nombre = @nombre, tipo_pedido = @tipo_pedido, cedula = @cedula " +
                 "WHERE id_platillo = @id;";

            using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
            {
                command.Parameters.AddWithValue("@id", PedidoModel.id_pedido);
                command.Parameters.AddWithValue("@tipo_pedido", PedidoModel.tipo_pedido);
                command.Parameters.AddWithValue("@nombre", PedidoModel.nombre);
                command.Parameters.AddWithValue("@cedula", PedidoModel.cedula);

                try
                {
                    if (command.ExecuteNonQuery() > 0) // Si la actualización fue exitosa
                    {
                        updatedPedido = PedidoModel; // Devolvemos el objeto actualizado
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de error
                    Console.WriteLine(ex.Message);
                }
            }

            connection.Disconnect();
            return updatedPedido; // Devolvemos el objeto actualizado o null si no hubo actualización
        }
        public bool DeletePedido(int id)
        {
            bool isDeleted = false;
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            string SQL = "DELETE FROM Gastrobyte.dbo.[Pedidos] WHERE id_pedido = @id";

            using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
            {
                command.Parameters.AddWithValue("@id", id);

                try
                {
                    isDeleted = command.ExecuteNonQuery() > 0;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            connection.Disconnect();
            return isDeleted;
        }

    }
}
