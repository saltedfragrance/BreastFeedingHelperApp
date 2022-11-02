using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Domain.Services.Mocking
{
    public class MockBabyService : IBabyService
    {
        private readonly List<Baby> babies = new List<Baby>
        {
            new Baby{ Id = Guid.NewGuid(), FirstName = "Junior", Height = 50, MotherId = new Guid("572a8007-46c7-44c0-ab7f-7c20d1530a2b"), Weight = 5}
        };
        public async Task CreateBaby(string firstName, int height, int weight, Guid motherId)
        {
            babies.Add(new Baby { Id = Guid.NewGuid(), FirstName = firstName, Height = height, Weight = weight });
        }
        public async Task DeleteBaby(Baby baby)
        {
            babies.Remove(baby);
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
