using EasyBooking.Application.Dtos;

namespace EasyBooking.Application.Dtos
{
    public class HotelDto : DtoBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        // Nuevas propiedades para coordenadas
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public string Ciudad { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string ImagenUrl { get; set; } = string.Empty;
        public decimal PrecioPorNoche { get; set; }
        public int Calificacion { get; set; }
        public string Servicios { get; set; } = string.Empty;

    }
}