using Mde.Project.Mobile.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Domain.Services.Interfaces
{
    public interface IBabyService
    {
        Task<Baby> GetBaby(string id);
        Task<List<Baby>> GetBabies();
        Task CreateBaby(string id, string firstName, double height, double weight, string motherId, string dateOfBirth);
        Task UpdateBaby(string id, string firstName, string birthDate, double weight, double height);
        Task DeleteBaby(string id);
    }
}
