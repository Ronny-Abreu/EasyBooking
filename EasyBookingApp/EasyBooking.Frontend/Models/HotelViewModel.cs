using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Frontend.Models
{
    public class HotelViewModel
    {
        public int Id { get; set; }
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
        public DateTime FechaCreacion { get; set; }
        public string Servicios { get; set; } = string.Empty;

        // Nueva propiedad para las imágenes
        public List<HotelImagenViewModel> Imagenes { get; set; } = new List<HotelImagenViewModel>();
        // Método para obtener la imagen principal
        public string GetImagenPrincipal()
        {
            if (Imagenes != null && Imagenes.Any())
            {
                var imagenPrincipal = Imagenes.FirstOrDefault(i => i.EsPrincipal);
                if (imagenPrincipal != null)
                {
                    return imagenPrincipal.Url;
                }
                return Imagenes.First().Url;
            }
            return string.IsNullOrEmpty(ImagenUrl) ? "/img/hotels/default.jpg" : ImagenUrl;
        }

        public class HotelImagenViewModel
        {
            public int Id { get; set; }
            public int HotelId { get; set; }
            public string Url { get; set; } = string.Empty;
            public string Titulo { get; set; } = string.Empty;
            public bool EsPrincipal { get; set; }
            public int Orden { get; set; }
        }

        public string GetGoogleMapsUrl()
        {
            if (Latitud.HasValue && Longitud.HasValue)
            {
                // Si tenemos coordenadas, usarlas directamente
                return $"https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d15000!2d{Longitud}!3d{Latitud}!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x0!2z!5e0!3m2!1ses-419!2s!4v1!5m2!1ses-419!2s";
            }
            else
            {
                // Si no tenemos coordenadas, usar la dirección
                string ubicacion = $"{Direccion}, {Ciudad}, {Pais}";
                string ubicacionCodificada = System.Net.WebUtility.UrlEncode(ubicacion);
                return $"https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d15000!2d-0!3d0!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x0!2z{ubicacionCodificada}!5e0!3m2!1ses-419!2s!4v1!5m2!1ses-419!2s";
            }
        }

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