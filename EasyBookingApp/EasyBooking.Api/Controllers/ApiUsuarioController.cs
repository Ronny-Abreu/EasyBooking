using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using EasyBooking.Application.Contracts;
using EasyBooking.Application.Dtos;
using EasyBooking.Domain.Entities;
using EasyBooking.Application.Services;
using System.Security.Cryptography;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace EasyBooking.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiUsuarioController : ControllerBase
    {
        private readonly IUsuarioService _service;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public ApiUsuarioController(IUsuarioService service, IEmailService emailService, IConfiguration configuration)
        {
            _service = service;
            _emailService = emailService;
            _configuration = configuration;
        }

        // POST: api/ApiUsuario/Login
        [HttpPost("Login")]
        public async Task<ActionResult<UsuarioDto>> Login([FromBody] LoginRequestDto loginDto)
        {
            if (loginDto == null || string.IsNullOrEmpty(loginDto.Email) || string.IsNullOrEmpty(loginDto.Password))
            {
                return BadRequest(new { message = "Email y contraseña son requeridos." });
            }

            var usuario = await _service.ObtenerPorEmailAsync(loginDto.Email);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado." });
            }

            // Verificar la contraseña
            var passwordHasher = new PasswordHasher<Usuario>();
            var result = passwordHasher.VerifyHashedPassword(usuario, usuario.Password, loginDto.Password);

            if (result == PasswordVerificationResult.Failed)
            {
                return Unauthorized(new { message = "Credenciales inválidas." });
            }

            var usuarioDto = new UsuarioDto
            {
                Id = usuario.Id,
                Nombre = usuario.Nombre,
                Apellido = usuario.Apellido,
                Email = usuario.Email,
                Username = usuario.Username,
                Telefono = usuario.Telefono,
                IsEmailVerified = usuario.IsEmailVerified
                // No incluimos la contraseña en la respuesta
            };

            return Ok(new { message = "Inicio de sesión exitoso.", data = usuarioDto });
        }

        // POST: api/ApiUsuario
        [HttpPost]
        public async Task<IActionResult> PostUsuario([FromBody] UsuarioDto usuarioDto)
        {
            try
            {
                if (usuarioDto == null)
                {
                    return BadRequest(new { message = "Los datos del usuario no son válidos." });
                }

                // Verificar si el email ya existe
                var existingUser = await _service.ObtenerPorEmailAsync(usuarioDto.Email);
                if (existingUser != null)
                {
                    return BadRequest(new { message = "El email ya está registrado." });
                }

                // Crear el objeto Usuario a partir del UsuarioDto
                var usuario = new Usuario
                {
                    Id = 0, // El ID se asignará automáticamente en la base de datos
                    Nombre = usuarioDto.Nombre ?? string.Empty,
                    Apellido = usuarioDto.Apellido ?? string.Empty,
                    Email = usuarioDto.Email ?? string.Empty,
                    Username = usuarioDto.Username ?? string.Empty,
                    Telefono = usuarioDto.Telefono ?? string.Empty,
                    IsEmailVerified = false // Por defecto, el email no está verificado
                };

                // Aquí generamos el hash de la contraseña antes de guardarla
                var passwordHasher = new PasswordHasher<Usuario>();
                usuario.Password = passwordHasher.HashPassword(usuario, usuarioDto.Password);  // Usamos la contraseña en texto claro

                // Ahora pasamos un objeto Usuario al servicio
                await _service.CrearUsuarioAsync(usuario);

                // Enviar correo de bienvenida
                await SendWelcomeEmail(usuario.Email, usuario.Nombre);

                return Ok(new { message = "Usuario creado exitosamente." });
            }
            catch (Exception ex)
            {
                // Log the exception
                Console.WriteLine($"Error al crear usuario: {ex.Message}");
                return StatusCode(500, new { message = "Error interno del servidor al crear el usuario." });
            }
        }

        // POST: api/ApiUsuario/SendVerificationEmail
        [HttpPost("SendVerificationEmail")]
        public async Task<IActionResult> SendVerificationEmail([FromQuery] string email, [FromQuery] int userId)
        {
            try
            {
                var usuario = await _service.ObtenerPorEmailAsync(email);
                if (usuario == null || usuario.Id != userId)
                {
                    return NotFound(new { message = "Usuario no encontrado." });
                }

                // Generar token de verificación
                var token = GenerateVerificationToken(usuario.Id, usuario.Email);

                // Construir URL de verificación - Usar el puerto correcto para la API
                var apiBaseUrl = _configuration["AppSettings:BaseUrl"] ?? "https://localhost:7191";
                // Usar el puerto correcto para el frontend
                var frontendBaseUrl = _configuration["AppSettings:WebAppUrl"] ?? "https://localhost:7094";

                // URL para la verificación de la API
                var verificationUrl = $"{apiBaseUrl}/api/ApiUsuario/VerifyEmail?token={token}&redirectUrl={Uri.EscapeDataString(frontendBaseUrl + "/Usuario/EmailVerified")}";

                // Enviar correo de verificación
                var subject = "✅ Verifica tu correo electrónico - EasyBooking";
                var body = $@"
                    <html>
                    <head>
                        <style>
                            body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }}
                    .header {{ background-color: #007bff; padding: 20px; text-align: center; color: white; border-radius: 8px 8px 0 0; }}
                    .content {{ padding: 30px; background-color: #fff; }}
                    .button {{ display: inline-block; padding: 12px 24px; background-color: #007bff; color: white; text-decoration: none; border-radius: 5px; font-weight: bold; margin: 20px 0; }}
                    .button:hover {{ background-color: #0056b3; }}
                    .footer {{ text-align: center; padding: 15px; font-size: 14px; color: #666; }}
                    .emoji {{ font-size: 24px; }}
                        </style>
                    </head>
                    <body>
                        <div class='container'>
                            <div class='header'>
                                <h1>Verifica tu correo electrónico</h1>
                            </div>
                            <div class='content'>
                        <p>¡Hola {usuario.Nombre}! 👋</p>
                        <p>Gracias por registrarte en EasyBooking. 🎉</p>
                        <p>Para completar tu registro y comenzar a disfrutar de todos nuestros servicios, por favor verifica tu dirección de correo electrónico haciendo clic en el siguiente botón:</p>
                        <p style='text-align: center;'><a href='{verificationUrl}' class='button'>✅ Verificar correo electrónico</a></p>
                        <p>Si no puedes hacer clic en el botón, copia y pega la siguiente URL en tu navegador:</p>
                        <p style='background-color: #f5f5f5; padding: 10px; border-radius: 5px; word-break: break-all;'>{verificationUrl}</p>
                        <p>⏰ Este enlace expirará en 24 horas.</p>
                        <p>Si no has solicitado esta verificación, puedes ignorar este correo. 🔒</p>
                        <p>Saludos,<br>El equipo de EasyBooking 🏨</p>
                    </div>
                        </div>
                    </body>
                    </html>
                ";

                await _emailService.SendEmailAsync(usuario.Email, subject, body);

                return Ok(new { message = "Correo de verificación enviado exitosamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar correo de verificación: {ex.Message}");
                return StatusCode(500, new { message = "Error interno del servidor al enviar el correo de verificación." });
            }
        }

        // GET: api/ApiUsuario/VerifyEmail
        [HttpGet("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromQuery] string token)
        {
            try
            {
                // Decodificar y validar el token
                var (isValid, userId, email) = ValidateVerificationToken(token);

                if (!isValid)
                {
                    return BadRequest("El enlace de verificación no es válido o ha expirado.");
                }

                // Obtener el usuario
                var usuario = await _service.ObtenerUsuarioPorIdAsync(userId);
                if (usuario == null || usuario.Email != email)
                {
                    return NotFound("Usuario no encontrado.");
                }

                // Actualizar el estado de verificación del correo
                usuario.IsEmailVerified = true;
                await _service.ActualizarUsuarioAsync(usuario);

                // Obtener la URL base de la aplicación web
                var baseUrl = _configuration["AppSettings:WebAppUrl"] ?? "https://localhost:7094";

                // Redirigir a la página de éxito en la aplicación web
                return Redirect($"{baseUrl}/Usuario/EmailVerified");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar correo: {ex.Message}");
                return StatusCode(500, "Error interno del servidor al verificar el correo electrónico.");
            }
        }
        private string GenerateVerificationToken(int userId, string email)
        {
            // Crear un token que incluya el ID de usuario, el correo y una marca de tiempo
            var tokenData = $"{userId}:{email}:{DateTime.UtcNow.AddHours(24).Ticks}";

            // Encriptar el token
            var key = _configuration["AppSettings:EmailVerificationKey"] ?? "your-secret-key-for-email-verification";
            var encryptedToken = EncryptString(tokenData, key);

            // Codificar en base64 para URL
            return Convert.ToBase64String(Encoding.UTF8.GetBytes(encryptedToken));
        }

        private (bool isValid, int userId, string email) ValidateVerificationToken(string token)
        {
            try
            {
                // Decodificar el token
                var encryptedToken = Encoding.UTF8.GetString(Convert.FromBase64String(token));

                // Desencriptar el token
                var key = _configuration["AppSettings:EmailVerificationKey"] ?? "your-secret-key-for-email-verification";
                var tokenData = DecryptString(encryptedToken, key);

                // Separar los componentes del token
                var parts = tokenData.Split(':');
                if (parts.Length != 3)
                {
                    return (false, 0, string.Empty);
                }

                var userId = int.Parse(parts[0]);
                var email = parts[1];
                var expirationTicks = long.Parse(parts[2]);

                // Verificar si el token ha expirado
                if (DateTime.UtcNow.Ticks > expirationTicks)
                {
                    return (false, 0, string.Empty);
                }

                return (true, userId, email);
            }
            catch
            {
                return (false, 0, string.Empty);
            }
        }


        //CODIGO NUEVO
        [HttpGet("GetUserStatus")]
        public async Task<IActionResult> GetUserStatus([FromQuery] int userId)
        {
            try
            {
                var usuario = await _service.ObtenerUsuarioPorIdAsync(userId);
                if (usuario == null)
                {
                    return NotFound(new { success = false, message = "Usuario no encontrado." });
                }

                return Ok(new
                {
                    success = true,
                    isEmailVerified = usuario.IsEmailVerified
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al obtener estado del usuario: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Error interno del servidor." });
            }
        }


        private string EncryptString(string text, string key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                aes.IV = new byte[16]; // IV simple para este ejemplo

                var encryptor = aes.CreateEncryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream())
                {
                    using (var cs = new CryptoStream(ms, encryptor, CryptoStreamMode.Write))
                    {
                        using (var sw = new StreamWriter(cs))
                        {
                            sw.Write(text);
                        }
                    }

                    return Convert.ToBase64String(ms.ToArray());
                }
            }
        }

        private string DecryptString(string cipherText, string key)
        {
            using (var aes = Aes.Create())
            {
                aes.Key = Encoding.UTF8.GetBytes(key.PadRight(32).Substring(0, 32));
                aes.IV = new byte[16]; // IV simple para este ejemplo

                var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);

                using (var ms = new MemoryStream(Convert.FromBase64String(cipherText)))
                {
                    using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        using (var sr = new StreamReader(cs))
                        {
                            return sr.ReadToEnd();
                        }
                    }
                }
            }
        }

        private async Task SendWelcomeEmail(string email, string nombre)
        {
            try
            {
                var subject = "¡Bienvenido a EasyBooking!";
                var body = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #4a90e2; padding: 20px; text-align: center; color: white; }}
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
                        <p>Hola {nombre},</p>
                        <p>¡Gracias por registrarte en EasyBooking! Estamos emocionados de tenerte como parte de nuestra comunidad.</p>
                        <p>Con EasyBooking podrás:</p>
                        <ul>
                            <li>Reservar fácilmente en tus lugares favoritos</li>
                            <li>Gestionar todas tus reservas en un solo lugar</li>
                            <li>Recibir notificaciones y recordatorios de tus próximas reservas</li>
                        </ul>
                        <p>Para comenzar a utilizar todas las funcionalidades, te recomendamos verificar tu correo electrónico.</p>
                        <p>Si tienes alguna pregunta o necesitas ayuda, no dudes en contactarnos.</p>
                        <p>¡Esperamos que disfrutes de EasyBooking!</p>
                        <p>Saludos,<br>El equipo de EasyBooking</p>
                    </div>
                    <div class='footer'>
                        <p>© 2025 EasyBooking. Todos los derechos reservados.</p>
                    </div>
                </div>
            </body>
            </html>
        ";

                await _emailService.SendEmailAsync(email, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar correo de bienvenida: {ex.Message}");
                // No lanzamos la excepción para que no afecte al flujo principal
            }
        }


        // PUT: api/ApiUsuario/UpdateProfile
        [HttpPut("UpdateProfile")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto updateDto)
        {
            try
            {
                if (updateDto == null || updateDto.Id <= 0)
                {
                    return BadRequest(new { success = false, message = "Datos de usuario inválidos." });
                }

                // Obtener el usuario actual
                var usuario = await _service.ObtenerUsuarioPorIdAsync(updateDto.Id);
                if (usuario == null)
                {
                    return NotFound(new { success = false, message = "Usuario no encontrado." });
                }

                // Actualizar los datos del usuario
                usuario.Nombre = updateDto.Nombre ?? usuario.Nombre;
                usuario.Apellido = updateDto.Apellido ?? usuario.Apellido;
                usuario.Username = updateDto.Username ?? usuario.Username;

                // Verificar si se está actualizando la contraseña
                bool passwordChanged = false;
                if (!string.IsNullOrEmpty(updateDto.Password))
                {
                    // Verificar la contraseña actual
                    if (string.IsNullOrEmpty(updateDto.CurrentPassword))
                    {
                        return BadRequest(new { success = false, message = "Se requiere la contraseña actual para cambiar la contraseña." });
                    }

                    // Verificar que la contraseña actual sea correcta
                    var passwordHasher = new PasswordHasher<Usuario>();
                    var verificationResult = passwordHasher.VerifyHashedPassword(usuario, usuario.Password, updateDto.CurrentPassword);

                    if (verificationResult == PasswordVerificationResult.Failed)
                    {
                        return BadRequest(new { success = false, message = "La contraseña actual es incorrecta." });
                    }

                    // Actualizar la contraseña
                    usuario.Password = passwordHasher.HashPassword(usuario, updateDto.Password);
                    passwordChanged = true;
                }

                // Guardar los cambios
                await _service.ActualizarUsuarioAsync(usuario);

                // Si se cambió la contraseña, enviar correo de confirmación
                if (passwordChanged)
                {
                    await SendPasswordChangeEmail(usuario.Email, usuario.Nombre);
                }

                return Ok(new { success = true, message = "Perfil actualizado correctamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al actualizar perfil: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Error interno del servidor al actualizar el perfil." });
            }
        }

        // Método privado para enviar correo de confirmación de cambio de contraseña
        private async Task SendPasswordChangeEmail(string email, string nombre)
        {
            try
            {
                var subject = "Confirmación de cambio de contraseña - EasyBooking";
                var body = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
                    .container {{ max-width: 600px; margin: 0 auto; padding: 20px; }}
                    .header {{ background-color: #f8f9fa; padding: 20px; text-align: center; }}
                    .content {{ padding: 20px; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h1>Contraseña Restablecida</h1>
                    </div>
                    <div class='content'>
                        <p>Hola {nombre}.</p>
                        <p>Te informamos que la contraseña de tu cuenta en EasyBooking ha sido restablecida exitosamente.</p>
                        <p>Si no has realizado este cambio, por favor contacta inmediatamente con nuestro equipo de soporte.</p>
                        <p>Saludos,<br>El equipo de EasyBooking</p>
                    </div>
                </div>
            </body>
            </html>
        ";

                await _emailService.SendEmailAsync(email, subject, body);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar correo de confirmación de cambio de contraseña: {ex.Message}");
                // No lanzamos la excepción para que no afecte al flujo principal
            }
        }

        // DELETE: api/ApiUsuario/DeleteAccount
        [HttpDelete("DeleteAccount")]
        public async Task<IActionResult> DeleteAccount([FromQuery] int userId)
        {
            try
            {
                // Obtener el usuario
                var usuario = await _service.ObtenerUsuarioPorIdAsync(userId);
                if (usuario == null)
                {
                    return NotFound(new { success = false, message = "Usuario no encontrado." });
                }

                // Eliminar el usuario
                await _service.EliminarUsuarioAsync(userId);

                return Ok(new { success = true, message = "Usuario eliminado correctamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al eliminar usuario: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Error interno del servidor al eliminar el usuario." });
            }
        }

        // Agregar este nuevo endpoint después del método DeleteAccount
        // POST: api/ApiUsuario/VerifyPassword
        [HttpPost("VerifyPassword")]
        public async Task<IActionResult> VerifyPassword([FromBody] PasswordVerificationDto verificationDto)
        {
            try
            {
                if (verificationDto == null || string.IsNullOrEmpty(verificationDto.Password) || verificationDto.UserId <= 0)
                {
                    return BadRequest(new { success = false, message = "Datos inválidos." });
                }

                // Obtener el usuario
                var usuario = await _service.ObtenerUsuarioPorIdAsync(verificationDto.UserId);
                if (usuario == null)
                {
                    return NotFound(new { success = false, message = "Usuario no encontrado." });
                }

                // Verificar la contraseña
                var passwordHasher = new PasswordHasher<Usuario>();
                var result = passwordHasher.VerifyHashedPassword(usuario, usuario.Password, verificationDto.Password);

                if (result == PasswordVerificationResult.Failed)
                {
                    return Ok(new { success = false, message = "Contraseña incorrecta." });
                }

                return Ok(new { success = true, message = "Contraseña verificada correctamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar contraseña: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Error interno del servidor al verificar la contraseña." });
            }
        }

        // POST: api/ApiUsuario/SendPasswordResetCode
        [HttpPost("SendPasswordResetCode")]
        public async Task<IActionResult> SendPasswordResetCode([FromBody] PasswordResetRequestDto request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email))
                {
                    return BadRequest(new { success = false, message = "El correo electrónico es requerido." });
                }

                // Verificar si el usuario existe
                var usuario = await _service.ObtenerPorEmailAsync(request.Email);
                if (usuario == null)
                {
                    // Por seguridad, no revelamos si el email existe o no
                    return Ok(new { success = true, message = "Si el correo existe en nuestra base de datos, recibirás un código de verificación." });
                }

                // Generar código de verificación (6 dígitos)
                var random = new Random();
                var verificationCode = random.Next(100000, 999999).ToString();

                // Guardar el código en la base de datos o en caché
                // Para este ejemplo, usaremos un enfoque simple con una propiedad temporal
                // En producción, deberías usar una tabla específica o un sistema de caché
                usuario.ResetCode = verificationCode;
                usuario.ResetCodeExpiry = DateTime.UtcNow.AddMinutes(15); // El código expira en 15 minutos
                await _service.ActualizarUsuarioAsync(usuario);

                // Enviar el código por correo electrónico
                await SendPasswordResetEmail(usuario.Email, usuario.Nombre, verificationCode);

                return Ok(new { success = true, message = "Código de verificación enviado. Por favor, revisa tu correo electrónico." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al enviar código de restablecimiento: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Error interno del servidor." });
            }
        }

        // POST: api/ApiUsuario/VerifyResetCode
        [HttpPost("VerifyResetCode")]
        public async Task<IActionResult> VerifyResetCode([FromBody] VerifyResetCodeDto request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Code))
                {
                    return BadRequest(new { success = false, message = "El correo electrónico y el código son requeridos." });
                }

                // Verificar si el usuario existe
                var usuario = await _service.ObtenerPorEmailAsync(request.Email);
                if (usuario == null)
                {
                    return NotFound(new { success = false, message = "Usuario no encontrado." });
                }

                // Verificar si el código es válido y no ha expirado
                if (usuario.ResetCode != request.Code)
                {
                    return Ok(new { success = false, message = "Código de verificación incorrecto." });
                }

                if (usuario.ResetCodeExpiry < DateTime.UtcNow)
                {
                    return Ok(new { success = false, message = "El código de verificación ha expirado. Por favor, solicita uno nuevo." });
                }

                return Ok(new { success = true, message = "Código verificado correctamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al verificar código: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Error interno del servidor." });
            }
        }

        // POST: api/ApiUsuario/ResetPassword
        [HttpPost("ResetPassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Email) || string.IsNullOrEmpty(request.Code) || string.IsNullOrEmpty(request.NewPassword))
                {
                    return BadRequest(new { success = false, message = "Todos los campos son requeridos." });
                }

                // Verificar si el usuario existe
                var usuario = await _service.ObtenerPorEmailAsync(request.Email);
                if (usuario == null)
                {
                    return NotFound(new { success = false, message = "Usuario no encontrado." });
                }

                // Verificar si el código es válido y no ha expirado
                if (usuario.ResetCode != request.Code)
                {
                    return Ok(new { success = false, message = "Código de verificación incorrecto." });
                }

                if (usuario.ResetCodeExpiry < DateTime.UtcNow)
                {
                    return Ok(new { success = false, message = "El código de verificación ha expirado. Por favor, solicita uno nuevo." });
                }

                // Actualizar la contraseña
                var passwordHasher = new PasswordHasher<Usuario>();
                usuario.Password = passwordHasher.HashPassword(usuario, request.NewPassword);

                // Limpiar el código de restablecimiento
                usuario.ResetCode = null;
                usuario.ResetCodeExpiry = null;

                await _service.ActualizarUsuarioAsync(usuario);

                // Enviar correo de confirmación
                await SendPasswordChangedEmail(usuario.Email, usuario.Nombre);

                return Ok(new { success = true, message = "Contraseña restablecida correctamente." });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al restablecer contraseña: {ex.Message}");
                return StatusCode(500, new { success = false, message = "Error interno del servidor." });
            }
        }

        // Método para enviar el correo con el código de restablecimiento
        private async Task SendPasswordResetEmail(string email, string nombre, string code)
        {
            var subject = "Código de restablecimiento de contraseña - EasyBooking";
            var body = $@"
        <html>
        <head>
            <style>
                body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }}
                .header {{ background-color: #007bff; padding: 20px; text-align: center; color: white; border-radius: 8px 8px 0 0; }}
                .content {{ padding: 30px; background-color: #fff; }}
                .code {{ font-size: 32px; font-weight: bold; text-align: center; margin: 20px 0; letter-spacing: 5px; color: #007bff; }}
                .footer {{ text-align: center; padding: 15px; font-size: 14px; color: #666; }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>Restablecimiento de Contraseña</h1>
                </div>
                <div class='content'>
                    <p>Hola {nombre},</p>
                    <p>Has solicitado restablecer tu contraseña en EasyBooking. Utiliza el siguiente código de verificación:</p>
                    <div class='code'>{code}</div>
                    <p>Este código expirará en 15 minutos por razones de seguridad.</p>
                    <p>Si no has solicitado este cambio, puedes ignorar este correo y tu contraseña seguirá siendo la misma.</p>
                    <p>Saludos,<br>El equipo de EasyBooking</p>
                </div>
                <div class='footer'>
                    <p>© 2025 EasyBooking - Todos los derechos reservados</p>
                </div>
            </div>
        </body>
        </html>
    ";

            await _emailService.SendEmailAsync(email, subject, body);
        }

        // Método para enviar el correo de confirmación de cambio de contraseña
        private async Task SendPasswordChangedEmail(string email, string nombre)
        {
            var subject = "Contraseña restablecida exitosamente - EasyBooking";
            var body = $@"
        <html>
        <head>
            <style>
                body {{ font-family: Arial, sans-serif; line-height: 1.6; color: #333; }}
                .container {{ max-width: 600px; margin: 0 auto; padding: 20px; border-radius: 8px; box-shadow: 0 0 10px rgba(0,0,0,0.1); }}
                .header {{ background-color: #28a745; padding: 20px; text-align: center; color: white; border-radius: 8px 8px 0 0; }}
                .content {{ padding: 30px; background-color: #fff; }}
                .footer {{ text-align: center; padding: 15px; font-size: 14px; color: #666; }}
            </style>
        </head>
        <body>
            <div class='container'>
                <div class='header'>
                    <h1>Contraseña Restablecida</h1>
                </div>
                <div class='content'>
                    <p>Hola {nombre},</p>
                    <p>Tu contraseña ha sido restablecida exitosamente.</p>
                    <p>Ya puedes iniciar sesión en EasyBooking con tu nueva contraseña.</p>
                    <p>Si no has realizado este cambio, por favor contacta inmediatamente con nuestro equipo de soporte.</p>
                    <p>Saludos,<br>El equipo de EasyBooking</p>
                </div>
                <div class='footer'>
                    <p>© 2025 EasyBooking - Todos los derechos reservados</p>
                </div>
            </div>
        </body>
        </html>
    ";

            await _emailService.SendEmailAsync(email, subject, body);
        }

    }

}

