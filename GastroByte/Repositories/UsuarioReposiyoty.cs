using GastroByte.Dtos;
using GastroByte.Utilities;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace GastroByte.Repositories
{
    public class UsuarioReposiyoty

    {
        public int CreateUser(UsuarioDto user)
        {
            int comando = 0;
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();
            string SQL = "INSERT INTO Gastrobyte.dbo.[Usuario] (id_rol,id_estado,nombre,contraseña,telefono,apellidos,tipo_documento,numero_documento,correo_electronico)" +
                          "VALUES ( @id_rol,@id_estado,@nombre,@contraseña,@telefono,@apellidos,@tipo_documento,@numero_documento,@correo_electronico);";

            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@id_rol", user.id_rol);
                command.Parameters.AddWithValue("@id_estado", user.id_estado);
                command.Parameters.AddWithValue("@nombre", user.nombre);
                command.Parameters.AddWithValue("@contraseña", user.contrasena);
                command.Parameters.AddWithValue("@telefono", user.telefono);
                command.Parameters.AddWithValue("@apellidos", user.apellidos);
                command.Parameters.AddWithValue("@tipo_documento", user.tipo_documento);
                command.Parameters.AddWithValue("@numero_documento", user.numero_documento);
                command.Parameters.AddWithValue("@correo_electronico", user.correo_electronico);

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

        public bool BuscarUsuario(string username)
        {
            bool result = false;
            string SQL = "SELECT id_usuario,id_estado,nombre,contraseña " +
                "FROM Gastrobyte.dbo.[Usuario] " +
                "WHERE nombre = '" + username + "';";
            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();
            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        result = true;
                    }
                }
            }
            Connection.Disconnect();

            return result;
        }


        
    }
}