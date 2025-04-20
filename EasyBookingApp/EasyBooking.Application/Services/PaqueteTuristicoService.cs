using AutoMapper;
using EasyBooking.Application.Contracts;
using EasyBooking.Application.Dtos;
using EasyBooking.Persistence.Interfaces;

namespace EasyBooking.Application.Services
{
    public class PaqueteTuristicoService : IPaqueteTuristicoService
    {
        private readonly IPaqueteTuristicoRepository _paqueteRepository;
        private readonly IMapper _mapper;

        public PaqueteTuristicoService(IPaqueteTuristicoRepository paqueteRepository, IMapper mapper)
        {
            _paqueteRepository = paqueteRepository;
            _mapper = mapper;
        }

        public async Task<List<PaqueteTuristicoDto>> GetAllPaquetesAsync()
        {
            var paquetes = await _paqueteRepository.GetAllAsync();
            return _mapper.Map<List<PaqueteTuristicoDto>>(paquetes);
        }

        public async Task<PaqueteTuristicoDto?> GetPaqueteByIdAsync(int id)
        {
            var paquete = await _paqueteRepository.GetPaqueteWithImagesAsync(id);

            if (paquete != null)
            {
                var paqueteDto = _mapper.Map<PaqueteTuristicoDto>(paquete);
                return paqueteDto;
            }
            return null;
        }

        public async Task<List<PaqueteTuristicoDto>> BuscarPaquetesAsync(string? destino = null, decimal? precioMinimo = null, decimal? precioMaximo = null, int? calificacion = null, int? duracionMinima = null, int? duracionMaxima = null)
        {
            var paquetes = await _paqueteRepository.BuscarPaquetesAsync(destino, precioMinimo, precioMaximo, calificacion, duracionMinima, duracionMaxima);
            return _mapper.Map<List<PaqueteTuristicoDto>>(paquetes);
        }
    }
}
