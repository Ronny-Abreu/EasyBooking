using EasyBooking.Application.Contracts;
using Microsoft.AspNetCore.Mvc;

namespace EasyBooking.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HotelesController : ControllerBase
    {
        private readonly IHotelService _hotelService;

        public HotelesController(IHotelService hotelService)
        {
            _hotelService = hotelService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var hoteles = await _hotelService.GetAllHotelesAsync();
            return Ok(new { Data = hoteles });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var hotel = await _hotelService.GetHotelByIdAsync(id);
            if (hotel == null)
            {
                return NotFound(new { Message = "Hotel no encontrado" });
            }

            return Ok(new { Data = hotel });
        }


        [HttpGet("buscar")]
        public async Task<IActionResult> Buscar([FromQuery] string? ciudad = null, [FromQuery] decimal? precioMinimo = null, [FromQuery] decimal? precioMaximo = null, [FromQuery] int? calificacion = null)
        {
            var hoteles = await _hotelService.BuscarHotelesAsync(ciudad, precioMinimo, precioMaximo, calificacion);
            return Ok(new { Data = hoteles });
        }
    }
}