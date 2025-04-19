namespace EasyBooking.Application.Dtos
{
    public class ReservaPaqueteDto : DtoBase
    {
        public int UsuarioId { get; set; }
        public UsuarioDto? Usuario { get; set; }
        public int PaqueteId { get; set; }
        public PaqueteTuristicoDto? Paquete { get; set; }
        public DateTime FechaInicio { get; set; }
        public int NumeroPersonas { get; set; }
        public decimal PrecioTotal { get; set; }
        public string Estado { get; set; } = "Pendiente";
        public string? ReferenciaPago { get; set; }
    }
}
