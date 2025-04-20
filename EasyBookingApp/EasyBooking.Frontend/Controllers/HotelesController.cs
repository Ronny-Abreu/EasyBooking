using EasyBooking.Frontend.Models;
using EasyBooking.Frontend.Services;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace EasyBooking.Frontend.Controllers
{
    public class HotelesController : Controller
    {
        private readonly HttpClientService _httpClientService;
        private readonly ILogger<HotelesController> _logger;

        public HotelesController(HttpClientService httpClientService, ILogger<HotelesController> logger)
        {
            _httpClientService = httpClientService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? ciudad = null, decimal? precioMinimo = null, decimal? precioMaximo = null, int? calificacion = null)
        {
            try
            {
                string endpoint = "hoteles";
                if (ciudad != null || precioMinimo != null || precioMaximo != null || calificacion != null)
                {
                    endpoint = $"hoteles/buscar?ciudad={ciudad}&precioMinimo={precioMinimo}&precioMaximo={precioMaximo}&calificacion={calificacion}";
                }

                var response = await _httpClientService.GetAsync<List<HotelViewModel>>(endpoint);

                if (response.Success)
                {
                    var model = new BusquedaHotelesViewModel
                    {
                        Hoteles = response.Data ?? new List<HotelViewModel>(),
                        Ciudad = ciudad,
                        PrecioMinimo = precioMinimo,
                        PrecioMaximo = precioMaximo,
                        Calificacion = calificacion,
                        TotalHoteles = response.Data?.Count ?? 0
                    };

                    return View(model);
                }
                else
                {
                    _logger.LogError("Error al obtener hoteles: {Error}", response.Error);
                    TempData["ErrorMessage"] = "Error al cargar los hoteles. Intente nuevamente más tarde.";
                    return View(new BusquedaHotelesViewModel());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener hoteles");
                TempData["ErrorMessage"] = "Error al cargar los hoteles. Intente nuevamente más tarde.";
                return View(new BusquedaHotelesViewModel());
            }
        }

        public async Task<IActionResult> Detalle(int id)
        {
            try
            {
                var response = await _httpClientService.GetAsync<HotelViewModel>($"hoteles/{id}");

                if (response.Success && response.Data != null)
                {
                    return View(response.Data);
                }
                else
                {
                    TempData["ErrorMessage"] = "Hotel no encontrado";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalle del hotel {Id}", id);
                TempData["ErrorMessage"] = "Error al cargar el hotel. Intente nuevamente más tarde.";
                return RedirectToAction("Index");
            }
        }
    }
}