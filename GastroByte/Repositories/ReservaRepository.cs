using GastroByte.Dtos;
using GastroByte.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GastroByte.Repositories
{
    public class ReservaRepository
    {
        public int CreateReservation(ReservaDto reserva)
        {
            int comando = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();
            string SQL = "INSERT INTO Gastrobyte.dbo.[Reservaciones] (id_usuario, numero_personas, mesa, id_estado, email, nombre, documento, fecha, hora) " +
                         "VALUES (@id_usuario, @numero_personas, @mesa, @id_estado, @email, @nombre, @documento, @fecha, @hora);";

            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@id_usuario", reserva.id_usuario);
                command.Parameters.AddWithValue("@numero_personas", reserva.numero_personas);
                command.Parameters.AddWithValue("@mesa", reserva.mesa);
                command.Parameters.AddWithValue("@id_estado", reserva.id_estado);
                command.Parameters.AddWithValue("@email", reserva.email);
                command.Parameters.AddWithValue("@nombre", reserva.nombre);
                command.Parameters.AddWithValue("@documento", reserva.documento);
                command.Parameters.AddWithValue("@hora", reserva.hora);
                command.Parameters.AddWithValue("@fecha", reserva.fecha);

                try
                {
                    comando = command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    // Manejo de error, registra el error
                    // Puedes usar un logger o escribir a la consola
                    Console.WriteLine(ex.Message);
                }
            }
            Connection.Disconnect();
            return comando;
        }
    }
}
