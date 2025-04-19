using EasyBooking.Frontend.Models;
using EasyBooking.Frontend.Services;
using Microsoft.AspNetCore.Mvc;

namespace EasyBooking.Frontend.Controllers
{
    public class PaquetesTuristicosController : Controller
    {
        private readonly HttpClientService _httpClientService;
        private readonly ILogger<PaquetesTuristicosController> _logger;

        public PaquetesTuristicosController(HttpClientService httpClientService, ILogger<PaquetesTuristicosController> logger)
        {
            _httpClientService = httpClientService;
            _logger = logger;
        }

        public async Task<IActionResult> Index(string? destino = null, decimal? precioMinimo = null, decimal? precioMaximo = null, int? calificacion = null, int? duracionMinima = null, int? duracionMaxima = null)
        {
            try
            {
                string endpoint = "paquetesturisticos";
                if (destino != null || precioMinimo != null || precioMaximo != null || calificacion != null || duracionMinima != null || duracionMaxima != null)
                {
                    endpoint = $"paquetesturisticos/buscar?destino={destino}&precioMinimo={precioMinimo}&precioMaximo={precioMaximo}&calificacion={calificacion}&duracionMinima={duracionMinima}&duracionMaxima={duracionMaxima}";
                }

                var response = await _httpClientService.GetAsync<List<PaqueteTuristicoViewModel>>(endpoint);

                if (response.Success)
                {
                    var model = new BusquedaPaquetesViewModel
                    {
                        Paquetes = response.Data ?? new List<PaqueteTuristicoViewModel>(),
                        Destino = destino,
                        PrecioMinimo = precioMinimo,
                        PrecioMaximo = precioMaximo,
                        Calificacion = calificacion,
                        DuracionMinima = duracionMinima,
                        DuracionMaxima = duracionMaxima
                    };

                    return View(model);
                }
                else
                {
                    _logger.LogError("Error al obtener paquetes turísticos: {Error}", response.Error);
                    TempData["ErrorMessage"] = "Error al cargar los paquetes turísticos. Intente nuevamente más tarde.";
                    return View(new BusquedaPaquetesViewModel());
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener paquetes turísticos");
                TempData["ErrorMessage"] = "Error al cargar los paquetes turísticos. Intente nuevamente más tarde.";
                return View(new BusquedaPaquetesViewModel());
            }
        }

        public async Task<IActionResult> Detalle(int id)
        {
            try
            {
                var response = await _httpClientService.GetAsync<PaqueteTuristicoViewModel>($"paquetesturisticos/{id}");

                if (response.Success && response.Data != null)
                {
                    //DEPURACIÓN
                    _logger.LogInformation("Paquete obtenido correctamente: {Id}, Nombre: {Nombre}",
                        response.Data.Id, response.Data.Nombre);

                    return View(response.Data);
                }
                else
                {
                    _logger.LogWarning("Paquete no encontrado: {Id}, Error: {Error}", id, response.Error);
                    TempData["ErrorMessage"] = "Paquete turístico no encontrado";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener detalle del paquete turístico {Id}", id);
                TempData["ErrorMessage"] = "Error al cargar el paquete turístico. Intente nuevamente más tarde.";
                return RedirectToAction("Index");
            }
        }
    }
}
