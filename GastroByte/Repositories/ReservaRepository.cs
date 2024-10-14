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
        public int CreateReser(ReservaDto reser)
        {
            int comando = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();
            string SQL = "INSERT INTO Gastrobyte.dbo.[Reservaciones] (documento,fecha,email)" +
                          "VALUES (@documento,@fecha,@email);";

            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@documento", reser.documento);
                command.Parameters.AddWithValue("@fecha", reser.fecha);
                command.Parameters.AddWithValue("@email", reser.email);
                
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