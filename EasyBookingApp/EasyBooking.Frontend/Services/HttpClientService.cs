using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using EasyBooking.Frontend.Models;
using Microsoft.Extensions.Options;

namespace EasyBooking.Frontend.Services
{
    public class HttpClientService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;
        private readonly string _apiBaseUrl;
        private readonly ILogger<HttpClientService> _logger;

        public HttpClientService(HttpClient httpClient, IConfiguration configuration, ILogger<HttpClientService> logger)
        {
            _httpClient = httpClient;
            _configuration = configuration;
            _apiBaseUrl = _configuration["ApiSettings:BaseUrl"] ?? "https://localhost:7191/api/";
            _logger = logger;
        }
        public string ApiBaseUrl => _apiBaseUrl;

        public async Task<ApiResponse<T>> GetAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiBaseUrl}{endpoint}");
                return await ProcessResponseAsync<T>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en GetAsync para {Endpoint}", endpoint);
                return new ApiResponse<T>
                {
                    Success = false,
                    Error = ex.Message
                };
            }
        }

        public async Task<ApiResponse<T>> PostAsync<T>(string endpoint, object data)
        {
            try
            {
                var url = $"{_apiBaseUrl}{endpoint}";
                _logger.LogInformation($"Making POST request to: {url}");

                var content = new StringContent(
                    JsonSerializer.Serialize(data),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PostAsync(url, content);
                _logger.LogInformation($"Response status: {response.StatusCode}");

                return await ProcessResponseAsync<T>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en PostAsync para {Endpoint}", endpoint);
                return new ApiResponse<T>
                {
                    Success = false,
                    Error = ex.Message
                };
            }
        }

        public async Task<ApiResponse<T>> PutAsync<T>(string endpoint, object data)
        {
            try
            {
                var content = new StringContent(
                    JsonSerializer.Serialize(data),
                    Encoding.UTF8,
                    "application/json");

                var response = await _httpClient.PutAsync($"{_apiBaseUrl}{endpoint}", content);
                return await ProcessResponseAsync<T>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en PutAsync para {Endpoint}", endpoint);
                return new ApiResponse<T>
                {
                    Success = false,
                    Error = ex.Message
                };
            }
        }

        public async Task<ApiResponse<T>> DeleteAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{_apiBaseUrl}{endpoint}");
                return await ProcessResponseAsync<T>(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error en DeleteAsync para {Endpoint}", endpoint);
                return new ApiResponse<T>
                {
                    Success = false,
                    Error = ex.Message
                };
            }
        }

        private async Task<ApiResponse<T>> ProcessResponseAsync<T>(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrEmpty(content))
            {
                if (response.IsSuccessStatusCode)
                {
                    return new ApiResponse<T>
                    {
                        Success = true,
                        Message = "Operación completada con éxito"
                    };
                }
                else
                {
                    return new ApiResponse<T>
                    {
                        Success = false,
                        Error = $"Error: {response.StatusCode} - Sin contenido en la respuesta"
                    };
                }
            }

            if (response.IsSuccessStatusCode)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    var apiResponse = JsonSerializer.Deserialize<ApiResponse<T>>(content, options);

                    if (apiResponse != null)
                    {
                        apiResponse.Success = true;
                        return apiResponse;
                    }

                    var data = JsonSerializer.Deserialize<T>(content, options);
                    return new ApiResponse<T>
                    {
                        Success = true,
                        Data = data
                    };
                }
                catch (JsonException ex)
                {
                    _logger.LogError(ex, "Error al deserializar JSON: {Content}", content);
                    // Si falla la deserialización, devolver un mensaje genérico
                    return new ApiResponse<T>
                    {
                        Success = true,
                        Message = "Operación completada con éxito",
                        Error = "No se pudo procesar la respuesta del servidor"
                    };
                }
            }
            else
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true
                    };

                    using var document = JsonDocument.Parse(content);

                    string? extractedMessage = null;

                    if (document.RootElement.ValueKind == JsonValueKind.Object)
                    {
                        // Intenta buscar "message" o "error" en el JSON
                        if (document.RootElement.TryGetProperty("message", out var messageProp))
                        {
                            extractedMessage = messageProp.GetString();
                        }
                        else if (document.RootElement.TryGetProperty("error", out var errorProp))
                        {
                            extractedMessage = errorProp.GetString();
                        }
                    }

                    return new ApiResponse<T>
                    {
                        Success = false,
                        Error = extractedMessage ?? $"Error: {response.StatusCode} - {content}"
                    };
                }
                catch (JsonException)
                {
                    return new ApiResponse<T>
                    {
                        Success = false,
                        Error = $"Error: {response.StatusCode} - {content}"
                    };
                }
            }


        }
    }
}