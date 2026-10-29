namespace SistemaLogin.Models
{
    public class Usuario
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public bool Activo { get; set; }
        public Usuario(int id, string email, string password, bool activo) { Id = id;
            Email = email;
            Password = password;
            Activo = activo;
        }
    }
}