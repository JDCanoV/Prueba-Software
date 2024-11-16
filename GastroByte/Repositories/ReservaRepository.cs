using GastroByte.Dtos;
using GastroByte.Utilities;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace GastroByte.Repositories
{
    public class ReservaRepository
    {
        public int CreateReser(ReservaDto reser)
        {
            int comando = 0;
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();
            string SQL = "INSERT INTO Gastrobyte.dbo.[Reservaciones] (documento, fecha, email, nombre, numero_personas, hora)" +
                         "VALUES (@documento, @fecha, @email, @nombre, @numero_personas, @hora);";

            using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
            {
                command.Parameters.AddWithValue("@documento", reser.documento);
                command.Parameters.AddWithValue("@fecha", reser.fecha);
                command.Parameters.AddWithValue("@email", reser.email);
                command.Parameters.AddWithValue("@nombre", reser.nombre);
                command.Parameters.AddWithValue("@numero_personas", reser.numero_personas);
                command.Parameters.AddWithValue("@hora", reser.hora);
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

        public IEnumerable<ReservaDto> GetAllReservas()
        {
            List<ReservaDto> reservas = new List<ReservaDto>();
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            string SQL = "SELECT * FROM Gastrobyte.dbo.[Reservaciones]";

            using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservas.Add(new ReservaDto
                        {
                            id_reservacion = (int)reader["id_reservacion"],
                            documento = reader["documento"].ToString(),
                            fecha = Convert.ToDateTime(reader["fecha"]),
                            email = reader["email"].ToString(),
                            nombre = reader["nombre"].ToString(),
                            numero_personas = reader["numero_personas"].ToString(),
                            hora = reader["hora"].ToString(),
                        });
                    }
                }
            }

            connection.Disconnect();
            return reservas;
        }

        public IEnumerable<ReservaDto> GetReservasPorFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            List<ReservaDto> reservas = new List<ReservaDto>();
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            string SQL = "SELECT * FROM Gastrobyte.dbo.[Reservaciones] WHERE fecha BETWEEN @fechaInicio AND @fechaFin";

            using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
            {
                command.Parameters.AddWithValue("@fechaInicio", fechaInicio);
                command.Parameters.AddWithValue("@fechaFin", fechaFin);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        reservas.Add(new ReservaDto
                        {
                            id_reservacion = (int)reader["id_reservacion"],
                            documento = reader["documento"].ToString(),
                            fecha = Convert.ToDateTime(reader["fecha"]),
                            email = reader["email"].ToString(),
                            nombre = reader["nombre"].ToString(),
                            numero_personas = reader["numero_personas"].ToString(),
                            hora = reader["hora"].ToString(),
                        });
                    }
                }
            }

            connection.Disconnect();
            return reservas;
        }


        public ReservaDto GetReservaById(int id)
        {
            ReservaDto reserva = null;
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            string SQL = "SELECT * FROM Gastrobyte.dbo.[Reservaciones] WHERE id_reservacion = @id";

            using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
            {
                command.Parameters.AddWithValue("@id", id);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        reserva = new ReservaDto
                        {
                            id_reservacion = (int)reader["id_reservacion"],
                            documento = reader["documento"].ToString(),
                            fecha = Convert.ToDateTime(reader["fecha"]),
                            email = reader["email"].ToString(),
                            nombre = reader["nombre"].ToString(),
                            numero_personas = reader["numero_personas"].ToString(),
                            hora = reader["hora"].ToString(),
                        };
                    }
                }
            }

            connection.Disconnect();
            return reserva;
        }



        public ReservaDto UpdateReserva(ReservaDto reservaModel, int userId)
{
    ReservaDto updatedReserva = null;
    DBContextUtility connection = new DBContextUtility();
    connection.Connect();

    using (SqlCommand command = new SqlCommand("sp_ActualizarReserva", connection.CONN()))
    {
        command.CommandType = CommandType.StoredProcedure;

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

        // Agregar los parámetros para actualizar la reserva
        command.Parameters.AddWithValue("@id_reservacion", reservaModel.id_reservacion);
        command.Parameters.AddWithValue("@documento", reservaModel.documento);
        command.Parameters.AddWithValue("@fecha", reservaModel.fecha);
        command.Parameters.AddWithValue("@email", reservaModel.email);
        command.Parameters.AddWithValue("@nombre", reservaModel.nombre);
        command.Parameters.AddWithValue("@numero_personas", reservaModel.numero_personas);
        command.Parameters.AddWithValue("@hora", reservaModel.hora);

        try
        {
            // Ejecutar el procedimiento almacenado
            var resultado = command.ExecuteScalar();

            // Si el SP retorna 1, significa que la actualización fue exitosa
            if (resultado != null && (int)resultado == 1)
            {
                updatedReserva = reservaModel;
            }
        }
        catch (Exception ex)
        {
            // Captura errores del procedimiento almacenado
            Console.WriteLine($"Error al actualizar la reserva: {ex.Message}");
            Console.WriteLine($"Detalles: {ex.StackTrace}");
        }
        finally
        {
            // Asegura la desconexión de la base de datos
            connection.Disconnect();
        }
    }

    return updatedReserva; // Devuelve el objeto actualizado o null si hubo un error
}



    }
}
