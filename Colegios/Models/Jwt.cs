using Colegios.Data;
using Newtonsoft.Json;
using System.Data;
using System.Security.Claims;

namespace Colegios.Models
{
    public class Jwt
    {

        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public string Subject { get; set; }

        public static dynamic ValidarToken(ClaimsIdentity identity)
        {
            DataTable tUsuarios = DatabaseDatos.Listar("SP_ConsultarUsuariosActivos");
            string jsonUsuarios = JsonConvert.SerializeObject(tUsuarios);
            var dbUsuarios = JsonConvert.DeserializeObject<List<Usuarios>>(jsonUsuarios);


            try
            {
                if (identity.Claims.Count() == 0)
                {
                    return new
                    {
                        success = false,
                        message = "Verificar si estas enviando un token valido",
                        result = ""
                    };
                }

                var email = identity.Claims.FirstOrDefault(x => x.Type == "Email").Value;

                Usuarios usuario = dbUsuarios.FirstOrDefault(x => x.Email == email);

                return new
                {
                    success = true,
                    message = "Haz logrado loggearte exitosamente",
                    result = usuario
                };
            }
            catch (Exception ex)
            {
                return new
                {
                    success = false,
                    message = "Catch: " + ex.Message,
                    result = ""
                };
            }
        }


    }
}
