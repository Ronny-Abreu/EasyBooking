using System.ComponentModel.DataAnnotations;

namespace EasyBooking.Frontend.Models
{
    public class PaqueteTuristicoViewModel
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public int Duracion { get; set; }
        public decimal Precio { get; set; }
        public int Calificacion { get; set; }
        public string ImagenUrl { get; set; } = string.Empty;
        public string Incluye { get; set; } = string.Empty;
        public string NoIncluye { get; set; } = string.Empty;
        public string Itinerario { get; set; } = string.Empty;
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }
        public DateTime FechaCreacion { get; set; }

        public List<PaqueteImagenViewModel> Imagenes { get; set; } = new List<PaqueteImagenViewModel>();

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
            return string.IsNullOrEmpty(ImagenUrl) ? "/img/packages/default.jpg" : ImagenUrl;
        }

        public class PaqueteImagenViewModel
        {
            public int Id { get; set; }
            public int PaqueteId { get; set; }
            public string Url { get; set; } = string.Empty;
            public string Titulo { get; set; } = string.Empty;
            public bool EsPrincipal { get; set; }
            public int Orden { get; set; }
        }

        public string GetGoogleMapsUrl()
        {
            if (Latitud.HasValue && Longitud.HasValue)
            {
                return $"https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d15000!2d{Longitud}!3d{Latitud}!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x0!2z!5e0!3m2!1ses-419!2s!4v1!5m2!1ses-419!2s";
            }
            else
            {
                string ubicacion = $"{Destino}, {Pais}";
                string ubicacionCodificada = System.Net.WebUtility.UrlEncode(ubicacion);
                return $"https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d15000!2d-0!3d0!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x0!2z{ubicacionCodificada}!5e0!3m2!1ses-419!2s!4v1!5m2!1ses-419!2s";
            }
        }
    }

    public class BusquedaPaquetesViewModel
    {
        public List<PaqueteTuristicoViewModel> Paquetes { get; set; } = new List<PaqueteTuristicoViewModel>();

        [Display(Name = "Destino")]
        public string? Destino { get; set; }

        [Display(Name = "Precio mínimo")]
        public decimal? PrecioMinimo { get; set; }

        [Display(Name = "Precio máximo")]
        public decimal? PrecioMaximo { get; set; }

        [Display(Name = "Calificación mínima")]
        public int? Calificacion { get; set; }

        [Display(Name = "Duración mínima (días)")]
        public int? DuracionMinima { get; set; }

        [Display(Name = "Duración máxima (días)")]
        public int? DuracionMaxima { get; set; }
        public int TotalPaquetes { get; set; }

    }
}
