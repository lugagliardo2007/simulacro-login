using SistemaLogin.Models;

namespace SistemaLogin.Services
{
    public class LoginService
    {
        private readonly List<Usuario> usuarios;

        public LoginService(List<Usuario> usuarios)
        {
            this.usuarios = usuarios;
        }

        public ResultadoLogin IniciarSesion(string email, string password)
        {
            Usuario? usuario = usuarios.Find(u => u.Email == email);

            if (usuario == null)
                return new ResultadoLogin(false, "Usuario inexistente");

            if (!usuario.Activo)
                return new ResultadoLogin(false, "Usuario inactivo");

            if (usuario.Password != password)
                return new ResultadoLogin(false, "Contraseña incorrecta");

            return new ResultadoLogin(true, "Inicio de sesión correcto", usuario.Id);
        }
    }
}