using Colegios.Data;
using Colegios.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
namespace Colegios.Controllers
{
    [Route("usuarios")]
    [ApiController]
    public class UsuariosController : Controller
    {
        public IConfiguration _configuration;
        public UsuariosController(IConfiguration configuration) {
            _configuration = configuration;
        }

        [HttpPost]
        public async Task CrearUsuario([FromBody] Usuarios parametros)
        {
            var funtion = new DatabaseDatos();
            await funtion.InsertarUsuario(parametros); 
        }


        [HttpPut("{id}")]
        public async Task<object> EditarUsuario(int id, [FromBody] Usuarios parametros)
        {
            var funtion = new DatabaseDatos();
            parametros.Id = id;
            await funtion.UpdateUsuario(parametros);

            return new
            {
                message = "Tu información de usaurio ha sido actualizada"
            };
        }


        [HttpPost]
        [Route("login")]
        public dynamic IniciarSesion([FromBody] Object optData)
        {
            var data = JsonConvert.DeserializeObject<dynamic>(optData.ToString());

            string user = data.email.ToString();
            string password = data.password.ToString();


            DataTable tUsuarios = DatabaseDatos.Listar("SP_ConsultarUsuarios");
            string jsonUsuarios = JsonConvert.SerializeObject(tUsuarios);
            var dbUsuarios = JsonConvert.DeserializeObject<List<Usuarios>>(jsonUsuarios);

            Usuarios usuario = dbUsuarios.FirstOrDefault(x => x.Email == user && x.Password == password);

            if (usuario == null)
            {
                return new
                {
                    success = false,
                    message = "Credenciales incorrectas",
                };
            }

            var jwt = _configuration.GetSection("Jwt").Get<Jwt>();

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, jwt.Subject),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
                new Claim("email", usuario.Email)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var singIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                    jwt.Issuer,
                    jwt.Audience,
                    claims,
                    expires: DateTime.Now.AddMinutes(60),
                    signingCredentials: singIn
                );

            return new
            {
                success = true,
                message = "Se ha iniciado sesión",
                result = new JwtSecurityTokenHandler().WriteToken(token)
            };
        }


        [HttpDelete]
        public dynamic EliminarUsuario([FromQuery] int id)
        {

            List<Parametro> parametro = new List<Parametro>
            {
                new Parametro("@id", id.ToString())
            };

            bool exito = DatabaseDatos.Ejecutar("SP_EliminarUsuario", parametro);

            return new
            {
                success = exito,
                message = exito ? "Usuario eliminado exitosamente" : "Error, no se ha podido eliminar este usuario o este id no existe"
            };

        }

        [HttpGet]
        [Route("activos")]
        public dynamic ListarUsuariosActivos()
        {
            DataTable tUsuarios = DatabaseDatos.Listar("SP_ConsultarUsuariosActivos");
            string jsonUsuarios = JsonConvert.SerializeObject(tUsuarios);
            return JsonConvert.DeserializeObject<List<Usuarios>>(jsonUsuarios);
        }

        [HttpGet]
        [Route("nombre")]
        public dynamic ListarUsuariosPorNombre(string nombre)
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@nombre", nombre)
            };

            DataTable tUsuarios = Data.DatabaseDatos.Listar("SP_ConsultarUsuariosPorNombre", parametros);
            string jsonUsuarios = JsonConvert.SerializeObject(tUsuarios);
            return JsonConvert.DeserializeObject<List<Usuarios>>(jsonUsuarios);
        }


        [HttpGet]
        [Route("rol")]
        public dynamic ListarUsuariosPorRol([FromQuery] int rol)
        {
            List<Parametro> parametros = new List<Parametro>
            {
                new Parametro("@rol", rol.ToString())
            };

            DataTable tUsuarios = Data.DatabaseDatos.Listar("SP_ConsultarUsuariosPorRol", parametros);
            string jsonUsuarios = JsonConvert.SerializeObject(tUsuarios);
            return JsonConvert.DeserializeObject<List<Usuarios>>(jsonUsuarios);
        }
    }
}
