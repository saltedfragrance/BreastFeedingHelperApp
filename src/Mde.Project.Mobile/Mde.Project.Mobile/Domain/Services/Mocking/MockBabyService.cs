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
        public async Task CreateBaby(string firstName, double height, double weight, Guid motherId)
        {
            babies.Add(new Baby { Id = Guid.NewGuid(), FirstName = firstName, Height = height, Weight = weight, MotherId = motherId });
        }
        public async Task DeleteBaby(Guid babyId)
        {
            babies.RemoveAll(b => b.Id == babyId);
        }
        public async Task<List<Baby>> GetBabies()
        {
            return await Task.FromResult(babies);
        }
        public async Task UpdateBaby(Baby baby)
        {
            babies.Remove(baby);
            await CreateBaby(baby.FirstName, baby.Height, baby.Weight, baby.Id);
        }
    }
}
