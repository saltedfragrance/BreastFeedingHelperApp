using System;
using System.Collections.Generic;
using System.Text;

namespace Mde.Project.Mobile.Domain.Services.Interfaces
{
    public interface INotificationManager
    {
        event EventHandler NotificationReceived;
        void Initialize();
        void ScheduleNotification(string id, string title, string message, string firstTriggerTime, string intervalTime);
        void ReceiveNotification(string title, string message);
        void CancelNotification(string id, string title, string message);
    }
}
