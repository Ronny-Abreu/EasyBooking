using AutoMapper;
using EasyBooking.Application.Contracts;
using EasyBooking.Application.Dtos;
using EasyBooking.Domain.Entities;
using EasyBooking.Persistence.Interfaces;
using Stripe;

namespace EasyBooking.Application.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaRepository _reservaRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;

        public ReservaService(
            IReservaRepository reservaRepository,
            IHotelRepository hotelRepository,
            IUsuarioRepository usuarioRepository,
            IMapper mapper,
            IEmailService emailService)

        {
            _reservaRepository = reservaRepository;
            _hotelRepository = hotelRepository;
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _emailService = emailService;

        }

        public async Task<ReservaDto?> CrearReservaAsync(CrearReservaDto reservaDto, int usuarioId)
        {
            // Verifica que el hotel existe
            var hotel = await _hotelRepository.GetByIdAsync(reservaDto.HotelId);
            if (hotel == null)
            {
                return null;
            }

            // Verifica que el usuario existe
            var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
            if (usuario == null)
            {
                return null;
            }

            // Verifica disponibilidad en las fechas seleccionadas (entre reservas activas)
            if (await _reservaRepository.ExisteReservaEnFechasAsync(reservaDto.HotelId, reservaDto.FechaEntrada, reservaDto.FechaSalida))
            {
                return null;
            }

            // Calcula el precio total
            var dias = (reservaDto.FechaSalida - reservaDto.FechaEntrada).Days;
            var precioTotal = hotel.PrecioPorNoche * dias;

            // Crea la reserva (Si se llenaron todos los datos)
            var reserva = new Reserva
            {
                UsuarioId = usuarioId,
                HotelId = reservaDto.HotelId,
                FechaEntrada = reservaDto.FechaEntrada,
                FechaSalida = reservaDto.FechaSalida,
                NumeroHuespedes = reservaDto.NumeroHuespedes,
                PrecioTotal = precioTotal,
                Estado = EstadoReserva.Pendiente
            };

            await _reservaRepository.AddAsync(reserva);

            // Envío de correo al usuario iniciado
            string subject = "📩 Confirmación de Reserva - EasyBooking";
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
                            <h2>¡Tu reserva está casi lista, {usuario.Nombre}!</h2>
                        </div>
                        <div class='content'>
                            <p>Has reservado el hotel <strong>{hotel.Nombre}</strong>.</p>
                            <p><strong>Fecha de entrada:</strong> {reserva.FechaEntrada:dd/MM/yyyy}</p>
                            <p><strong>Fecha de salida:</strong> {reserva.FechaSalida:dd/MM/yyyy}</p>
                            <p><strong>Número de huéspedes:</strong> {reserva.NumeroHuespedes}</p>
                            <p><strong>Precio total:</strong> {precioTotal:C}</p>
                            <br />
                            <p>Para completar tu reserva, por favor realiza el pago haciendo clic en el siguiente botón:</p>
                            <p><a class='button' href='https://localhost:7094/Reservas'>Ir a mis Reservas (Inicia sesión si es necesario)</a>
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


            return _mapper.Map<ReservaDto>(reserva);
        }

        public async Task<ReservaDto?> GetReservaByIdAsync(int id)
        {
            var reserva = await _reservaRepository.GetReservaByIdWithDetailsAsync(id);

            if (reserva == null)
                return null;

            return _mapper.Map<ReservaDto>(reserva);
        }

        public async Task<List<ReservaDto>> GetReservasByUsuarioIdAsync(int usuarioId)
        {
            var reservas = await _reservaRepository.GetReservasByUsuarioIdAsync(usuarioId);
            return _mapper.Map<List<ReservaDto>>(reservas);
        }

        public async Task<ReservaDto?> ActualizarReservaAsync(int id, CrearReservaDto reservaDto)
        {
            var reserva = await _reservaRepository.GetByIdAsync(id);
            if (reserva == null)
            {
                return null;
            }

            var hotel = await _hotelRepository.GetByIdAsync(reservaDto.HotelId);
            if (hotel == null)
            {
                return null;
            }

            var existeReserva = await _reservaRepository.GetAsync(
                r => r.HotelId == reservaDto.HotelId && r.Id != id && r.Estado != EstadoReserva.Cancelada &&
                    ((reservaDto.FechaEntrada >= r.FechaEntrada && reservaDto.FechaEntrada < r.FechaSalida) ||
                     (reservaDto.FechaSalida > r.FechaEntrada && reservaDto.FechaSalida <= r.FechaSalida) ||
                     (reservaDto.FechaEntrada <= r.FechaEntrada && reservaDto.FechaSalida >= r.FechaSalida)));

            if (existeReserva.Any())
            {
                return null;
            }

            var dias = (reservaDto.FechaSalida - reservaDto.FechaEntrada).Days;
            var precioTotal = hotel.PrecioPorNoche * dias;

            reserva.HotelId = reservaDto.HotelId;
            reserva.FechaEntrada = reservaDto.FechaEntrada;
            reserva.FechaSalida = reservaDto.FechaSalida;
            reserva.NumeroHuespedes = reservaDto.NumeroHuespedes;
            reserva.PrecioTotal = precioTotal;

            await _reservaRepository.UpdateAsync(reserva);

            return _mapper.Map<ReservaDto>(reserva);
        }

        public async Task<bool> CancelarReservaAsync(int id)
        {
            // Verifica que la reserva existe
            var reserva = await _reservaRepository.GetByIdAsync(id);
            if (reserva == null)
            {
                return false;
            }

            // Cambia el estado a cancelado
            reserva.Estado = EstadoReserva.Cancelada;

            await _reservaRepository.UpdateAsync(reserva);

            var usuario = await _usuarioRepository.GetByIdAsync(reserva.UsuarioId);
            if (usuario != null)
            {
                string subject = "📩 Cancelación de Reserva - EasyBooking";
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
                        <h2>Tu reserva ha sido cancelada, {usuario.Nombre}</h2>
                    </div>
                    <div class='content'>
                        <p>Lamentamos informarte que tu reserva en el hotel <strong>{reserva.Hotel}</strong> ha sido cancelada.</p>
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

        public async Task<ReservaDto?> ProcesarPagoAsync(PagoDto pagoDto)
        {
            // Verificar que la reserva existe
            var reserva = await _reservaRepository.GetByIdAsync(pagoDto.ReservaId);
            if (reserva == null)
            {
                return null;
            }

            // Aquí debe ir la lógica para procesar el pago
            // POR EL MOMENTO SOLO PONDRÉ UNA SIMULACIÓN (varios inputs y que se cree una referencia de pago. DEBO RECORDAR IMPLEMENTAR UNO REAL SI DA TIEMPO AL ENTREGAR EL PROYECTO (20 de abril)
            try
            {
                // Genera una referencia de pago única
                var referenciaPago = $"PAY-{Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper()}";

                reserva.Estado = EstadoReserva.Confirmada;
                reserva.ReferenciaPago = referenciaPago;

                await _reservaRepository.UpdateAsync(reserva);

                return _mapper.Map<ReservaDto>(reserva);
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}