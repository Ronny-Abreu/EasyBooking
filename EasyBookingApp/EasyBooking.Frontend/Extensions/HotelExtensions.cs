using EasyBooking.Frontend.Models;
using System.Web;

namespace EasyBooking.Frontend.Extensions
{
    public static class HotelExtensions
    {
        public static string GetGoogleMapsUrl(this HotelViewModel hotel)
        {
            if (string.IsNullOrEmpty(hotel.Direccion) || string.IsNullOrEmpty(hotel.Ciudad) || string.IsNullOrEmpty(hotel.Pais))
                return string.Empty;

            string ubicacion = $"{hotel.Direccion}, {hotel.Ciudad}, {hotel.Pais}";
            string ubicacionCodificada = HttpUtility.UrlEncode(ubicacion);

            return $"https://www.google.com/maps/embed?pb=!1m18!1m12!1m3!1d15000!2d-0!3d0!2m3!1f0!2f0!3f0!3m2!1i1024!2i768!4f13.1!3m3!1m2!1s0x0%3A0x0!2z{ubicacionCodificada}!5e0!3m2!1ses-419!2s!4v1!5m2!1ses-419!2s";
        }
    }
}
