using System;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using AndroidX.Core.App;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Domain;
using Mde.Project.Mobile.Droid;
using Xamarin.Forms;
using AndroidApp = Android.App.Application;

[assembly: Dependency(typeof(Mde.Project.Mobile.Droid.AndroidNotificationManager))]
namespace Mde.Project.Mobile.Droid
{
    public class AndroidNotificationManager : INotificationManager
    {
        const string channelId = "1";
        const string channelName = "Reminders";
        const string channelDescription = "Reminders for feeding and pumping";

        public const string IdKey = "id";
        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized = false;
        int messageId = 0;

        NotificationManager manager;

        public event EventHandler NotificationReceived;

        public static AndroidNotificationManager Instance { get; private set; }

        public AndroidNotificationManager()
        {
            Initialize();
        }

        public void Initialize()
        {
            if (Instance == null)
            {
                CreateNotificationChannel();
                Instance = this;
            }
        }

        public void ScheduleNotification(string id, string title, string message, string firstTriggerTime, string intervalTime)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            long interval = long.Parse(intervalTime) * 60000;
            Intent intent = new Intent(AndroidApp.Context, typeof(AlarmHandler));
            intent.PutExtra(IdKey, id);
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            PendingIntent pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, 0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);
            AlarmManager alarmManager = AndroidApp.Context.GetSystemService(Context.AlarmService) as AlarmManager;
            alarmManager.SetRepeating(AlarmType.RtcWakeup, long.Parse(firstTriggerTime), interval, pendingIntent);

        }

        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
        }

        void CreateNotificationChannel()
        {
            manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription,
                    LockscreenVisibility = NotificationVisibility.Public,
                    Importance = NotificationImportance.Max,
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }

        public void Show(string id, string title, string message)
        {
            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, id);
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, 0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetFullScreenIntent(pendingIntent, true)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetShowWhen(true)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.babydrinkingbottle))
                .SetSmallIcon(Resource.Drawable.babydrinkingbottle)
                .SetVisibility((int)NotificationVisibility.Public)
                .SetStyle(new NotificationCompat.BigTextStyle().BigText(message))
                .SetDefaults(NotificationCompat.DefaultVibrate | NotificationCompat.DefaultSound)
                .SetPriority((int)NotificationPriority.Max);

            Notification notification = builder.Build();
            manager.Notify(messageId++, notification);
        }

        public void CancelNotification(string id, string title, string message)
        {
            Intent intent = new Intent(AndroidApp.Context, typeof(AlarmHandler));
            intent.PutExtra(IdKey, id);
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            AlarmManager alarmManager = AndroidApp.Context.GetSystemService(Context.AlarmService) as AlarmManager;
            PendingIntent pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, 0, intent, PendingIntentFlags.UpdateCurrent | PendingIntentFlags.Immutable);
            alarmManager.Cancel(pendingIntent);
        }
    }
}