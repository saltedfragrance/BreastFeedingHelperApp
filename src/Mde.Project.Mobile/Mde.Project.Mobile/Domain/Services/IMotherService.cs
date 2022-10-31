using Mde.Project.Mobile.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Domain.Services
{
    public interface IMotherService
    {
        Task<Mother> GetMother(Guid id);
        Task<Mother> UpdateMother(Mother mother);
        Task<Mother> CreateMother(Mother mother);
    }
}
