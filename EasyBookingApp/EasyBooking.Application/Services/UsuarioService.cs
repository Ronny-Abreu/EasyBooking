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