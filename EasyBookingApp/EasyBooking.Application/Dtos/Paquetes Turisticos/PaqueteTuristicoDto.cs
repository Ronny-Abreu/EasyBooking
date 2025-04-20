namespace EasyBooking.Application.Dtos
{
    public class PaqueteTuristicoDto : DtoBase
    {
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

        public List<PaqueteImagenDto> Imagenes { get; set; } = new List<PaqueteImagenDto>();
    }
}
