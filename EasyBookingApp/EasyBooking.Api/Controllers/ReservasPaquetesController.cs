using EasyBooking.Application.Contracts;
using EasyBooking.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EasyBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasPaquetesController : ControllerBase
    {
        private readonly IReservaPaqueteService _reservaPaqueteService;

        public ReservasPaquetesController(IReservaPaqueteService reservaPaqueteService)
        {
            _reservaPaqueteService = reservaPaqueteService;
        }

        [HttpPost]
        public async Task<IActionResult> CrearReserva([FromBody] CrearReservaPaqueteDto reservaDto, [FromQuery] int usuarioId)
        {
            var reserva = await _reservaPaqueteService.CrearReservaPaqueteAsync(reservaDto, usuarioId);
            if (reserva == null)
            {
                return BadRequest(new { Message = "No se pudo crear la reserva" });
            }

            return Ok(new { Data = reserva });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reserva = await _reservaPaqueteService.GetReservaPaqueteByIdAsync(id);
            if (reserva == null)
            {
                return NotFound(new { Message = "Reserva no encontrada" });
            }

            return Ok(new { Data = reserva });
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuarioId(int usuarioId)
        {
            var reservas = await _reservaPaqueteService.GetReservasPaqueteByUsuarioIdAsync(usuarioId);
            return Ok(new { Data = reservas });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelarReserva(int id)
        {
            var resultado = await _reservaPaqueteService.CancelarReservaPaqueteAsync(id);
            if (!resultado)
            {
                return BadRequest(new { Message = "No se pudo cancelar la reserva" });
            }

            return Ok(new { Message = "Reserva cancelada con éxito" });
        }

        [HttpPost("pago")]
        public async Task<IActionResult> ProcesarPago([FromBody] PagoDto pagoDto)
        {
            var reserva = await _reservaPaqueteService.ProcesarPagoAsync(pagoDto);
            if (reserva == null)
            {
                return BadRequest(new { Message = "No se pudo procesar el pago" });
            }

            return Ok(new { Data = reserva });
        }
    }
}
