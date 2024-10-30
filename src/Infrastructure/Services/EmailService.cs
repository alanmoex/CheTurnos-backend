using Application.Interfaces;
using Application.Models;
using MimeKit;
using MailKit.Security;
using MimeKit.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Domain.Entities;
using Application.Models.Requests;
using Domain.Exceptions;
using Domain.Interfaces;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Asn1.Ocsp;


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
        private readonly IRepositoryUser _userRepository;
        public EmailService(IConfiguration config, IOptions<EmailSettingsOptions> options, IRepositoryUser repositoryUser)
        {
            _config = config;
            _options = options.Value;
            _userRepository = repositoryUser;
        }

        //Contenido del correo
        public void SendMail(EmailDTO request) //funcion basica de envio de mails
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

        public void AccountCreationConfirmationEmail(string addressee, string nameUser) //envia correo de bienvenida
        {
            if(string.IsNullOrWhiteSpace(addressee))
            {
               throw new ArgumentNullException("The recipient cannto be empty", nameof(addressee));
            }
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_options.UserName));
            email.To.Add(MailboxAddress.Parse(addressee));
            email.Subject = "Useted se a registrado en Che Turnos";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $@"<h2 style=""color: #333; text-align: center;"">¡Bienvenido a CHE Turnos!</h2>
                   <p style=""font-size: 16px; color: #555;"">Hola <strong>{nameUser}</strong>,</p>
                   <p style=""font-size: 16px; color: #555;"">Estamos emocionados de informarte que tu registro fue exitoso. Ahora puedes comenzar a explorar tu cuenta.</p>"
            };

            try
            {
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_options.UserName, _options.Password);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("could not send confiration email", ex);
            }
        }

        public void SendPasswordRestCode(string addressee, string resetCode, string userName) //envio de codigo para recuperar la cuenta.
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_options.UserName));
            email.To.Add(MailboxAddress.Parse(addressee));
            email.Subject = "Recuperacion de Contraseña de Che Turnos";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $@"<p>Hola,{userName}</p>
                  <p>Hemos recibido una solicitud para restablecer su contraseña.  Utilice el siguiente código para restablecer su contraseña</p>
                  <p> Tiene 15 minutos antes de que se venza su codigo para restablecer su contraseña.</p>
                  <h3>{resetCode}</h3>
                  <p>Si no has solicitado restablecer tu contraseña, ignora este correo electrónico.</p>"
            };
            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.UserName, _options.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public void changePassword(string addressee, string userName) //Confirmacion de resetablecimiento de contraseña
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_options.UserName));
            email.To.Add(MailboxAddress.Parse(addressee));
            email.Subject = "Recuperacion de Contraseña de Che Turnos";
            email.Body = new TextPart(TextFormat.Html)
            {
                Text = $@"<p>Hola, {userName}</p>
                  <p>Hemos restablecido su contraseña.</p>
                  <h3></h3>
                  <p>Ahora puedes seguir gestionando tus turnos.</p>"
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.UserName, _options.Password);
            smtp.Send(email);
            smtp.Disconnect(true);
        }

        public void RequestPassReset(string email) //Pide la clave para cambiar su contraseña
        {
            var user = _userRepository.GetByEmail(email)
                ?? throw new NotFoundException($"{email} is not registered");

            //genero un codigo de 6 digitos para recuperar la pass
            //GUID: valor único de 16 bytes, substring: extrae los 6 primeros caracteres.
            var resetCode = Guid.NewGuid().ToString().Substring(0, 6);

            // el tiempo de expiración del codigo 15 minutos
            var expirationTime = DateTime.UtcNow.AddMinutes(15);

            //se guarda los datos en la bd
            _userRepository.SavePassResetCode(email, resetCode, expirationTime);

            //Se envia mail con el pass para recuperar la contraseña.
            SendPasswordRestCode(email, resetCode, user.Name);
        }

        public void ResetPassword(ResetPasswordRequest request) //cambia la contraseña con la clave de reset
        {
            var user = _userRepository.GetByEmail(request.email)
                ?? throw new NotFoundException($"{request.email} is not registered");

            //Valida si expiro el codigo
            if (DateTime.UtcNow > user.ResetCodeExpiration)
            {
                throw new Exception("the password recovery code has expired");
            }

            if (request.Code != user.PasswordResetCode)
            {
                throw new Exception("The recovery code is not correct ");
            }

            if (!ValidatePassword(request.NewPassword))
            {
                throw new Exception("The password does not meet requirements.");
            }

            var userUpdateDto = new UserUpdateRequestDto();
            userUpdateDto.Password = request.NewPassword;
            userUpdateDto.Email = user.Email;

            UpdateUser(user.Id, userUpdateDto);
            changePassword(user.Email, user.Name);
        }

        private bool ValidatePassword(string password)
        {
            //comprobamos si la contraseña es nula o tiene menos de 8 caracteres
            if (string.IsNullOrEmpty(password) || password.Length < 8)
            {
                return false;
            }

            /*con esta expresión regular verificaremos que la contraseña contenga al menos una letra y un número*/
            string pattern = @"^(?=.*[a-zA-Z])(?=.*\d).+$";
            //la siguiente función devolverá true si hay match, y false en caso contrario
            return Regex.IsMatch(password, pattern);
        }

        private void UpdateUser(int id, UserUpdateRequestDto request)
        {
            var user = _userRepository.GetById(id)
                ?? throw new NotFoundException("User not Found");
            
            user.Password = request.Password;

            _userRepository.Update(user);
        }

        public void NotifyClientCancellation(string email, string nameUser, string nameShop, string tel) // Notifica al cliente sobre la cancelación del turno
        {
            var mail = new MimeMessage();
            mail.From.Add(MailboxAddress.Parse(_options.UserName));
            mail.To.Add(MailboxAddress.Parse(email));
            mail.Subject = "Cancelación de turno en Che Turnos";
            mail.Body = new TextPart(TextFormat.Html)
            {
                Text = $@"<h2 style=""color: #333; text-align: center;"">Cancelación de Turno</h2>
                 <p style=""font-size: 16px; color: #555;"">Hola <strong>{nameUser}</strong>,</p>
                 <p style=""font-size: 16px; color: #555;"">Lamentamos informarte que tu turno ha sido cancelado. Para más información, por favor consulta con {nameShop} ({tel}).</p>
                 <p style=""font-size: 16px; color: #555;"">Gracias por tu comprensión.</p>"
            };

            using var smtp = new MailKit.Net.Smtp.SmtpClient();
            smtp.Connect(_options.Host, _options.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_options.UserName, _options.Password);
            smtp.Send(mail);
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

