using EasyBooking.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EasyBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaquetesTuristicosController : ControllerBase
    {
        private readonly IPaqueteTuristicoService _paqueteService;

        public PaquetesTuristicosController(IPaqueteTuristicoService paqueteService)
        {
            _paqueteService = paqueteService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var paquetes = await _paqueteService.GetAllPaquetesAsync();
            return Ok(new { Data = paquetes });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var paquete = await _paqueteService.GetPaqueteByIdAsync(id);
            if (paquete == null)
            {
                return NotFound(new { Message = "Paquete turístico no encontrado" });
            }

            return Ok(new { Data = paquete });
        }

        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar(
            [FromQuery] string? destino = null,
            [FromQuery] decimal? precioMinimo = null,
            [FromQuery] decimal? precioMaximo = null,
            [FromQuery] int? calificacion = null,
            [FromQuery] int? duracionMinima = null,
            [FromQuery] int? duracionMaxima = null)
        {
            var paquetes = await _paqueteService.BuscarPaquetesAsync(destino, precioMinimo, precioMaximo, calificacion, duracionMinima, duracionMaxima);
            return Ok(new { Data = paquetes });
        }
    }
}
