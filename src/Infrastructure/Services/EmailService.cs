﻿using Application.Interfaces;
using Application.Models;
using MimeKit;
using MailKit.Security;
using MimeKit.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;


namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        //para esto se necesita un mail
        //Luego se selecciona Manage your Google Account en la pagina principal de gmail
        //-> en la nueva pantalla que nos aparece buscamos "App passwords" para crear la key que necesitamos: tikx jizl tpyu bbwe
        //En el appsetting se agrega la seccion:   "MailSettings": {...}

        //Esta config se usa para poder leer lo escrito en el appsetting
        private readonly IConfiguration _config;
        private readonly EmailSettingsOptions _options;
        public EmailService(IConfiguration config, IOptions<EmailSettingsOptions> options)
        {
            _config = config;
            _options = options.Value;
        }

        //Contenido del correo
        public void SendMail(EmailDTO request)
        {
            //El corre se va a enviar desde "MailSettings: UserName"...
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_options.UserName));
           
            email.To.Add(MailboxAddress.Parse(request.For));
            email.Subject = request.Subject;
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = request.Body
            };

            //Configuracion del servidor para enviar el correo
            //SMTP configuracion por la cual se va a mandar el msj. 

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.UserName, _options.Password);
            smtp.Send( email );
            smtp.Disconnect(true);
        }

        public void SendAccountConfirmationEmail(string addressee)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_options.UserName));
            email.To.Add(MailboxAddress.Parse(addressee));
            email.Subject = "useted se a registrado en che turnos";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $@"<h2 style=""color: #333; text-align: center;"">¡Bienvenido a CHE Turnos!</h2>
                   <p style=""font-size: 16px; color: #555;"">Hola <strong>{addressee}</strong>,</p>
                   <p style=""font-size: 16px; color: #555;"">Estamos emocionados de informarte que tu registro fue exitoso. Ahora puedes comenzar a usar tu cuenta para explorar todas las funciones que ofrecemos.</p>"
            };


            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.UserName, _options.Password);
            smtp.Send(email);
            smtp.Disconnect(true);

        }

        public class EmailSettingsOptions
        {
            public const string EmailService = "MailSettings";
            public string Host { get; set; }
            public int Port { get; set; }
            public string UserName { get; set; }
            public string Password { get; set; }
        }
    } 

}
