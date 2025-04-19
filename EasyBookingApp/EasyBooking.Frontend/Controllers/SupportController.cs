using EasyBooking.Application.Contracts;
using EasyBooking.Frontend.Models;
using Microsoft.AspNetCore.Mvc;

namespace EasyBooking.Frontend.Controllers
{
    public class SupportController : Controller
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<SupportController> _logger;

        public SupportController(IEmailService emailService, ILogger<SupportController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult SupportUser()
        {
            return View("~/Views/Usuarios/SupportUser.cshtml", new SupportViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SupportUser(SupportViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("~/Views/Usuarios/SupportUser.cshtml", model);
            }

            try
            {
                string subject = "📩 Nueva consulta de soporte - EasyBooking";
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
                            <h2>Nueva consulta de soporte</h2>
                        </div>
                        <div class='content'>
                            <p><strong>Correo electrónico del usuario:</strong> {model.Email}</p>
                            <p><strong>Mensaje:</strong></p>
                            <p style='background-color: #f8f9fa; padding: 15px; border-radius: 5px;'>{model.Message}</p>
                        </div>
                        <div class='footer'>
                            © 2025 EasyBooking. Todos los derechos reservados.
                        </div>
                    </div>
                </body>
                </html>";

                // Enviar correo electrónico
                await _emailService.SendEmailAsync("EasyBookingValidation@gmail.com", subject, body);

                string userSubject = "✅ Hemos recibido tu consulta - EasyBooking";
                string userBody = $@"
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
                            <h2>Hemos recibido tu consulta</h2>
                        </div>
                        <div class='content'>
                            <p>Hola,</p>
                            <p>Gracias por contactar con el soporte de EasyBooking. Hemos recibido tu consulta y te responderemos lo antes posible.</p>
                            <p><strong>Tu mensaje:</strong></p>
                            <p style='background-color: #f8f9fa; padding: 15px; border-radius: 5px;'>{model.Message}</p>
                            <p>Si tienes alguna otra pregunta, no dudes en contactarnos.</p>
                            <p>Saludos cordiales,<br>El equipo de EasyBooking</p>
                        </div>
                        <div class='footer'>
                            © 2025 EasyBooking. Todos los derechos reservados.
                        </div>
                    </div>
                </body>
                </html>";

                await _emailService.SendEmailAsync(model.Email, userSubject, userBody);

                TempData["SuccessMessage"] = "Tu consulta ha sido enviada correctamente. Te responderemos lo antes posible.";
                return RedirectToAction("SupportUser");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar correo de soporte");
                ModelState.AddModelError("", "Ha ocurrido un error al enviar tu consulta. Por favor, inténtalo de nuevo más tarde.");
                return View(model);
            }
        }
    }
}
    