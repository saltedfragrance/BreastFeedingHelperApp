using System;
using System.Collections.Generic;
using System.Text;

namespace Mde.Project.Mobile.Domain.Services.Interfaces
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;
        void Initialize();
        void SendNotification(string title, string message, string firstTriggerTime, string intervalTime);
        void ReceiveNotification(string title, string message);
    }
}
