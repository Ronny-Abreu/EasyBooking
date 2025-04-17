using AutoMapper;
using EasyBooking.Application.Contracts;
using EasyBooking.Application.Dtos;
using EasyBooking.Persistence.Interfaces;

namespace EasyBooking.Application.Services
{
    public class HotelService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly IMapper _mapper;

        public HotelService(IHotelRepository hotelRepository, IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _mapper = mapper;
        }

        public async Task<List<HotelDto>> GetAllHotelesAsync()
        {
            var hoteles = await _hotelRepository.GetAllAsync();
            return _mapper.Map<List<HotelDto>>(hoteles);
        }

        public async Task<HotelDto?> GetHotelByIdAsync(int id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id);

            if (hotel != null)
            {
                var hotelDto = _mapper.Map<HotelDto>(hotel);
                hotelDto.Servicios = hotel.Servicios;

                return hotelDto;
            }
            return null;
        }

        public async Task<List<HotelDto>> BuscarHotelesAsync(string? ciudad = null, decimal? precioMinimo = null, decimal? precioMaximo = null, int? calificacion = null)
        {
            var hoteles = await _hotelRepository.BuscarHotelesAsync(ciudad, precioMinimo, precioMaximo, calificacion);
            return _mapper.Map<List<HotelDto>>(hoteles);
        }
    }
}
