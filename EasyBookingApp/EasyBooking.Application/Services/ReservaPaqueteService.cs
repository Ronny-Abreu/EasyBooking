using AutoMapper;
using EasyBooking.Application.Contracts;
using EasyBooking.Application.Dtos;
using EasyBooking.Domain.Entities;
using EasyBooking.Persistence.Interfaces;

namespace EasyBooking.Application.Services
{
    public class ReservaPaqueteService : IReservaPaqueteService
    {
        private readonly IReservaPaqueteRepository _reservaPaqueteRepository;
        private readonly IPaqueteTuristicoRepository _paqueteRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public ReservaPaqueteService(
            IReservaPaqueteRepository reservaPaqueteRepository,
            IPaqueteTuristicoRepository paqueteRepository,
            IUsuarioRepository usuarioRepository,
            IMapper mapper,
            IEmailService emailService)
        {
            _reservaPaqueteRepository = reservaPaqueteRepository;
            _paqueteRepository = paqueteRepository;
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _emailService = emailService;
        }

        public async Task<ReservaPaqueteDto?> CrearReservaPaqueteAsync(CrearReservaPaqueteDto reservaDto, int usuarioId)
        {
            // Verifica que el paquete existe
            var paquete = await _paqueteRepository.GetByIdAsync(reservaDto.PaqueteId);
            if (paquete == null)
            {
                return null;
            }

            // Verifica que el usuario existe
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
            {
                return null;
            }

            // Calcula el precio total
            var precioTotal = paquete.Precio * reservaDto.NumeroPersonas;

            // Crea la reserva
            var reserva = new ReservaPaquete
            {
                UsuarioId = usuarioId,
                PaqueteId = reservaDto.PaqueteId,
                FechaInicio = reservaDto.FechaInicio,
                NumeroPersonas = reservaDto.NumeroPersonas,
                PrecioTotal = precioTotal,
                Estado = EstadoReservaPaquete.Pendiente
            };

            await _reservaPaqueteRepository.AddAsync(reserva);

            // Envío de correo al usuario
            string subject = "📩 Confirmación de Reserva de Paquete - EasyBooking";
            string body = $@"
                <html>
                <head>
                    <style>
                        body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
                        .container {{ max-width: 600px; margin: auto; padding: 20px; }}
                        .header {{ background-color: indianred; padding: 20px; text-align: center; color: white; }}
                        .content {{ padding: 20px; }}
                        .button {{ display: inline-block; padding: 12px 24px; background-color: #28a745; color: white; text-decoration: none; border-radius: 5px; font-weight: bold; }}
                        .footer {{ background-color: #f8f9fa; padding: 15px; text-align: center; font-size: 0.8em; color: #555; }}
                    </style>
                </head>
                <body>
                    <div class='container'>
                        <div class='header'>
                            <h2>¡Tu reserva de paquete está casi lista, {usuario.Nombre}!</h2>
                        </div>
                        <div class='content'>
                            <p>Has reservado el paquete turístico <strong>{paquete.Nombre}</strong>.</p>
                            <p><strong>Fecha de inicio:</strong> {reserva.FechaInicio:dd/MM/yyyy}</p>
                            <p><strong>Número de personas:</strong> {reserva.NumeroPersonas}</p>
                            <p><strong>Precio total:</strong> {precioTotal:C}</p>
                            <br />
                            <p>Para completar tu reserva, por favor realiza el pago haciendo clic en el siguiente botón:</p>
                            <p><a class='button' href='https://localhost:7243/ReservasPaquetes'>Ir a mis Reservas (Inicia sesión si es necesario)</a>
                </p>
                            <br />
                            <p>Gracias por confiar en <strong>EasyBooking</strong>. ¡Te esperamos!</p>
                        </div>
                        <div class='footer'>
                            © 2025 EasyBooking. Todos los derechos reservados.
                        </div>
                    </div>
                </body>
                </html>";

            await _emailService.SendEmailAsync(usuario.Email, subject, body);

            return _mapper.Map<ReservaPaqueteDto>(reserva);
        }

        public async Task<ReservaPaqueteDto?> GetReservaPaqueteByIdAsync(int id)
        {
            var reserva = await _reservaPaqueteRepository.GetReservaByIdWithDetailsAsync(id);

            if (reserva == null)
                return null;

            return _mapper.Map<ReservaPaqueteDto>(reserva);
        }

        public async Task<List<ReservaPaqueteDto>> GetReservasPaqueteByUsuarioIdAsync(int usuarioId)
        {
            var reservas = await _reservaPaqueteRepository.GetReservasByUsuarioIdAsync(usuarioId);
            return _mapper.Map<List<ReservaPaqueteDto>>(reservas);
        }

        public async Task<bool> CancelarReservaPaqueteAsync(int id)
        {
            // Verifica que la reserva existe
            var reserva = await _reservaPaqueteRepository.GetByIdAsync(id);
            if (reserva == null)
            {
                return false;
            }

            // Cambia el estado a cancelado
            reserva.Estado = EstadoReservaPaquete.Cancelada;

            await _reservaPaqueteRepository.UpdateAsync(reserva);

            var usuario = await _usuarioRepository.GetByIdAsync(reserva.UsuarioId);
            if (usuario != null)
            {
                string subject = "📩 Cancelación de Reserva de Paquete - EasyBooking";
                string body = $@"
            <html>
            <head>
                <style>
                    body {{ font-family: Arial, sans-serif; line-height: 1.6; }}
                    .container {{ max-width: 600px; margin: auto; padding: 20px; }}
                    .header {{ background-color: indianred; padding: 20px; text-align: center; color: white; }}
                    .content {{ padding: 20px; }}
                    .footer {{ background-color: #f8f9fa; padding: 15px; text-align: center; font-size: 0.8em; color: #555; }}
                </style>
            </head>
            <body>
                <div class='container'>
                    <div class='header'>
                        <h2>Tu reserva de paquete ha sido cancelada, {usuario.Nombre}</h2>
                    </div>
                    <div class='content'>
                        <p>Lamentamos informarte que tu reserva del paquete turístico ha sido cancelada.</p>
                        <p>Si tienes alguna pregunta o necesitas asistencia, no dudes en contactarnos.</p>
                        <p>Gracias por confiar en <strong>EasyBooking</strong>.</p>
                    </div>
                    <div class='footer'>
                        © 2025 EasyBooking. Todos los derechos reservados.
                    </div>
                </div>
            </body>
            </html>";

                await _emailService.SendEmailAsync(usuario.Email, subject, body);
            }

            return true;
        }

        public async Task<ReservaPaqueteDto?> ProcesarPagoAsync(PagoDto pagoDto)
        {
            // Verificar que la reserva existe
            var reserva = await _reservaPaqueteRepository.GetByIdAsync(pagoDto.ReservaId);
            if (reserva == null)
            {
                return null;
            }

            // Simulación de procesamiento de pago
            try
            {
                // Genera una referencia de pago única
                var referenciaPago = $"PAY-{Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper()}";

                reserva.Estado = EstadoReservaPaquete.Confirmada;
                reserva.ReferenciaPago = referenciaPago;

                await _reservaPaqueteRepository.UpdateAsync(reserva);

                return _mapper.Map<ReservaPaqueteDto>(reserva);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
