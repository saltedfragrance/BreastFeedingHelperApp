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
        public async Task CreateBaby(string firstName, double height, double weight, string motherId)
        {
            babies.Add(new Baby { Id = Guid.NewGuid(), FirstName = firstName, Height = height, Weight = weight, MotherId = new Guid(motherId) });
        }
        public async Task DeleteBaby(string id)
        {
            babies.RemoveAll(b => b.Id == new Guid(id));
        }
        public async Task<List<Baby>> GetBabies()
        {
            return await Task.FromResult(babies);
        }
        public async Task UpdateBaby(string id)
        {
            Baby baby = babies.Where(b => b.Id == (new Guid(id))).FirstOrDefault();
            babies.Remove(baby);
            await CreateBaby(baby.FirstName, baby.Height, baby.Weight, baby.MotherId.ToString());
        }
    }
}
