using AutoMapper;
using EasyBooking.Application.Dtos;
using EasyBooking.Domain.Entities;


namespace EasyBooking.Application.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Usuario mappings
            CreateMap<Usuario, UsuarioDto>();
            CreateMap<RegistroUsuarioDto, Usuario>();

            // Hotel mappings
            CreateMap<Hotel, HotelDto>()
             .ForMember(dest => dest.Servicios, opt => opt.MapFrom(src => src.Servicios));

            // Reserva mappings
            CreateMap<Reserva, ReservaDto>()
                .ForMember(dest => dest.Estado, opt => opt.MapFrom(src => src.Estado.ToString()));
            CreateMap<CrearReservaDto, Reserva>();

            // Mapeo de HotelImagen a HotelImagenDto
            CreateMap<HotelImagen, HotelImagenDto>().ReverseMap();
        }
    }
}