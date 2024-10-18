using GastroByte.Dtos;
using GastroByte.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GastroByte.Repositories
{
    public class ReservaRepository
    {
        public int CreateReservation(ReservaDto reserva)
        {
            int comando = 0;
<<<<<<< Updated upstream
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();
            string SQL = "INSERT INTO Gastrobyte.dbo.[Reservaciones] (id_usuario, numero_personas, mesa, id_estado, email, nombre, documento, fecha, hora) " +
                         "VALUES (@id_usuario, @numero_personas, @mesa, @id_estado, @email, @nombre, @documento, @fecha, @hora);";
=======
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();
            string SQL = "INSERT INTO Gastrobyte.dbo.[Reservaciones] (documento,fecha,email,nombre,numero_personas,hora) " +
                         "VALUES (@documento,@fecha,@email,@nombre,@numero_personas,@hora);";
>>>>>>> Stashed changes

            using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
            {
<<<<<<< Updated upstream
                command.Parameters.AddWithValue("@id_usuario", reserva.id_usuario);
                command.Parameters.AddWithValue("@numero_personas", reserva.numero_personas);
                command.Parameters.AddWithValue("@mesa", reserva.mesa);
                command.Parameters.AddWithValue("@id_estado", reserva.id_estado);
                command.Parameters.AddWithValue("@email", reserva.email);
                command.Parameters.AddWithValue("@nombre", reserva.nombre);
                command.Parameters.AddWithValue("@documento", reserva.documento);
                command.Parameters.AddWithValue("@hora", reserva.hora);
                command.Parameters.AddWithValue("@fecha", reserva.fecha);

=======
                command.Parameters.AddWithValue("@documento", reser.documento);
                command.Parameters.AddWithValue("@fecha", reser.fecha);
                command.Parameters.AddWithValue("@email", reser.email);
                command.Parameters.AddWithValue("@nombre", reser.nombre);
                command.Parameters.AddWithValue("@numero_personas", reser.numero_personas);
                command.Parameters.AddWithValue("@hora", reser.hora);
>>>>>>> Stashed changes
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

            string SQL = "SELECT * FROM Gastrobyte.dbo.[Reservaciones]"; // Cambia esto según tus requerimientos

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
                            fecha = reader["fecha"].ToString(),
                            email = reader["email"].ToString(),
                            nombre = reader["nombre"].ToString(),
                            numero_personas = reader["numero_personas"].ToString(),
                            hora = reader["hora"].ToString(),
                            // Agrega otros campos si es necesario
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
                            fecha = reader["fecha"].ToString(),
                            email = reader["email"].ToString(),
                            nombre = reader["nombre"].ToString(),
                            numero_personas = reader["numero_personas"].ToString(),
                            hora = reader["hora"].ToString(),
                            // Agrega otros campos si es necesario
                        };
                    }
                }
            }

            connection.Disconnect();
            return reserva;
        }

        public ReservaDto UpdateReserva(ReservaDto reservaModel)
        {
            ReservaDto updatedReserva = null;
            DBContextUtility connection = new DBContextUtility();
            connection.Connect();

            string SQL = "UPDATE Gastrobyte.dbo.[Reservaciones] SET documento = @documento, fecha = @fecha, email = @email, " +
                         "nombre = @nombre, numero_personas = @numero_personas, hora = @hora WHERE id_reservacion = @id;";

            using (SqlCommand command = new SqlCommand(SQL, connection.CONN()))
            {
                command.Parameters.AddWithValue("@id", reservaModel.id_reservacion);
                command.Parameters.AddWithValue("@documento", reservaModel.documento);
                command.Parameters.AddWithValue("@fecha", reservaModel.fecha);
                command.Parameters.AddWithValue("@email", reservaModel.email);
                command.Parameters.AddWithValue("@nombre", reservaModel.nombre);
                command.Parameters.AddWithValue("@numero_personas", reservaModel.numero_personas);
                command.Parameters.AddWithValue("@hora", reservaModel.hora);

                try
                {
                    if (command.ExecuteNonQuery() > 0) // Si la actualización fue exitosa
                    {
                        updatedReserva = reservaModel; // Devolvemos el objeto actualizado
                    }
                }
                catch (Exception ex)
                {
                    // Manejo de error
                    Console.WriteLine(ex.Message);
                }
            }

            connection.Disconnect();
            return updatedReserva; // Devolvemos el objeto actualizado o null si no hubo actualización
        }
    }
}
