using EasyBooking.Frontend.Models;
using EasyBooking.Frontend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Text.Json;
namespace EasyBooking.Frontend.Controllers
{
    [Authorize]
    public class ReservasController : Controller
    {
        private readonly HttpClientService _httpClientService;
        private readonly ILogger<ReservasController> _logger;

        public ReservasController(HttpClientService httpClientService, ILogger<ReservasController> logger)
        {
            _httpClientService = httpClientService;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _httpClientService.GetAsync<List<ReservaViewModel>>($"reservas/usuario/{usuarioId}");

                if (response.Success)
                {
                    // Filtrar reservas activas (que no estén canceladas)
                    var reservasActivas = response.Data?
                        .Where(r => r.Estado != null && !r.Estado.Equals("Cancelada", StringComparison.OrdinalIgnoreCase))
                        .ToList();

                    return View(reservasActivas ?? new List<ReservaViewModel>());
                }
                else
                {
                    _logger.LogError("Error al obtener reservas: {Error}", response.Error);
                    TempData["ErrorMessage"] = "Error al cargar las reservas. Intente nuevamente más tarde.";
                    return View(new List<ReservaViewModel>());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener reservas");
                TempData["ErrorMessage"] = "Error al cargar las reservas. Intente nuevamente más tarde.";
                return View(new List<ReservaViewModel>());
            }
        }


        [HttpGet]
        public async Task<IActionResult> Crear(int hotelId)
        {
            try
            {
                var response = await _httpClientService.GetAsync<HotelViewModel>($"hoteles/{hotelId}");

                if (response.Success && response.Data != null)
                {
                    var model = new CrearReservaViewModel
                    {
                        HotelId = hotelId,
                        Hotel = response.Data,
                        FechaEntrada = DateTime.Now.AddDays(1),
                        FechaSalida = DateTime.Now.AddDays(3),
                        NumeroHuespedes = 2
                    };

                    return View(model);
                }
                else
                {
                    TempData["ErrorMessage"] = "Hotel no encontrado";
                    return RedirectToAction("Index", "Hoteles");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al preparar la creación de reserva para el hotel {HotelId}", hotelId);
                TempData["ErrorMessage"] = "Error al cargar el hotel. Intente nuevamente más tarde.";
                return RedirectToAction("Index", "Hoteles");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Crear(CrearReservaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    var hotelResponse = await _httpClientService.GetAsync<HotelViewModel>($"hoteles/{model.HotelId}");
                    if (hotelResponse.Success && hotelResponse.Data != null)
                    {
                        model.Hotel = hotelResponse.Data;
                    }
                }
                catch { }

                return View(model);
            }

            try
            {
                var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var response = await _httpClientService.PostAsync<ReservaViewModel>($"reservas?usuarioId={usuarioId}", model);

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
                        var hotelResponse = await _httpClientService.GetAsync<HotelViewModel>($"hoteles/{model.HotelId}");
                        if (hotelResponse.Success && hotelResponse.Data != null)
                        {
                            model.Hotel = hotelResponse.Data;
                        }
                    }
                    catch { }

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al crear reserva");
                ModelState.AddModelError("", "Error al procesar la solicitud. Intente nuevamente más tarde.");

                try
                {
                    var hotelResponse = await _httpClientService.GetAsync<HotelViewModel>($"hoteles/{model.HotelId}");
                    if (hotelResponse.Success && hotelResponse.Data != null)
                    {
                        model.Hotel = hotelResponse.Data;
                    }
                }
                catch { }

                return View(model);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Editar(int id)
        {
            try
            {
                var response = await _httpClientService.GetAsync<ReservaViewModel>($"reservas/{id}");

                if (response.Success && response.Data != null)
                {
                    var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (response.Data.UsuarioId.ToString() != usuarioId)
                    {
                        TempData["ErrorMessage"] = "No tiene permiso para editar esta reserva";
                        return RedirectToAction("Index");
                    }

                    var model = new CrearReservaViewModel
                    {
                        HotelId = response.Data.HotelId,
                        FechaEntrada = response.Data.FechaEntrada,
                        FechaSalida = response.Data.FechaSalida,
                        NumeroHuespedes = response.Data.NumeroHuespedes
                    };

                    try
                    {
                        var hotelResponse = await _httpClientService.GetAsync<HotelViewModel>($"hoteles/{model.HotelId}");
                        if (hotelResponse.Success && hotelResponse.Data != null)
                        {
                            model.Hotel = hotelResponse.Data;
                        }
                    }
                    catch { }

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
                _logger.LogError(ex, "Error al obtener reserva para editar {Id}", id);
                TempData["ErrorMessage"] = "Error al cargar la reserva. Intente nuevamente más tarde.";
                return RedirectToAction("Index");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Editar(int id, CrearReservaViewModel model)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    var hotelResponse = await _httpClientService.GetAsync<HotelViewModel>($"hoteles/{model.HotelId}");
                    if (hotelResponse.Success && hotelResponse.Data != null)
                    {
                        model.Hotel = hotelResponse.Data;
                    }
                }
                catch { }

                return View(model);
            }

            try
            {
                var response = await _httpClientService.PutAsync<ReservaViewModel>($"reservas/{id}", model);

                if (response.Success)
                {
                    TempData["SuccessMessage"] = "Reserva actualizada con éxito";
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", response.Error ?? "Error al actualizar la reserva");

                    try
                    {
                        var hotelResponse = await _httpClientService.GetAsync<HotelViewModel>($"hoteles/{model.HotelId}");
                        if (hotelResponse.Success && hotelResponse.Data != null)
                        {
                            model.Hotel = hotelResponse.Data;
                        }
                    }
                    catch { }

                    return View(model);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al actualizar reserva {Id}", id);
                ModelState.AddModelError("", "Error al procesar la solicitud. Intente nuevamente más tarde.");

                try
                {
                    var hotelResponse = await _httpClientService.GetAsync<HotelViewModel>($"hoteles/{model.HotelId}");
                    if (hotelResponse.Success && hotelResponse.Data != null)
                    {
                        model.Hotel = hotelResponse.Data;
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
                var response = await _httpClientService.DeleteAsync<dynamic>($"reservas/{id}");

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
                _logger.LogError(ex, "Error al cancelar reserva {Id}", id);
                return StatusCode(500, new { success = false, message = "Error inesperado al cancelar la reserva" });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Pago(int id)
        {
            try
            {
                var response = await _httpClientService.GetAsync<ReservaViewModel>($"reservas/{id}");

                if (response.Success && response.Data != null)
                {
                    var usuarioId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    if (response.Data.UsuarioId.ToString() != usuarioId)
                    {
                        TempData["ErrorMessage"] = "No tiene permiso para pagar esta reserva";
                        return RedirectToAction("Index");
                    }

                    var model = new PagoViewModel
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
        public async Task<IActionResult> Pago(PagoViewModel model)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    var reservaResponse = await _httpClientService.GetAsync<ReservaViewModel>($"reservas/{model.ReservaId}");
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
                var response = await _httpClientService.PostAsync<ReservaViewModel>("reservas/pago", model);

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
                        var reservaResponse = await _httpClientService.GetAsync<ReservaViewModel>($"reservas/{model.ReservaId}");
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
                    var reservaResponse = await _httpClientService.GetAsync<ReservaViewModel>($"reservas/{model.ReservaId}");
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