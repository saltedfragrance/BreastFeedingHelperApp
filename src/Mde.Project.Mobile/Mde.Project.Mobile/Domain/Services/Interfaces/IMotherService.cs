using Mde.Project.Mobile.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Domain.Services.Interfaces
{
    public interface IMotherService
    {
        Task Login(string email, string password);
        Task Logout();
        Task Register(string firstName, string lastName, string userName, string email,
            string passWord, int midWifePhoneNumber, string location);
        Task<Mother> GetMother(Guid id);
        Task<Mother> UpdateMother(Mother mother);
        Task<Mother> CreateMother(Mother mother);
    }
}
