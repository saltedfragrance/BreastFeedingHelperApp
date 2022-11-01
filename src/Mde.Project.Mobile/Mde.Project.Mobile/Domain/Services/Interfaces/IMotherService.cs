using Mde.Project.Mobile.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Domain.Services.Interfaces
{
    public interface IMotherService
    {
        Task<List<Mother>> GetMothers();
        Task<Mother> UpdateMother(Mother mother);
        Task<Mother> CreateMother(Mother mother);
    }
}
