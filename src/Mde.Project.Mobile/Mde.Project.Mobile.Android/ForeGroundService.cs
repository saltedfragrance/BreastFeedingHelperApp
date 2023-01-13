using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using Mde.Project.Mobile.Droid;

[Service]
public class ForegroundService : Service
{
    public const string NOTIFICATION_CHANNEL_ID = "default";
    private readonly int NOTIFICATION_ID = 1000;
    private readonly string NOTIFICATION_CHANNEL_NAME = "Default";
    private void startForegroundService()
    {
        var notificationManager = GetSystemService(Context.NotificationService) as NotificationManager;
        var intent = new Intent(this, typeof(MainActivity));
        PendingIntent pendingIntent = PendingIntent.GetActivity(this, 1, intent, PendingIntentFlags.OneShot);

        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
        {
            createNotificationChannel(notificationManager);
        }
        var notification = new NotificationCompat.Builder(this, NOTIFICATION_CHANNEL_ID);
        notification.SetAutoCancel(false);
        notification.SetSmallIcon(Resource.Drawable.xamagonBlue);
        notification.SetContentTitle("This is notification title");
        notification.SetContentText("This is notification message");
        notification.SetContentIntent(pendingIntent);
        StartForeground(NOTIFICATION_ID, notification.Build());
    }

    private void createNotificationChannel(NotificationManager notificationMnaManager)
    {
        var channel = new NotificationChannel(NOTIFICATION_CHANNEL_ID, NOTIFICATION_CHANNEL_NAME,
            NotificationImportance.Low);
        notificationMnaManager.CreateNotificationChannel(channel);
    }

    public override IBinder OnBind(Intent intent)
    {
        return null;
    }

    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
    {
        startForegroundService();
        return StartCommandResult.RedeliverIntent;
    }
    public override void OnDestroy()
    {
        base.OnDestroy();
    }
}