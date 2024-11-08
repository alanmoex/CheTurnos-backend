﻿using Application.Models.Requests;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces
{
    public interface IAuthenticationService
    {
        string Authenticate (AuthenticationRequest authenticationRequest);
    }
}
