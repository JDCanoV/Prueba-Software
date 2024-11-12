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
        public UsuarioDto BuscarUsuarioPorNumeroDocumento(string numeroDocumento)
        {
            UsuarioDto user = null;
            string SQL = "SELECT id_usuario, nombre, contraseña, id_rol, id_estado, numero_documento, telefono, correo_electronico " +
                         "FROM Gastrobyte.dbo.[Usuario] WHERE numero_documento = @numero_documento";

            DBContextUtility Connection = new DBContextUtility();
            Connection.Connect();

            using (SqlCommand command = new SqlCommand(SQL, Connection.CONN()))
            {
                command.Parameters.AddWithValue("@numero_documento", numeroDocumento);
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        user = new UsuarioDto
                        {
                            id_usuario = (int)reader["id_usuario"],
                            nombre = reader["nombre"].ToString(),
                            contrasena = reader["contraseña"].ToString(),
                            id_rol = (int)reader["id_rol"],
                            id_estado = (int)reader["id_estado"],
                            numero_documento = reader["numero_documento"].ToString(),
                            telefono = reader["telefono"].ToString(),
                            correo_electronico = reader["correo_electronico"].ToString()
                        };
                    }
                }
            }
            Connection.Disconnect();
            return user;
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