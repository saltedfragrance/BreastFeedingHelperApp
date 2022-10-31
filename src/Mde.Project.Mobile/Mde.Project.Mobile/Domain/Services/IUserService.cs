﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Domain.Services
{
    public interface IUserService
    {
        Task Login(string email, string password);
        Task Logout();
        Task Register(string firstName, string lastName, string userName, string email,
            string passWord, int midWifePhoneNumber, string location);
    }
}
