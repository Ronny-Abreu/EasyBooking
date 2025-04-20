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
            // Reemplaza las plantillas de correo existentes con las nuevas versiones
            // Por ejemplo, para el correo de confirmación de reserva:

            string subject = "🌴 ¡Tu reserva está casi lista! - EasyBooking";
            string body = $@"
             <!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Reserva Confirmada - EasyBooking</title>
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap');
        
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        
        body {{
            font-family: 'Poppins', 'Segoe UI', sans-serif;
            background-color: #f8f9fa;
            margin: 0;
            padding: 0;
            color: #333;
            -webkit-font-smoothing: antialiased;
        }}
        
        .container {{
            max-width: 600px;
            margin: 20px auto;
            background-color: #ffffff;
            border-radius: 16px;
            overflow: hidden;
            box-shadow: 0 10px 30px rgba(0,0,0,0.08);
        }}
        
        .header {{
            background: linear-gradient(135deg, #f8345c, #e02a4e);
            color: #ffffff;
            padding: 40px 20px;
            text-align: center;
            position: relative;
        }}
        
        .header-icon {{
            margin-bottom: 20px;
            font-size: 48px;
        }}
        
        .header h1 {{
            margin: 0;
            font-size: 28px;
            font-weight: 700;
            letter-spacing: -0.5px;
        }}
        
        .header-wave {{
            position: absolute;
            bottom: 0;
            left: 0;
            width: 100%;
            height: 30px;
            fill: #ffffff;
        }}
        
        .content {{
            padding: 40px 30px;
            color: #333333;
            font-size: 16px;
            line-height: 1.6;
        }}
        
        .reservation-details {{
            background-color: #f9f9f9;
            border-radius: 12px;
            padding: 25px;
            margin: 25px 0;
        }}
        
        .reservation-title {{
            font-weight: 600;
            margin-bottom: 20px;
            color: #333;
            font-size: 18px;
            display: flex;
            align-items: center;
        }}
        
        .package-name {{
            font-size: 20px;
            font-weight: 600;
            color: #f8345c;
            margin-bottom: 20px;
            padding-bottom: 15px;
            border-bottom: 1px dashed #ddd;
        }}
        
        .detail-row {{
            display: flex;
            margin-bottom: 15px;
            align-items: center;
        }}
        
        .detail-icon {{
            width: 40px;
            height: 40px;
            background-color: #fff;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            margin-right: 15px;
            box-shadow: 0 2px 10px rgba(0,0,0,0.05);
            color: #f8345c;
            font-size: 18px;
        }}
        
        .detail-content {{
            flex: 1;
        }}
        
        .detail-label {{
            font-size: 14px;
            color: #666;
            margin-bottom: 2px;
        }}
        
        .detail-value {{
            font-weight: 600;
            color: #333;
        }}
        
        .price-container {{
            background-color: #f0fff4;
            border-radius: 12px;
            padding: 20px;
            margin: 25px 0;
            display: flex;
            justify-content: space-between;
            align-items: center;
            border-left: 4px solid #10b981;
        }}
        
        .price-label {{
            font-size: 18px;
            font-weight: 600;
            color: #333;
        }}
        
        .price-value {{
            font-size: 24px;
            font-weight: 700;
            color: #10b981;
            line-height: 1;
        }}
        
        .cta-container {{
            text-align: center;
            margin: 30px 0;
        }}
        
        .cta-button {{
            display: inline-block;
            padding: 16px 35px;
            background: linear-gradient(135deg, #10b981, #059669);
            color: white;
            text-decoration: none;
            border-radius: 50px;
            font-weight: 600;
            font-size: 16px;
            transition: transform 0.3s ease, box-shadow 0.3s ease;
            box-shadow: 0 4px 15px rgba(16, 185, 129, 0.3);
        }}
        
        .next-steps {{
            background-color: #fff;
            border-radius: 12px;
            padding: 25px;
            margin: 25px 0;
            border: 1px solid #eee;
        }}
        
        .steps-title {{
            font-weight: 600;
            margin-bottom: 20px;
            color: #333;
            font-size: 18px;
        }}
        
        .step {{
            display: flex;
            margin-bottom: 15px;
        }}
        
        .step-number {{
            width: 30px;
            height: 30px;
            background-color: #f8345c;
            border-radius: 50%;
            display: flex;
            align-items: center;
            justify-content: center;
            margin-right: 15px;
            color: white;
            font-weight: 600;
            font-size: 14px;
            text-align: center;
            line-height: 30px;
            padding: 0;
        }}
        
        .step-content {{
            flex: 1;
        }}
        
        .step-title {{
            font-weight: 600;
            margin-bottom: 5px;
            color: #333;
        }}
        
        .step-description {{
            font-size: 14px;
            color: #666;
        }}
        
        .signature {{
            margin-top: 30px;
            font-weight: 500;
        }}
        
        .signature-name {{
            font-weight: 600;
            color: #333;
        }}
        
        .footer {{
            background-color: #f8f9fa;
            padding: 25px 20px;
            text-align: center;
            font-size: 14px;
            color: #666666;
            border-top: 1px solid #eeeeee;
        }}
        
        .footer p {{
            margin: 5px 0;
        }}
        
        @media only screen and (max-width: 600px) {{
            .container {{
                margin: 0;
                border-radius: 0;
            }}
            
            .content {{
                padding: 30px 20px;
            }}
            
            .header {{
                padding: 30px 15px;
            }}
            
            .header h1 {{
                font-size: 24px;
            }}
            
            .price-container {{
                flex-direction: column;
                text-align: center;
            }}
            
            .price-label {{
                margin-bottom: 10px;
            }}
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <div class=""header-icon"">
                🎉
            </div>
            <h1>¡Tu reserva está casi lista, {usuario.Nombre}!</h1>
            <svg class=""header-wave"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 1440 100"">
                <path fill=""#ffffff"" fill-opacity=""1"" d=""M0,64L80,69.3C160,75,320,85,480,80C640,75,800,53,960,48C1120,43,1280,53,1360,58.7L1440,64L1440,100L1360,100C1280,100,1120,100,960,100C800,100,640,100,480,100C320,100,160,100,80,100L0,100Z""></path>
            </svg>
        </div>
        
        <div class=""content"">
            <p>Has reservado el paquete turístico:</p>
            
            <div class=""reservation-details"">
                <p class=""package-name"">{paquete.Nombre}</p>
                
                <div class=""detail-row"">
                    <div class=""detail-icon"">📅</div>
                    <div class=""detail-content"">
                        <div class=""detail-label"">Fecha de inicio</div>
                        <div class=""detail-value"">{reserva.FechaInicio:dd/MM/yyyy}</div>
                    </div>
                </div>
                
                <div class=""detail-row"">
                    <div class=""detail-icon"">👥</div>
                    <div class=""detail-content"">
                        <div class=""detail-label"">Número de personas</div>
                        <div class=""detail-value"">{reserva.NumeroPersonas}</div>
                    </div>
                </div>
                
                <div class=""detail-row"">
                    <div class=""detail-icon"">🏞️</div>
                    <div class=""detail-content"">
                        <div class=""detail-label"">Duración</div>
                        <div class=""detail-value"">{(paquete.Duracion > 0 ? paquete.Duracion.ToString() + " días" : "Consultar")}</div>
                    </div>
                </div>
            </div>
            
            <div class=""price-container"">
                <div class=""price-label"">Precio total</div>
                <div class=""price-value"">{precioTotal:C}</div>
            </div>
            
            <p>Para completar tu reserva, realiza el pago haciendo clic en el siguiente botón:</p>
            
            <div class=""cta-container"">
                <a href=""https://localhost:7243/ReservasPaquetes"" class=""cta-button"">Completar Reserva</a>
            </div>
            
            <div class=""next-steps"">
                <p class=""steps-title"">Próximos pasos:</p>
                
                <div class=""step"">
                    <div class=""step-number"">1</div>
                    <div class=""step-content"">
                        <div class=""step-title"">Realiza el pago</div>
                        <div class=""step-description"">Completa el proceso de pago de forma segura con cualquier método disponible.</div>
                    </div>
                </div>
                
                <div class=""step"">
                    <div class=""step-number"">2</div>
                    <div class=""step-content"">
                        <div class=""step-title"">Recibe confirmación</div>
                        <div class=""step-description"">Te enviaremos un email con todos los detalles y vouchers necesarios.</div>
                    </div>
                </div>
                
                <div class=""step"">
                    <div class=""step-number"">3</div>
                    <div class=""step-content"">
                        <div class=""step-title"">¡Prepárate para tu aventura!</div>
                        <div class=""step-description"">Revisa nuestras recomendaciones para tu viaje.</div>
                    </div>
                </div>
            </div>
            
            <div class=""signature"">
                <p>Gracias por confiar en nosotros,</p>
                <p class=""signature-name"">El equipo de EasyBooking</p>
            </div>
        </div>
        
        <div class=""footer"">
            <p>© 2025 EasyBooking. Todos los derechos reservados.</p>
            <p>Si tienes alguna pregunta, contáctanos en soporte@easybooking.com</p>
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
<!DOCTYPE html>
<html lang=""es"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <title>Reserva Cancelada - EasyBooking</title>
    <style>
        @import url('https://fonts.googleapis.com/css2?family=Poppins:wght@300;400;500;600;700&display=swap');
        
        * {{
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }}
        
        body {{
            font-family: 'Poppins', 'Segoe UI', sans-serif;
            background-color: #f8f9fa;
            margin: 0;
            padding: 0;
            color: #333;
            -webkit-font-smoothing: antialiased;
        }}
        
        .container {{
            max-width: 600px;
            margin: 20px auto;
            background-color: #ffffff;
            border-radius: 16px;
            overflow: hidden;
            box-shadow: 0 10px 30px rgba(0,0,0,0.08);
        }}
        
        .header {{
            background: linear-gradient(135deg, #f8345c, #e02a4e);
            color: #ffffff;
            padding: 40px 20px;
            text-align: center;
            position: relative;
        }}
        
        .header-icon {{
            margin-bottom: 20px;
            font-size: 48px;
        }}
        
        .header h1 {{
            margin: 0;
            font-size: 28px;
            font-weight: 700;
            letter-spacing: -0.5px;
        }}
        
        .header-wave {{
            position: absolute;
            bottom: 0;
            left: 0;
            width: 100%;
            height: 30px;
            fill: #ffffff;
        }}
        
        .content {{
            padding: 40px 30px;
            color: #333333;
            font-size: 16px;
            line-height: 1.6;
        }}
        
        .message-box {{
            background-color: #fff5f5;
            border-left: 4px solid #f8345c;
            padding: 20px;
            border-radius: 8px;
            margin: 25px 0;
        }}
        
        .message-title {{
            font-weight: 600;
            margin-bottom: 10px;
            color: #333;
            font-size: 18px;
        }}
        
        .help-section {{
            background-color: #f9f9f9;
            border-radius: 12px;
            padding: 25px;
            margin: 25px 0;
        }}
        
        .help-title {{
            font-weight: 600;
            margin-bottom: 15px;
            color: #333;
            font-size: 18px;
            display: flex;
            align-items: center;
        }}
        
        .help-title svg {{
            margin-right: 10px;
        }}
        
        .help-options {{
            display: flex;
            justify-content: space-between;
            flex-wrap: wrap;
            gap: 15px;
            margin-top: 20px;
        }}
        
        .help-option {{
            flex: 1;
            min-width: 150px;
            background-color: white;
            padding: 15px;
            border-radius: 10px;
            text-align: center;
            box-shadow: 0 2px 10px rgba(0,0,0,0.05);
            transition: transform 0.3s ease;
        }}
        
        .help-option:hover {{
            transform: translateY(-3px);
        }}
        
        .help-option-icon {{
            font-size: 24px;
            margin-bottom: 10px;
            color: #f8345c;
        }}
        
        .help-option-title {{
            font-weight: 600;
            font-size: 14px;
            margin-bottom: 5px;
        }}
        
        .help-option-desc {{
            font-size: 12px;
            color: #666;
        }}
        
        .cta-container {{
            text-align: center;
            margin: 30px 0;
        }}
        
        .cta-button {{
            display: inline-block;
            padding: 14px 30px;
            background: linear-gradient(135deg, #f8345c, #e02a4e);
            color: white;
            text-decoration: none;
            border-radius: 50px;
            font-weight: 600;
            font-size: 16px;
            transition: transform 0.3s ease, box-shadow 0.3s ease;
            box-shadow: 0 4px 15px rgba(248, 52, 92, 0.3);
        }}
        
        .cta-button:hover {{
            transform: translateY(-2px);
            box-shadow: 0 6px 20px rgba(248, 52, 92, 0.4);
        }}
        
        .signature {{
            margin-top: 30px;
            font-weight: 500;
        }}
        
        .signature-name {{
            font-weight: 600;
            color: #333;
        }}
        
        .footer {{
            background-color: #f8f9fa;
            padding: 25px 20px;
            text-align: center;
            font-size: 14px;
            color: #666666;
            border-top: 1px solid #eeeeee;
        }}
        
        .footer p {{
            margin: 5px 0;
        }}
        
        @media only screen and (max-width: 600px) {{
            .container {{
                margin: 0;
                border-radius: 0;
            }}
            
            .content {{
                padding: 30px 20px;
            }}
            
            .header {{
                padding: 30px 15px;
            }}
            
            .header h1 {{
                font-size: 24px;
            }}
            
            .help-options {{
                flex-direction: column;
            }}
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <div class=""header"">
            <div class=""header-icon"">
                ✕
            </div>
            <h1>Tu reserva ha sido cancelada, {usuario.Nombre}</h1>
            <svg class=""header-wave"" xmlns=""http://www.w3.org/2000/svg"" viewBox=""0 0 1440 100"">
                <path fill=""#ffffff"" fill-opacity=""1"" d=""M0,64L80,69.3C160,75,320,85,480,80C640,75,800,53,960,48C1120,43,1280,53,1360,58.7L1440,64L1440,100L1360,100C1280,100,1120,100,960,100C800,100,640,100,480,100C320,100,160,100,80,100L0,100Z""></path>
            </svg>
        </div>
        
        <div class=""content"">
            <div class=""message-box"">
                <p class=""message-title"">Información de la cancelación</p>
                <p>Lamentamos informarte que tu reserva del paquete turístico ha sido cancelada. Si no solicitaste esta cancelación, por favor contáctanos inmediatamente.</p>
            </div>
            
            <p>Entendemos que los planes pueden cambiar. Si necesitas ayuda para encontrar una alternativa, nuestro equipo está listo para asistirte.</p>
            
            <div class=""help-section"">
                <p class=""help-title"">
                    <svg width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" xmlns=""http://www.w3.org/2000/svg"">
                        <path d=""M12 22C17.5228 22 22 17.5228 22 12C22 6.47715 17.5228 2 12 2C6.47715 2 2 6.47715 2 12C2 17.5228 6.47715 22 12 22Z"" stroke=""#f8345c"" stroke-width=""2""/>
                        <path d=""M12 16V12"" stroke=""#f8345c"" stroke-width=""2"" stroke-linecap=""round""/>
                        <path d=""M12 8H12.01"" stroke=""#f8345c"" stroke-width=""2"" stroke-linecap=""round""/>
                    </svg>
                    ¿Necesitas ayuda?
                </p>
                <p>Estamos aquí para asistirte con cualquier pregunta o inquietud que puedas tener.</p>
                
                <div class=""help-options"">
                    <div class=""help-option"">
                        <div class=""help-option-icon"">📞</div>
                        <div class=""help-option-title"">Llámanos</div>
                        <div class=""help-option-desc"">Atención 24/7</div>
                    </div>
                    <div class=""help-option"">
                        <div class=""help-option-icon"">💬</div>
                        <div class=""help-option-title"">Sistema para soporte   </div>
                        <div class=""help-option-desc"">Respuesta inmediata</div>
                    </div>
                    <div class=""help-option"">
                        <div class=""help-option-icon"">📧</div>
                        <div class=""help-option-title"">Email</div>
                        <div class=""help-option-desc"">@easybookingvalidation.com</div>
                    </div>
                </div>
            </div>
            
            <div class=""cta-container"">
                <a href=""https://localhost:7094/PaquetesTuristicos"" class=""cta-button"">Explorar Otros Paquetes</a>
            </div>
            
            <div class=""signature"">
                <p>Gracias por confiar en nosotros,</p>
                <p class=""signature-name"">El equipo de EasyBooking</p>
            </div>
        </div>
        
        <div class=""footer"">
            <p>© 2025 EasyBooking. Todos los derechos reservados.</p>
            <p>Si tienes alguna pregunta, contáctanos en @easybookingvalidation.com</p>
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
