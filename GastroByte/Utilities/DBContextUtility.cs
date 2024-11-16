using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GastroByte.Utilities
{
    public class DBContextUtility
    {
        static string SERVER = "LAPTOP-AA823G2C";
        static string DB_NAME = "Gastrobyte";
        static string DB_USER = "Dios2";
        static string DB_PASSWORD = "1234" +
            "" +
            "" +
            "";

        static string Conn = "server=" + SERVER + ";database=" + DB_NAME + ";user id=" + DB_USER + ";password=" + DB_PASSWORD + ";MultipleActiveResultSets=true";
        //mi conexion:
        SqlConnection Con = new SqlConnection(Conn);

        //procedimiento que abre la conexion sqlsever
        public bool Connect()
        {
            try
            {
                Con.Open();
                return true;  // Retorna true si la conexión fue exitosa
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;  // Retorna false si hubo algún error
            }
        }
        //procedimiento que cierra la conexion sqlserver
        public void Disconnect()
        {
            Con.Close();
        }

        //funcion que devuelve la conexion sqlserver
        public SqlConnection CONN()
        {
            return Con;
        }
    }
}