﻿using Application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IEmailService
    {

        void SendMail(EmailDTO request);
        void SendAccountConfirmationEmail(string addressee);
    }
}