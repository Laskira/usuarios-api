namespace Colegios.Models
{
    public class Usuarios
    {
        public int? Id { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Celular { get; set; }
        public int RolId { get; set; }
        public DateTime FechaIngreso { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public int? EstadoId { get; set; }
    }

}
