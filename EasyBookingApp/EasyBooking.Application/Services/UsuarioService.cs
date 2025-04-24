using AutoMapper;
using EasyBooking.Application.Contracts;
using EasyBooking.Application.Dtos;
using EasyBooking.Domain.Entities;
using EasyBooking.Persistence.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace EasyBooking.Application.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IMapper mapper,
            IEmailService emailService)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<UsuarioDto?> RegistrarUsuarioAsync(RegistroUsuarioDto registroDto)
        {
            // Verifica si el email ya existe
            if (await _usuarioRepository.ExisteEmailAsync(registroDto.Email))
            {
                return null;
            }

            // Verifica que las contraseñas coincidan
            if (registroDto.Password != registroDto.ConfirmarPassword)
            {
                return null;
            }

            var passwordHash = HashPassword(registroDto.Password);

            var usuario = new Usuario
            {
                Nombre = registroDto.Nombre,
                Apellido = registroDto.Apellido,
                Email = registroDto.Email,
                PasswordHash = passwordHash,
                Telefono = registroDto.Telefono
            };

            await _usuarioRepository.AddAsync(usuario);


            return _mapper.Map<UsuarioDto>(usuario);
        }



        public async Task<UsuarioDto?> LoginAsync(LoginUsuarioDto loginDto)
        {
            // Buscar el usuario por email
            var usuario = await _usuarioRepository.GetByEmailAsync(loginDto.Email);
            if (usuario == null)
            {
                return null;
            }

            if (!VerifyPassword(loginDto.Password, usuario.PasswordHash))
            {
                return null;
            }

            return _mapper.Map<UsuarioDto>(usuario);
        }


        public async Task<bool> EliminarUsuarioAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                return false;

            await _usuarioRepository.DeleteAsync(usuario);
            return true;
        }

        public async Task<UsuarioDto?> ActualizarUsuarioAsync(ActualizarUsuarioDto dto)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(dto.Id);
            if (usuario == null) return null;

            bool hayCambios = false;

            if (!string.IsNullOrWhiteSpace(dto.Nombre) && usuario.Nombre != dto.Nombre)
            {
                usuario.Nombre = dto.Nombre;
                hayCambios = true;
            }


            if (!string.IsNullOrWhiteSpace(dto.Apellido) && usuario.Apellido != dto.Apellido)
            {
                usuario.Apellido = dto.Apellido;
                hayCambios = true;
            }

            if (!string.IsNullOrWhiteSpace(dto.Telefono) && usuario.Telefono != dto.Telefono)
            {
                usuario.Telefono = dto.Telefono;
                hayCambios = true;
            }

            if (!string.IsNullOrWhiteSpace(dto.Email) && usuario.Email != dto.Email)
            {
                // Opcional: Verifica si el nuevo email ya está en uso
                var existeEmail = await _usuarioRepository.ExisteEmailAsync(dto.Email);
                if (existeEmail)
                {
                    throw new Exception("El nuevo correo ya está en uso.");
                }

                usuario.Email = dto.Email;
                hayCambios = true;
            }


            if (!string.IsNullOrWhiteSpace(dto.PasswordNueva))
            {
                if (string.IsNullOrWhiteSpace(dto.PasswordActual) || !VerifyPassword(dto.PasswordActual, usuario.PasswordHash))
                {
                    throw new Exception("La contraseña actual es incorrecta.");
                }

                usuario.PasswordHash = HashPassword(dto.PasswordNueva);
                hayCambios = true;
            }

            if (!hayCambios)
            {
                throw new Exception("No se detectaron cambios para actualizar.");
            }

            await _usuarioRepository.UpdateAsync(usuario);
            return _mapper.Map<UsuarioDto>(usuario);
        }


        public async Task<bool> ValidarContrasenaAsync(int id, string password)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null)
                return false;

            return VerifyPassword(password, usuario.PasswordHash);
        }


        public async Task<UsuarioDto?> GetUsuarioByIdAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            return usuario != null ? _mapper.Map<UsuarioDto>(usuario) : null;
        }

        public async Task<UsuarioDto?> GetUsuarioByEmailAsync(string email)
        {
            var usuario = await _usuarioRepository.GetByEmailAsync(email);
            return usuario != null ? _mapper.Map<UsuarioDto>(usuario) : null;
        }

        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
            return Convert.ToBase64String(hashedBytes);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            var hashedPassword = HashPassword(password);
            return hashedPassword == storedHash;
        }
    }
}