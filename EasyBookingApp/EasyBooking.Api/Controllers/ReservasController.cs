using EasyBooking.Application.Contracts;
using EasyBooking.Application.Dtos;
using Microsoft.AspNetCore.Mvc;

namespace EasyBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservasController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearReservaDto reservaDto, [FromQuery] int usuarioId)
        {
            var resultado = await _reservaService.CrearReservaAsync(reservaDto, usuarioId);
            if (resultado == null)
            {
                return BadRequest(new { Message = "No se pudo crear la reserva. No está disponible para esta fecha." });
            }

            return Ok(new { Message = "Reserva creada con éxito", Data = resultado });

        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var reserva = await _reservaService.GetReservaByIdAsync(id);
            if (reserva == null)
            {
                return NotFound(new { Message = "Reserva no encontrada" });
            }

            return Ok(new { Data = reserva });
        }

        [HttpGet("usuario/{usuarioId}")]
        public async Task<IActionResult> GetByUsuarioId(int usuarioId)
        {
            var reservas = await _reservaService.GetReservasByUsuarioIdAsync(usuarioId);
            return Ok(new { Data = reservas });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Actualizar(int id, [FromBody] CrearReservaDto reservaDto)
        {
            var resultado = await _reservaService.ActualizarReservaAsync(id, reservaDto);
            if (resultado == null)
            {
                return BadRequest(new { Message = "No se pudo actualizar la reserva. Verifique los datos e intente nuevamente." });
            }

            return Ok(new { Message = "Reserva actualizada con éxito", Data = resultado });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Cancelar(int id)
        {
            var resultado = await _reservaService.CancelarReservaAsync(id);
            if (!resultado)
            {
                return BadRequest(new { Message = "No se pudo cancelar la reserva. Verifique los datos e intente nuevamente." });
            }

            return Ok(new { Message = "Reserva cancelada con éxito" });
        }

        [HttpPost("pago")]
        public async Task<IActionResult> ProcesarPago([FromBody] PagoDto pagoDto)
        {
            var resultado = await _reservaService.ProcesarPagoAsync(pagoDto);
            if (resultado == null)
            {
                return BadRequest(new { Message = "No se pudo procesar el pago. Verifique los datos e intente nuevamente." });
            }

            return Ok(new { Message = "Pago procesado con éxito", Data = resultado });
        }


    }
}