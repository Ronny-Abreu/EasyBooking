using EasyBooking.Application.Contracts;
using EasyBooking.Domain.Entities;
using EasyBooking.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace EasyBooking.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly EasyBookingDbContext _context;

        public UsuarioService(EasyBookingDbContext context)
        {
            _context = context;
        }

        public async Task<Usuario> CrearUsuarioAsync(Usuario usuario)
        {
            try
            {
                // Verificar si el email ya existe
                var existingEmail = await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == usuario.Email);
                if (existingEmail != null)
                {
                    throw new InvalidOperationException("El email ya está registrado.");
                }

                // Verificar si el username ya existe
                var existingUsername = await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == usuario.Username);
                if (existingUsername != null)
                {
                    throw new InvalidOperationException("El nombre de usuario ya está registrado.");
                }

                _context.Usuarios.Add(usuario);
                await _context.SaveChangesAsync();
                return usuario;
            }
            catch (DbUpdateException ex)
            {
                // Loggear la excepción
                Console.WriteLine($"Error de base de datos: {ex.Message}");
                throw new Exception("Ocurrió un error al guardar el usuario en la base de datos.", ex);
            }
            catch (Exception ex)
            {
                // Loggear la excepción
                Console.WriteLine($"Error general: {ex.Message}");
                throw new Exception("Ocurrió un error al crear el usuario.", ex);
            }
        }

        public async Task<Usuario> ObtenerUsuarioPorIdAsync(int id)
        {
            // Usar FindAsync es más eficiente si solo se busca por ID
            var usuario = await _context.Usuarios.FindAsync(id);
            return usuario;  // Si no se encuentra, devuelve null
        }

        public async Task<IEnumerable<Usuario>> ObtenerUsuariosAsync()
        {
            return await _context.Usuarios.ToListAsync();
        }

        public async Task<Usuario> ActualizarUsuarioAsync(Usuario usuario)
        {
            var usuarioExistente = await ObtenerUsuarioPorIdAsync(usuario.Id);
            if (usuarioExistente == null)
            {
                throw new KeyNotFoundException($"Usuario con ID {usuario.Id} no encontrado.");
            }

            try
            {
                _context.Usuarios.Update(usuario);
                await _context.SaveChangesAsync();
                return usuario;
            }
            catch (Exception ex)
            {
                // Manejo de errores (podrías loggear o lanzar una excepción personalizada)
                throw new Exception("Ocurrió un error al actualizar el usuario.", ex);
            }
        }

        public async Task EliminarUsuarioAsync(int id)
        {
            var usuario = await ObtenerUsuarioPorIdAsync(id);
            if (usuario == null)
            {
                throw new KeyNotFoundException($"Usuario con ID {id} no encontrado.");
            }

            try
            {
                _context.Usuarios.Remove(usuario);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Manejo de errores (podrías loggear o lanzar una excepción personalizada)
                throw new Exception("Ocurrió un error al eliminar el usuario.", ex);
            }
        }

        public async Task<Usuario> ObtenerPorEmailAsync(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<Usuario> ObtenerPorUsernameAsync(string username)
        {
            return await _context.Usuarios.FirstOrDefaultAsync(u => u.Username == username);
        }
    }
}

