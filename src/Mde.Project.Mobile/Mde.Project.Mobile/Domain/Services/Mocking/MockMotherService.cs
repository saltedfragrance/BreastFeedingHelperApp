using Mde.Project.Mobile.Domain.Models;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Mde.Project.Mobile.Domain.Services.Mocking
{
    public class MockMotherService : IMotherService
    {
        private readonly static List<Mother> mothers = new List<Mother>
        {
            new Mother
            {
                Id = Guid.Parse(Guid.NewGuid().ToString()),
                Email = "testmother@test.com",
                FirstName = "Angelina",
                LastName = "Jolie",
                MidWifePhoneNumber = 0497554433,
                PassWord = "test",
                UserName = "TestMother"
            }
        };
        public Task<Mother> CreateMother(Mother mother)
        {
            throw new NotImplementedException();
        }

        public async Task<List<Mother>> GetMothers()
        {
            return await Task.FromResult(mothers);
        }

        public Task<Mother> UpdateMother(Mother mother)
        {
            throw new NotImplementedException();
        }
    }
}
