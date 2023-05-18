using System.Data.SqlClient;

namespace Colegios.Data
{
    public class Conexion
    {
        public SqlConnection connection = new SqlConnection("Data Source=Fresita\\SQLEXPRESS;Initial Catalog=Colegios;Integrated Security=True");
    }
}
