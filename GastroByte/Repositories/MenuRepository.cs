using GastroByte.Dtos;
using GastroByte.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GastroByte.Repositories
{
    public class MenuRepository
    {
        public int CreateMenu(MenuDto menu)
        {
            int comando = 0;
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();
            string SQL = "INSERT INTO Gastrobyte.dbo.[Platillos] (nombre_platillo, descripcion, precio)" +
                         "VALUES (@nombre_platillo, @descripcion, @precio);";

            using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
            {
                command.Parameters.AddWithValue("@nombre_platillo", menu.nombre_platillo);
                command.Parameters.AddWithValue("@descripcion", menu.descripcion);
                command.Parameters.AddWithValue("@precio", menu.precio);
                
                try
                {
                    comando = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Manejo de error, registra el error
                    Console.WriteLine(ex.Message);
                }
            }
            connection.Disconnect();
            return comando;
        }

        public IEnumerable<MenuDto> GetAllMenus()
        {
            List<MenuDto> menus = new List<MenuDto>();
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            string SQL = "SELECT * FROM Gastrobyte.dbo.[Platillos]";

            using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        menus.Add(new MenuDto
                        {
                            id_platillo = (int)reader["id_platillo"],
                            nombre_platillo = reader["nombre_platillo"].ToString(),
                            descripcion = reader["descripcion"].ToString(),
                            precio = (int)reader["precio"]
                        });
                    }
                }
            }

            connection.Disconnect();
            return menus;
        }

        public MenuDto GetMenuById(int id)
        {
            MenuDto menu = null;
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            string SQL = "SELECT * FROM Gastrobyte.dbo.[Platillos] WHERE id_platillo = @id";

            using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
            {
                command.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        menu = new MenuDto
                        {
                            id_platillo = (int)reader["id_platillo"],
                            nombre_platillo = reader["nombre_platillo"].ToString(),
                            descripcion = reader["descripcion"].ToString(),
                            precio = (int)reader["precio"]
                        };
                    }
                }
            }

            connection.Disconnect();
            return menu;
        }

        public MenuDto UpdateMenu(MenuDto MenuModel, int userId)
        {
            MenuDto updatedMenu = null;
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            string SQL = "UPDATE Gastrobyte.dbo.[Platillos] SET nombre_platillo = @nombre_platillo, descripcion = @descripcion, precio = @precio " +
                 "WHERE id_platillo = @id;";

            using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
            {
                // Insertar el UserID en la tabla temporal
                using (SqlCommand tempUserIdCommand = new SqlCommand("INSERT INTO TempUserId (UserId) VALUES (@UserId)", connection.CONN()))
                {
                    tempUserIdCommand.Parameters.AddWithValue("@UserId", userId); // Usar el UserID desde el controlador
                    tempUserIdCommand.ExecuteNonQuery();
                }

                // Establecer el UserID en CONTEXT_INFO, si es necesario
                try
                {
                    using (SqlCommand contextCommand = new SqlCommand("SET CONTEXT_INFO @UserID", connection.CONN()))
                    {
                        byte[] userIdBytes = BitConverter.GetBytes(userId); // Usar el valor real del usuario

                        // Agregar el parámetro y ejecutar el comando
                        contextCommand.Parameters.AddWithValue("@UserID", userIdBytes);
                        contextCommand.ExecuteNonQuery();
                    }
                }
                catch (Exception ex)
                {
                    // Captura cualquier excepción
                    Console.WriteLine($"Error al establecer CONTEXT_INFO: {ex.Message}");
                    Console.WriteLine($"Detalles: {ex.StackTrace}");
                    throw; // Relanzar la excepción para que sea manejada más arriba
                }

                command.Parameters.AddWithValue("@id", MenuModel.id_platillo);
                command.Parameters.AddWithValue("@nombre_platillo", MenuModel.nombre_platillo);
                command.Parameters.AddWithValue("@descripcion", MenuModel.descripcion);
                command.Parameters.AddWithValue("@precio", MenuModel.precio);

                try
                {
                    if (command.ExecuteNonQuery() > 0) // Si la actualización fue exitosa
                    {
                        updatedMenu = MenuModel; // Devolvemos el objeto actualizado
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de error
                    Console.WriteLine(ex.Message);
                }
            }

            connection.Disconnect();
            return updatedMenu; // Devolvemos el objeto actualizado o null si no hubo actualización
        }
        public bool DeleteMenu(int id)
        {
            bool isDeleted = false;
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            string SQL = "DELETE FROM Gastrobyte.dbo.[Platillos] WHERE id_platillo = @id";

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
