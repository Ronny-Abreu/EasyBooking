using EasyBooking.Domain.Core;

namespace EasyBooking.Domain.Entities
{
    public class HotelImagen : ClaseBase
    {
        public int HotelId { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public bool EsPrincipal { get; set; } = false;
        public int Orden { get; set; } = 0;

        // Propiedad de navegación
        public Hotel Hotel { get; set; } = null!;
    }
}