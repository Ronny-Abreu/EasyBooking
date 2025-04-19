using EasyBooking.Application.Contracts;

namespace EasyBooking.Application.Services
{
    public class TokenService : ITokenService
    {
        private static readonly Dictionary<string, int> _tokenMap = new();

        public Task<string> GenerarTokenEliminacionAsync(int usuarioId)
        {
            var token = Guid.NewGuid().ToString();
            _tokenMap[token] = usuarioId;
            return Task.FromResult(token);
        }

        public Task<int?> ObtenerUsuarioIdDesdeTokenAsync(string token)
        {
            if (_tokenMap.TryGetValue(token, out var userId))
            {
                _tokenMap.Remove(token);
                return Task.FromResult<int?>(userId);
            }

            return Task.FromResult<int?>(null);
        }
    }
}
