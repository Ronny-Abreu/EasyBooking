using AutoMapper;
using EasyBooking.Application.Dtos;
using EasyBooking.Frontend.Models;
using static EasyBooking.Frontend.Models.HotelViewModel;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace EasyBooking.Frontend.Mappings
{
    public class FrontendMappingProfile : Profile
    {
        public FrontendMappingProfile()
        {
            // Mapeos entre DTOs y ViewModels
            CreateMap<HotelDto, HotelViewModel>().ReverseMap();
            CreateMap<UsuarioDto, UsuarioViewModel>().ReverseMap();
            CreateMap<ReservaDto, ReservaViewModel>().ReverseMap();

            // Mapeo para HotelImagenDto
            CreateMap<HotelImagenDto, HotelImagenViewModel>().ReverseMap();

            // Mapeo para Paquetes turísticos
            CreateMap<PaqueteTuristicoDto, PaqueteTuristicoViewModel>().ReverseMap();
            CreateMap<PaqueteImagenDto, PaqueteTuristicoViewModel.PaqueteImagenViewModel>().ReverseMap();
            CreateMap<ReservaPaqueteDto, ReservaPaqueteViewModel>().ReverseMap();
        }
    }
}