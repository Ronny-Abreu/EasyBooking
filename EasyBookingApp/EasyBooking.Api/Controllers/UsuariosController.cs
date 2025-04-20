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
            var subject = "🌟 Bienvenido a EasyBooking";

            var body = $@"
                <!DOCTYPE html>
                <html lang=""es"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Bienvenido a EasyBooking</title>
                    <style>
                        body {{
                            font-family: 'Segoe UI', Arial, sans-serif;
                            background-color: #f8f9fa;
                            margin: 0;
                            padding: 0;
                            color: #333;
                        }}
                        .container {{
                            max-width: 600px;
                            margin: 20px auto;
                            background-color: #ffffff;
                            border-radius: 16px;
                            overflow: hidden;
                            box-shadow: 0 10px 30px rgba(0,0,0,0.08);
                        }}
                        .header {{
                            background: linear-gradient(135deg, #f8345c, #e02a4e);
                            color: #ffffff;
                            padding: 40px 20px;
                            text-align: center;
                        }}
                        .header h1 {{
                            margin: 0;
                            font-size: 26px;
                            font-weight: 700;
                        }}
                        .content {{
                            padding: 40px 30px;
                            font-size: 16px;
                            line-height: 1.6;
                        }}
                        .greeting {{
                            font-size: 20px;
                            font-weight: 600;
                            color: #f8345c;
                            margin-bottom: 20px;
                        }}
                        .features {{
                            background-color: #f9f9f9;
                            border-radius: 12px;
                            padding: 25px;
                            margin: 25px 0;
                        }}
                        .features-title {{
                            font-weight: 600;
                            margin-bottom: 15px;
                            font-size: 18px;
                        }}
                        .features li {{
                            margin-bottom: 10px;
                            list-style: none;
                        }}
                        .features li::before {{
                            content: '✓ ';
                            color: #f8345c;
                            font-weight: bold;
                        }}
                        .cta-button {{
                            display: inline-block;
                            padding: 14px 30px;
                            background: linear-gradient(135deg, #f8345c, #e02a4e);
                            color: white;
                            text-decoration: none;
                            border-radius: 50px;
                            font-weight: 600;
                            font-size: 16px;
                            margin-top: 20px;
                        }}
                        .signature {{
                            margin-top: 30px;
                            font-weight: 500;
                        }}
                        .signature-name {{
                            font-weight: 600;
                            color: #333;
                        }}
                        .footer {{
                            background-color: #f1f1f1;
                            padding: 25px 20px;
                            text-align: center;
                            font-size: 14px;
                            color: #666666;
                            border-top: 1px solid #eeeeee;
                        }}
                    </style>
                </head>
                <body>
                    <div class=""container"">
                        <div class=""header"">
                            <h1>¡Bienvenido a la experiencia EasyBooking!</h1>
                        </div>
                        <div class=""content"">
                            <p class=""greeting"">Hola {resultado.Nombre},</p>
                            <p>¡Gracias por unirte a nuestra comunidad de viajeros! Estamos emocionados de tenerte con nosotros y ayudarte a descubrir destinos increíbles.</p>
                            <div class=""features"">
                                <p class=""features-title"">Con tu cuenta de EasyBooking podrás:</p>
                                <ul>
                                    <li>Reservar hoteles y paquetes turísticos con las mejores tarifas</li>
                                    <li>Gestionar tus reservas fácilmente</li>
                                    <li>Recibir ofertas exclusivas y personalizadas</li>
                                    <li>Acceder a atención al cliente 24/7</li>
                                </ul>
                            </div>
                            <div style=""text-align: center;"">
                                <a class=""cta-button"" href=""https://localhost:7243/Home"">Explorar Destinos</a>
                            </div>
                            <div class=""signature"">
                                <p>¡Felices viajes!</p>
                                <p class=""signature-name"">El equipo de EasyBooking</p>
                            </div>
                        </div>
                        <div class=""footer"">
                            <p>© 2025 EasyBooking. Todos los derechos reservados.</p>
                            <p>Calle Principal 123, Ciudad, País</p>
                        </div>
                    </div>
                </body>
                </html>";



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