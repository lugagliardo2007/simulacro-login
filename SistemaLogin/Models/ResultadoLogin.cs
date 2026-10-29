namespace SistemaLogin.Models
{
    public class ResultadoLogin
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; }

        public int? UsuarioId { get; set; }

        public ResultadoLogin(bool exitoso, string mensaje, int? usuarioId = null)
        {
            Exitoso = exitoso;
            Mensaje = mensaje;
            UsuarioId = usuarioId;
        }
    }
}

