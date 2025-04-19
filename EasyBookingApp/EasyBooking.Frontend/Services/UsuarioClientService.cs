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

        // Registrar nuevo usuario
        public async Task<ApiResponse<UsuarioViewModel>> RegistrarUsuarioAsync(RegistroUsuarioViewModel usuario)
        {
            return await _httpClient.PostAsync<UsuarioViewModel>("Usuarios/registro", usuario);
        }

        // Iniciar sesión
        public async Task<ApiResponse<UsuarioViewModel>> LoginAsync(LoginUsuarioViewModel login)
        {
            return await _httpClient.PostAsync<UsuarioViewModel>("Usuarios/login", login);
        }

        // Obtener usuario por ID
        public async Task<ApiResponse<UsuarioViewModel>> ObtenerUsuarioPorIdAsync(int id)
        {
            return await _httpClient.GetAsync<UsuarioViewModel>($"Usuarios/{id}");
        }

        // Eliminar usuario por ID
        public async Task<ApiResponse<bool>> EliminarUsuarioAsync(int id)
        {
            return await _httpClient.DeleteAsync<bool>($"Usuarios/{id}");
        }
    }
}
