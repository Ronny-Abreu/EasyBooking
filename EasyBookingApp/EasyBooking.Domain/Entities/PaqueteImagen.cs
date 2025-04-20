using EasyBooking.Domain.Core;

namespace EasyBooking.Domain.Entities
{
    public class PaqueteImagen : ClaseBase
    {
        public int PaqueteId { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public bool EsPrincipal { get; set; } = false;
        public int Orden { get; set; } = 0;

        // Propiedad de navegación
        public PaqueteTuristico Paquete { get; set; } = null!;
    }
}
