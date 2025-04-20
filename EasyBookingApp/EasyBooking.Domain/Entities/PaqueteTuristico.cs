using EasyBooking.Domain.Core;
using EasyBooking.Domain.Entities;

namespace EasyBooking.Domain.Entities
{
    public class PaqueteTuristico : ClaseBase
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Destino { get; set; } = string.Empty;
        public string Pais { get; set; } = string.Empty;
        public int Duracion { get; set; } // Duración en días
        public decimal Precio { get; set; }
        public int Calificacion { get; set; }
        public string ImagenUrl { get; set; } = string.Empty;
        public string Incluye { get; set; } = string.Empty; // Lista de servicios incluidos
        public string NoIncluye { get; set; } = string.Empty; // Lista de servicios no incluidos
        public string Itinerario { get; set; } = string.Empty; // Descripción del itinerario
        public double? Latitud { get; set; }
        public double? Longitud { get; set; }

        // Colecciones de navegación
        public ICollection<ReservaPaquete> Reservas { get; set; } = new List<ReservaPaquete>();
        public ICollection<PaqueteImagen> Imagenes { get; set; } = new List<PaqueteImagen>();
    }
}
