using System.Security.Claims;
using EasyBooking.Application.Contracts;
using EasyBooking.Application.Dtos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EasyBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _config;
        private readonly ITokenService _tokenService;


        public UsuariosController(
            IUsuarioService usuarioService,
            IEmailService emailService,
            IConfiguration config,
            ITokenService tokenService)
        {
            _usuarioService = usuarioService;
            _emailService = emailService;
            _config = config;
            _tokenService = tokenService;
        }


        [HttpPost("registro")]
        public async Task<IActionResult> Registro([FromBody] RegistroUsuarioDto registroDto)
        {
            var resultado = await _usuarioService.RegistrarUsuarioAsync(registroDto);
            if (resultado == null)
            {
                return BadRequest(new { Message = "No se pudo registrar el usuario. El email ya existe o las contraseñas no coinciden." });
            }

            // Enviar correo de bienvenida
            var subject = "¡Bienvenido a EasyBooking!";
            var body = $@"
        <html>
        <head>
            <style>
                body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
                .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                .header {{ background-color: indianred; padding: 20px; text-align: center; color: white; }}
                .content {{ padding: 20px; }}
                .button {{ display: inline-block; padding: 10px 20px; background-color: #4a90e2; color: white; text-decoration: none; border-radius: 5px; }}
                .footer {{ background-color: #f8f9fa; padding: 15px; text-align: center; font-size: 0.8em; }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>¡Bienvenido a EasyBooking!</h1>
                </div>
                <div class='content'>
                    <p>Hola {resultado.Nombre},</p>
                    <p>¡Gracias por registrarte en <strong>EasyBooking</strong>! Estamos emocionados de tenerte con nosotros.</p>
                    <p>A partir de ahora podrás:</p>
                    <ul>
                        <li>Reservar Hoteles y Paquetes Turísticos en pocos clics</li>
                        <li>Gestionar y revisar tus reservas fácilmente</li>
                        <li>Recibir notificaciones y recordatorios importantes</li>
                    </ul>
                    <p>Para empezar, solo inicia sesión y descubre todo lo que tenemos para ti.</p>
                    <p>Si tienes preguntas o necesitas asistencia, nuestro equipo está aquí para ayudarte.</p>
                    <p>¡Esperamos que disfrutes de tu experiencia en EasyBooking!</p>
                    <p>Saludos,<br>El equipo de EasyBooking</p>
                </div>
                <div class='footer'>
                    <p>© 2025 EasyBooking. Todos los derechos reservados.</p>
                </div>
            </div>
        </body>
        </html>
    ";

            await _emailService.SendEmailAsync(resultado.Email, subject, body);

            return Ok(new { Message = "Usuario registrado con éxito", Data = resultado });
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginUsuarioDto loginDto)
        {
            var resultado = await _usuarioService.LoginAsync(loginDto);
            if (resultado == null)
            {
                return Unauthorized(new { Message = "Credenciales inválidas" });
            }

            return Ok(new { Message = "Inicio de sesión exitoso", Data = resultado });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarUsuario(int id)
        {
            var eliminado = await _usuarioService.EliminarUsuarioAsync(id);
            if (!eliminado)
            {
                return NotFound(new { Message = "Usuario no encontrado" });
            }

            return Ok(new { Message = "Usuario eliminado exitosamente" });
        }

        [Authorize]
        [HttpPost("solicitar-eliminacion")]
        public async Task<IActionResult> SolicitarEliminacion()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? "0");
            var usuario = await _usuarioService.GetUsuarioByIdAsync(userId);
            if (usuario == null)
                return Unauthorized();

            var token = await _tokenService.GenerarTokenEliminacionAsync(userId);
            var confirmUrl = $"{Request.Scheme}://{Request.Host}/api/usuarios/confirmar-eliminacion?token={token}";

            await _emailService.SendEmailAsync(
                usuario.Email,
                "Confirmación para eliminar tu cuenta",
                $@"<p>Hola {usuario.Nombre},</p>
           <p>Recibimos tu solicitud para eliminar la cuenta. Haz clic en el botón para confirmar:</p>
           <a href='{confirmUrl}' style='padding: 10px 20px; background-color: red; color: white; text-decoration: none;'>Confirmar eliminación</a>"
            );

            await HttpContext.SignOutAsync();
            return Ok(new { Message = "Se ha enviado un correo para confirmar la eliminación de tu cuenta." });
        }

        [HttpPost("verificar-contrasena")]
        public async Task<IActionResult> VerificarContrasena([FromBody] VerificarContrasenaDto dto)
        {
            if (dto == null || dto.UsuarioId <= 0 || string.IsNullOrEmpty(dto.Password))
            {
                return BadRequest(new { Message = "Datos inválidos" });
            }

            var esValida = await _usuarioService.ValidarContrasenaAsync(dto.UsuarioId, dto.Password);
            if (!esValida)
            {
                return BadRequest(new { Message = "Contraseña incorrecta." });
            }

            return Ok(new { Message = "Contraseña verificada correctamente." });
        }

        [HttpGet("confirmar-eliminacion")]
        public async Task<IActionResult> ConfirmarEliminacionCuenta(string token)
        {
            // Validar si el token es válido y obtener el ID
            var usuarioIdNullable = await _tokenService.ObtenerUsuarioIdDesdeTokenAsync(token);

            if (usuarioIdNullable == null)
                return BadRequest("Token inválido o expirado.");

            int usuarioId = usuarioIdNullable.Value;

            var usuario = await _usuarioService.GetUsuarioByIdAsync(usuarioId);
            if (usuario == null)
                return NotFound("Usuario no encontrado");

            await _usuarioService.EliminarUsuarioAsync(usuarioId);

            await _emailService.SendEmailAsync(
                usuario.Email,
                "Cuenta eliminada exitosamente",
                "<p>Tu cuenta ha sido eliminada. Esperamos verte de vuelta pronto.</p>"
            );

            var frontendUrl = _config["AppSettings:FrontendUrl"];
            var redirectUrl = $"{frontendUrl}/Usuarios/CuentaEliminada";

            return Redirect(redirectUrl);
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var usuario = await _usuarioService.GetUsuarioByIdAsync(id);
            if (usuario == null)
            {
                return NotFound(new { Message = "Usuario no encontrado" });
            }

            return Ok(new { Data = usuario });
        }
    }
}