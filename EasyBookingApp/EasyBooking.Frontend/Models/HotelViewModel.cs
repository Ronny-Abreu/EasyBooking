using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Frontend.Models
{
    public class HotelViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Ciudad { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public string ImagenUrl { get; set; } = string.Empty;
        public decimal PrecioPorNoche { get; set; }
        public int Calificacion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string Servicios { get; set; } = string.Empty;

    }

    public class BusquedaHotelesViewModel
    {
        public List<HotelViewModel> Hoteles { get; set; } = new List<HotelViewModel>();

        [Display(Name = "Ciudad")]
        public string? Ciudad { get; set; }

        [Display(Name = "Precio mínimo")]
        public decimal? PrecioMinimo { get; set; }

        [Display(Name = "Precio máximo")]
        public decimal? PrecioMaximo { get; set; }

        [Display(Name = "Calificación mínima")]
        public int? Calificacion { get; set; }
    }

    public class ServicioViewModel
    {
        public string Nombre { get; set; } = string.Empty;
        public string IconoCss { get; set; } = string.Empty;
    }
}