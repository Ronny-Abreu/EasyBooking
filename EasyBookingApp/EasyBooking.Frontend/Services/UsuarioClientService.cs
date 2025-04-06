using System.Net.Http;
using EasyBooking.Frontend.Models;

namespace EasyBooking.Frontend.Services
{
    public class UsuarioClientService
    {
        private readonly HttpClientService _httpClient;

        public UsuarioClientService(HttpClientService httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<ApiResponse<UsuarioViewModel>> RegistrarUsuarioAsync(UsuarioViewModel usuario)
        {
            return await _httpClient.PostAsync<UsuarioViewModel>("Usuario", usuario);
        }

        public async Task<ApiResponse<UsuarioViewModel>> LoginAsync(string email, string password)
        {
            var loginData = new { Email = email, Password = password };
            return await _httpClient.PostAsync<UsuarioViewModel>("Usuario/Login", loginData);
        }

        public async Task<ApiResponse<UsuarioViewModel>> ObtenerUsuarioPorIdAsync(int id)
        {
            return await _httpClient.GetAsync<UsuarioViewModel>($"Usuario/{id}");
        }

        public async Task<ApiResponse<List<UsuarioViewModel>>> ObtenerTodosLosUsuariosAsync()
        {
            return await _httpClient.GetAsync<List<UsuarioViewModel>>("Usuario");
        }

        public async Task<ApiResponse<UsuarioViewModel>> ActualizarUsuarioAsync(UsuarioViewModel usuario)
        {
            return await _httpClient.PutAsync<UsuarioViewModel>($"Usuario/{usuario.Id}", usuario);
        }

        public async Task<ApiResponse<bool>> EliminarUsuarioAsync(int id)
        {
            return await _httpClient.DeleteAsync<bool>($"Usuario/{id}");
        }

    }
}