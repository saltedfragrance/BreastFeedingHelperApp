﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Domain.Services.Interfaces
{
    public interface IUserService
    {
        Task<bool> Login(string email, string password);

        Task<bool> Logout();

        Task<bool> Register(string firstName, string lastName, string userName, string email, string passWord, int midWifePhoneNumber, string location);
    }
}
