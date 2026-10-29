using SistemaLogin.Models;
using SistemaLogin.Services;
using Xunit;

namespace AuthTests
{
    public class PruebasUnitarias
    {
        private LoginService CrearServicio()
        {
            var usuarios = new List<Usuario>
            {
                new Usuario(1, "yo@gmail.com", "123456", true),
                new Usuario(2, "inactivo@gmail.com", "abcdef", false)
            };
            return new LoginService(usuarios);
        }

        [Fact]
        public void ProbarLoginCorrecto()
        {
            // Arrange (Carga)
            var servicio = CrearServicio();

            // Act (Procesamiento)
            ResultadoLogin respuesta = servicio.IniciarSesion("yo@gmail.com", "123456");

            // Assert (Verificación)
            Assert.True(respuesta.Exitoso);
            Assert.Equal("Inicio de sesión correcto", respuesta.Mensaje);
            Assert.Equal(1, respuesta.UsuarioId);
        }

        [Theory]
        [InlineData("nadie@gmail.com", "123456", "Usuario inexistente")]
        [InlineData("inactivo@gmail.com", "abcdef", "Usuario inactivo")]
        [InlineData("yo@gmail.com", "incorrecta", "Contraseña incorrecta")]
        [InlineData("", "", "Usuario inexistente")]
        public void ProbarLoginFallido(string email, string password, string mensajeEsperado)
        {
            // Arrange
            var servicio = CrearServicio();

            // Act
            ResultadoLogin respuesta = servicio.IniciarSesion(email, password);

            // Assert
            Assert.False(respuesta.Exitoso);
            Assert.Equal(mensajeEsperado, respuesta.Mensaje);
            Assert.Null(respuesta.UsuarioId);
        }
    }
}