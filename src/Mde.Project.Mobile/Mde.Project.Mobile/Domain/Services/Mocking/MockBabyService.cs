using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Domain.Services.Mocking
{
    public class MockBabyService : IBabyService
    {
        private readonly List<Baby> babies = new List<Baby>
        {

        };
        public async Task CreateBaby(string firstName, double height, double weight, string motherId, string dateOfBirth)
        {
            babies.Add(new Baby { Id = Guid.NewGuid(), FirstName = firstName, Height = height, Weight = weight, MotherId = new Guid(motherId), DateOfBirth = Convert.ToDateTime(dateOfBirth) });
        }
        public async Task DeleteBaby(string id)
        {
            babies.RemoveAll(b => b.Id == new Guid(id));
        }
        public async Task<List<Baby>> GetBabies()
        {
            return await Task.FromResult(babies);
        }

        public async Task<Baby> GetBaby(string id)
        {
            return babies.Where(b => b.Id == new Guid(id)).FirstOrDefault();
        }
        public async Task UpdateBaby(string id, string firstName, string birthDate, double weight, double height)
        {
            Baby baby = babies.Where(b => b.Id == (new Guid(id))).FirstOrDefault();
            await CreateBaby(firstName, height, weight, baby.MotherId.ToString(), birthDate.ToString());
            babies.Remove(baby);
        }
    }
}
