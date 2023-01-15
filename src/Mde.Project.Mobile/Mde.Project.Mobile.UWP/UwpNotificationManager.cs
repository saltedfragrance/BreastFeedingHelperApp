using Mde.Project.Mobile.Domain.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.ApplicationModel.Background;

[assembly: Dependency(typeof(Mde.Project.Mobile.UWP.UwpNotificationManager))]
namespace Mde.Project.Mobile.UWP
{
    public class UwpNotificationManager : INotificationManager
    {
        public event EventHandler NotificationReceived;

        public void CancelNotification(string id, string title, string message)
        {
            throw new NotImplementedException();
        }

        public void Initialize()
        {
            throw new NotImplementedException();
        }

        public void ReceiveNotification(string title, string message)
        {
            throw new NotImplementedException();
        }

        public void ScheduleNotification(string id, string title, string message, string firstTriggerTime, string intervalTime)
        {
            new ToastContentBuilder()
            .AddArgument("title", "message")
            .AddText(title)
            .AddText(message)
            .Schedule(DateTime.Now.AddMinutes(double.Parse(intervalTime)));


            
        }

    }
}
