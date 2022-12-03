using Firebase.Database;
using System;
using System.Collections.Generic;
using System.Text;

namespace Mde.Project.Mobile.Domain.Services.Interfaces
{
    public interface IFireBaseService
    {
        FirebaseClient Client { get; }
    }
}
