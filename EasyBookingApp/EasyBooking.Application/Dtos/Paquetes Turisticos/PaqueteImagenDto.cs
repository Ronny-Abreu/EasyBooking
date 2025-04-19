namespace EasyBooking.Application.Dtos
{
    public class PaqueteImagenDto : DtoBase
    {
        public int PaqueteId { get; set; }
        public string Url { get; set; } = string.Empty;
        public string Titulo { get; set; } = string.Empty;
        public bool EsPrincipal { get; set; }
        public int Orden { get; set; }
    }
}
