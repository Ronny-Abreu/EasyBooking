using EasyBooking.Frontend.Models;
using EasyBooking.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyBooking.Frontend.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly UsuarioClientService _usuarioService;

        public UsuarioController(UsuarioClientService usuarioService)
        {
            _usuarioService = usuarioService;
        }

        // GET: Usuario/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }


        // POST: Usuario/Login
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _usuarioService.LoginAsync(model.Email, model.Password);

            if (response.Success)
            {

                return RedirectToAction("Index", "Inicio");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Credenciales inválidas");
                return View(model);
            }
        }


        // GET: Usuario/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(new UsuarioViewModel());
        }

        // POST: Usuario/Register
        [HttpPost]
        public async Task<IActionResult> Register(UsuarioViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var response = await _usuarioService.RegistrarUsuarioAsync(model);

            if (response.Success)
            {
                TempData["SuccessMessage"] = "Usuario registrado correctamente. Por favor, inicia sesión.";
                return RedirectToAction("Login");
            }
            else
            {
                ModelState.AddModelError(string.Empty, response.Error ?? "Error al registrar el usuario");
                return View(model);
            }
        }

        // GET: Usuario/PerfilUser
        [HttpGet]
        public async Task<IActionResult> PerfilUser()
        {
            Console.WriteLine("Acción PerfilUser ejecutada");

            ViewData["HideFooter"] = true;

            // Default to not verified
            ViewBag.IsEmailVerified = false;

            // Try to get userId from query string
            if (int.TryParse(Request.Query["userId"], out int userId) && userId > 0)
            {
                try
                {
                    
                    var apiUrl = $"https://localhost:7191/api/ApiUsuario/GetUserStatus?userId={userId}";

                    using (var httpClient = new HttpClient())
                    {
                        var response = await httpClient.GetAsync(apiUrl);
                        if (response.IsSuccessStatusCode)
                        {
                            var content = await response.Content.ReadAsStringAsync();
                            var result = System.Text.Json.JsonSerializer.Deserialize<UserStatusResponse>(content);

                            if (result != null && result.success)
                            {
                                ViewBag.IsEmailVerified = result.isEmailVerified;
                                Console.WriteLine($"Email verification status: {result.isEmailVerified}");
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error retrieving user data: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("No user ID provided, defaulting to not verified");
            }

            return View("PerfilUser");
        }

        public class UserStatusResponse
        {
            public bool success { get; set; }
            public bool isEmailVerified { get; set; }
        }


        // GET: /Usuario/EmailVerified
        public IActionResult EmailVerified()
        {
            return View();
        }

        // GET: Usuario/Logout
        public IActionResult Logout()
        {
            return RedirectToAction("Index", "Inicio");
        }







    
    }
}

