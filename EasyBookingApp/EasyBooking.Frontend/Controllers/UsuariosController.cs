using EasyBooking.Frontend.Models;
using EasyBooking.Frontend.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
using Microsoft.AspNetCore.Authorization;

namespace EasyBooking.Frontend.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly HttpClientService _httpClientService;
        private readonly ILogger<UsuariosController> _logger;
        private readonly UsuarioClientService _usuarioClientService;


        public UsuariosController(HttpClientService httpClientService,
                          ILogger<UsuariosController> logger,
                          UsuarioClientService usuarioClientService)
        {
            _httpClientService = httpClientService;
            _logger = logger;
            _usuarioClientService = usuarioClientService;
        }
        [HttpGet]
        public IActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Registro(RegistroUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var response = await _httpClientService.PostAsync<dynamic>("usuarios/registro", model);

                if (response.Success)
                {
                    TempData["SuccessMessage"] = "Usuario registrado con éxito. Por favor, inicie sesión.";
                    return RedirectToAction("Login");
                }
                else
                {
                    ModelState.AddModelError("", response.Error ?? "Error al registrar el usuario");
                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al registrar usuario");
                ModelState.AddModelError("", "Error al procesar la solicitud. Intente nuevamente más tarde.");
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var response = await _httpClientService.PostAsync<dynamic>("usuarios/login", model);

                if (response.Success)
                {
                    var jsonString = JsonSerializer.Serialize(response.Data);
                    var usuario = JsonSerializer.Deserialize<UsuarioViewModel>(jsonString, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (usuario != null)
                    {
                        // Crear claims para la autenticación
                        var claims = new List<Claim>
                        {
                            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                            new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),
                            new Claim(ClaimTypes.Email, usuario.Email)
                        };

                        var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                        var authProperties = new AuthenticationProperties
                        {
                            IsPersistent = model.RecordarMe
                        };

                        await HttpContext.SignInAsync(
                            CookieAuthenticationDefaults.AuthenticationScheme,
                            new ClaimsPrincipal(claimsIdentity),
                            authProperties);

                        return RedirectToAction("Index", "Home");
                    }
                }

                ModelState.AddModelError("", "Credenciales inválidas");
                return View(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al iniciar sesión");
                ModelState.AddModelError("", "Error al procesar la solicitud. Intente nuevamente más tarde.");
                return View(model);
            }
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> EliminarCuenta()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized();
                }

                var response = await _usuarioClientService.EliminarUsuarioAsync(userId);

                if (response.Success)
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    TempData["SuccessMessage"] = "Cuenta eliminada correctamente.";
                    return RedirectToAction("Index", "Home");
                }

                TempData["ErrorMessage"] = response.Error ?? "No se pudo eliminar la cuenta.";
                return RedirectToAction("PerfilUser");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al eliminar cuenta");
                TempData["ErrorMessage"] = "Hubo un error inesperado al eliminar la cuenta.";
                return RedirectToAction("PerfilUser");
            }
        }

        [Authorize]
        [HttpPut]
        public async Task<IActionResult> ActualizarUsuario([FromBody] ActualizarUsuarioViewModel model)
        {
            var response = await _usuarioClientService.ActualizarUsuarioAsync(model);
            return Json(response);
        }


        [HttpGet]
        public IActionResult CuentaEliminada()
        {
            return View();
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> VerificarContrasena([FromBody] VerificarContrasenaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Datos inválidos" });
            }

            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Json(new { success = false, message = "Usuario no autenticado" });
                }

                var dto = new
                {
                    UsuarioId = userId,
                    Password = model.Password
                };

                var response = await _httpClientService.PostAsync<dynamic>("usuarios/verificar-contrasena", dto);

                if (response.Success)
                {
                    return Json(new { success = true });
                }
                else
                {
                    return Json(new { success = false, message = response.Error ?? "Contraseña incorrecta" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al verificar contraseña");
                return Json(new { success = false, message = "Error al procesar la solicitud" });
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SolicitarEliminacion()
        {
            try
            {
                // Call the API endpoint
                var response = await _httpClientService.PostAsync<dynamic>("usuarios/solicitar-eliminacion", null);

                if (response.Success)
                {
                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                    return Json(new { success = true, message = response.Message ?? "Se ha enviado un correo de confirmación para eliminar tu cuenta" });
                }
                else
                {
                    return Json(new { success = false, message = response.Error ?? "Error al solicitar la eliminación de la cuenta" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al solicitar eliminación de cuenta");
                return Json(new { success = false, message = "Error al procesar la solicitud" });
            }
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> PerfilUser()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized();
            }

            var response = await _usuarioClientService.ObtenerUsuarioPorIdAsync(userId);

            if (!response.Success || response.Data == null)
            {
                TempData["ErrorMessage"] = "No se pudo cargar el perfil del usuario.";
                return RedirectToAction("Index", "Home");
            }

            return View(response.Data);
        }


    }
}