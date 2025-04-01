namespace EasyBooking.Frontend.Models
{
    public class ApiResponse<T>
    {
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
        public bool Success { get; set; }
        public string? Error { get; set; }
    }
}