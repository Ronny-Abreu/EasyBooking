namespace EasyBooking.Domain.Core
{
    public abstract class ClaseBase
    {
        public int Id { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public bool Activo { get; set; } = true;
    }


}
