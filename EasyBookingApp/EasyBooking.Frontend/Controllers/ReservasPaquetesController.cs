using EasyBooking.Frontend.Models;
using EasyBooking.Frontend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EasyBooking.Frontend.Controllers
{
    [Authorize]
    public class ReservasPaquetesController : Controller
    {
        private readonly HttpClientService _httpClientService;
        private readonly ILogger<ReservasPaquetesController> _logger;

        public ReservasPaquetesController(HttpClientService httpClientService, ILogger<ReservasPaquetesController> logger)
        {
            _httpClientService = httpClientService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _httpClientService.GetAsync<List<ReservaPaqueteViewModel>>($"reservaspaquetes/usuario/{usuarioId}");

                if (response.Success)
                {
                    // Filtrar reservas activas (que no estén canceladas)
                    var reservasActivas = response.Data?
                        .Where(r => r.Estado != null && !r.Estado.Equals("Cancelada", StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    return View(reservasActivas ?? new List<ReservaPaqueteViewModel>());
                }
                else
                {
                    _logger.LogError("Error al obtener reservas de paquetes: {Error}", response.Error);
                    TempData["ErrorMessage"] = "Error al cargar las reservas. Intente nuevamente más tarde.";
                    return View(new List<ReservaPaqueteViewModel>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reservas de paquetes");
                TempData["ErrorMessage"] = "Error al cargar las reservas. Intente nuevamente más tarde.";
                return View(new List<ReservaPaqueteViewModel>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Crear(int paqueteId)
        {
            try
            {
                var response = await _httpClientService.GetAsync<PaqueteTuristicoViewModel>($"paquetesturisticos/{paqueteId}");

                if (response.Success && response.Data != null)
                {
                    var model = new CrearReservaPaqueteViewModel
                    {
                        PaqueteId = paqueteId,
                        Paquete = response.Data,
                        FechaInicio = DateTime.Now.AddDays(7),
                        NumeroPersonas = 2
                    };

                    return View(model);
                }
                else
                {
                    TempData["ErrorMessage"] = "Paquete turístico no encontrado";
                    return RedirectToAction("Index", "PaquetesTuristicos");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al preparar la creación de reserva para el paquete {PaqueteId}", paqueteId);
                TempData["ErrorMessage"] = "Error al cargar el paquete. Intente nuevamente más tarde.";
                return RedirectToAction("Index", "PaquetesTuristicos");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearReservaPaqueteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    var paqueteResponse = await _httpClientService.GetAsync<PaqueteTuristicoViewModel>($"paquetesturisticos/{model.PaqueteId}");
                    if (paqueteResponse.Success && paqueteResponse.Data != null)
                    {
                        model.Paquete = paqueteResponse.Data;
                    }
                }
                catch { }

                return View(model);
            }

            try
            {
                var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _httpClientService.PostAsync<ReservaPaqueteViewModel>($"reservaspaquetes?usuarioId={usuarioId}", model);

                if (response.Success)
                {
                    TempData["SuccessMessage"] = "Reserva creada con éxito";
                    return RedirectToAction("Pago", new { id = response.Data?.Id });
                }
                else
                {
                    ModelState.AddModelError("", response.Error ?? "Error al crear la reserva");

                    try
                    {
                        var paqueteResponse = await _httpClientService.GetAsync<PaqueteTuristicoViewModel>($"paquetesturisticos/{model.PaqueteId}");
                        if (paqueteResponse.Success && paqueteResponse.Data != null)
                        {
                            model.Paquete = paqueteResponse.Data;
                        }
                    }
                    catch { }

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear reserva de paquete");
                ModelState.AddModelError("", "Error al procesar la solicitud. Intente nuevamente más tarde.");

                try
                {
                    var paqueteResponse = await _httpClientService.GetAsync<PaqueteTuristicoViewModel>($"paquetesturisticos/{model.PaqueteId}");
                    if (paqueteResponse.Success && paqueteResponse.Data != null)
                    {
                        model.Paquete = paqueteResponse.Data;
                    }
                }
                catch { }

                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> Cancelar(int id)
        {
            try
            {
                var response = await _httpClientService.DeleteAsync<dynamic>($"reservaspaquetes/{id}");

                if (response.Success)
                {
                    return Json(new { success = true, message = "Reserva cancelada con éxito" });
                }
                else
                {
                    return BadRequest(new { success = false, message = response.Error ?? "Error al cancelar la reserva" });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al cancelar reserva de paquete {Id}", id);
                return StatusCode(500, new { success = false, message = "Error inesperado al cancelar la reserva" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Pago(int id)
        {
            try
            {
                var response = await _httpClientService.GetAsync<ReservaPaqueteViewModel>($"reservaspaquetes/{id}");

                if (response.Success && response.Data != null)
                {
                    var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (response.Data.UsuarioId.ToString() != usuarioId)
                    {
                        TempData["ErrorMessage"] = "No tiene permiso para pagar esta reserva";
                        return RedirectToAction("Index");
                    }

                    var model = new PagoPaqueteViewModel
                    {
                        ReservaId = id,
                        Reserva = response.Data,
                        NumeroTarjeta = "",
                        NombreTitular = "",
                        FechaExpiracion = "",
                        CVV = ""
                    };

                    return View(model);
                }
                else
                {
                    TempData["ErrorMessage"] = "Reserva no encontrada";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reserva para pago {Id}", id);
                TempData["ErrorMessage"] = "Error al cargar la reserva. Intente nuevamente más tarde.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Pago(PagoPaqueteViewModel model)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    var reservaResponse = await _httpClientService.GetAsync<ReservaPaqueteViewModel>($"reservaspaquetes/{model.ReservaId}");
                    if (reservaResponse.Success && reservaResponse.Data != null)
                    {
                        model.Reserva = reservaResponse.Data;
                    }
                }
                catch { }

                return View(model);
            }

            try
            {
                var response = await _httpClientService.PostAsync<ReservaPaqueteViewModel>("reservaspaquetes/pago", model);

                if (response.Success)
                {
                    TempData["SuccessMessage"] = "Pago procesado con éxito. Su reserva ha sido confirmada.";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", response.Error ?? "Error al procesar el pago");

                    try
                    {
                        var reservaResponse = await _httpClientService.GetAsync<ReservaPaqueteViewModel>($"reservaspaquetes/{model.ReservaId}");
                        if (reservaResponse.Success && reservaResponse.Data != null)
                        {
                            model.Reserva = reservaResponse.Data;
                        }
                    }
                    catch { }

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al procesar pago para reserva {Id}", model.ReservaId);
                ModelState.AddModelError("", "Error al procesar la solicitud. Intente nuevamente más tarde.");

                try
                {
                    var reservaResponse = await _httpClientService.GetAsync<ReservaPaqueteViewModel>($"reservaspaquetes/{model.ReservaId}");
                    if (reservaResponse.Success && reservaResponse.Data != null)
                    {
                        model.Reserva = reservaResponse.Data;
                    }
                }
                catch { }

                return View(model);
            }
        }
    }
}
