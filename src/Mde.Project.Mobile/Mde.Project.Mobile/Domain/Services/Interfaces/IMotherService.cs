﻿using Mde.Project.Mobile.Domain.Enums;
using Mde.Project.Mobile.Domain.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace Mde.Project.Mobile.Domain.Services.Interfaces
{
    public interface IMotherService
    {
        Mother CurrentMother { get; set; }
        Task<List<Mother>> GetMothers();
        Task RefreshCurrentMother();
        Task UpdateMother(string id, Mother mother);
        Task CreateMother(string firstName, string lastName, string email, string passWord, int midWifePhoneNumber);
        Task AddEventToTimeLine(string eventMessage, TimeLineCategories messageCategory);
    }
}
