using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyBooking.Application.Contracts
{
    public interface ITokenService
    {
        Task<string> GenerarTokenEliminacionAsync(int usuarioId);
        Task<int?> ObtenerUsuarioIdDesdeTokenAsync(string token);
    }

}
