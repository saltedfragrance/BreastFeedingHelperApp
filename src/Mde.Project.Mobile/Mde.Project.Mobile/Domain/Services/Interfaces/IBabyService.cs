using Mde.Project.Mobile.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Domain.Services.Interfaces
{
    public interface IBabyService
    {
        Task<List<Baby>> GetBabies();
        Task CreateBaby(string firstName, double height, double weight, Guid motherId);
        Task UpdateBaby(Baby baby);
        Task DeleteBaby(Baby baby);
    }
}
