using System.Data.SqlClient;
using System.Data;
using Colegios.Models;

namespace Colegios.Data
{
    public class DatabaseDatos
    {

        public static DataTable Listar(string nombreProcedimiento, List<Parametro> parametros = null)
        {
            Conexion conexion = new Conexion();

            try
            {
                conexion.connection.Open();
                SqlCommand cmd = new SqlCommand(nombreProcedimiento, conexion.connection);
                cmd.CommandType = CommandType.StoredProcedure;

                if (parametros != null)
                {
                    foreach (var parametro in parametros)
                    {
                        cmd.Parameters.AddWithValue(parametro.Nombre, parametro.Valor);
                    }
                }
                DataTable tabla = new DataTable();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);


                return tabla;
            }
            catch (Exception ex)
            {
                return null;
            }
            finally
            {
             conexion.connection.Close();
            }
        }

        public static bool Ejecutar(string nombreProcedimiento, List<Parametro> parametros = null)
        {
            Conexion conexion = new Conexion();

            try
            {
                conexion.connection.Open();
                SqlCommand cmd = new SqlCommand(nombreProcedimiento, conexion.connection);
                cmd.CommandType = CommandType.StoredProcedure;

                if (parametros != null)
                {
                    foreach (var parametro in parametros)
                    {
                        cmd.Parameters.AddWithValue(parametro.Nombre, parametro.Valor);
                    }
                }

                int i = cmd.ExecuteNonQuery();

                return (i > 0) ? true : false;
            }
            catch (Exception ex)
            {
                return false;
            }
            finally
            {
                conexion.connection.Close();
            }
        }


        public async Task InsertarUsuario(Usuarios usuario) {

            Conexion conexion = new Conexion();
           
                SqlCommand cmd = new SqlCommand("SP_CrearUsuario", conexion.connection);
                 cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Nombres", usuario.Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", usuario.Apellidos);
                cmd.Parameters.AddWithValue("@FechaNacimiento", usuario.FechaNacimiento);
                cmd.Parameters.AddWithValue("@Celular", usuario.Celular);
                cmd.Parameters.AddWithValue("@RolId", usuario.RolId);
                cmd.Parameters.AddWithValue("@FechaIngreso", DateTime.Now);
                cmd.Parameters.AddWithValue("@Email", usuario.Email);
                cmd.Parameters.AddWithValue("@Password", usuario.Password);

                await conexion.connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
        }


        public async Task UpdateUsuario(Usuarios usuario)
        {
            if (usuario != null) { 

                Conexion conexion = new Conexion();

                SqlCommand cmd = new SqlCommand("SP_EditarUsuario", conexion.connection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@Id", usuario.Id);
                cmd.Parameters.AddWithValue("@Nombres", usuario.Nombres);
                cmd.Parameters.AddWithValue("@Apellidos", usuario.Apellidos);
                cmd.Parameters.AddWithValue("@FechaNacimiento", usuario.FechaNacimiento);
                cmd.Parameters.AddWithValue("@Celular", usuario.Celular);
                cmd.Parameters.AddWithValue("@RolId", usuario.RolId);
                cmd.Parameters.AddWithValue("@Email", usuario.Email);
                cmd.Parameters.AddWithValue("@Password", usuario.Password);
                cmd.Parameters.AddWithValue("@EstadoId", usuario.EstadoId);

                await conexion.connection.OpenAsync();
                await cmd.ExecuteNonQueryAsync();
             }
        }

    }
}

