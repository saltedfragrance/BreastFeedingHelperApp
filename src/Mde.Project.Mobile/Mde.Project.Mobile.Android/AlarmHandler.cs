using Android.Content;
using FreshMvvm;
using Mde.Project.Mobile.Domain.Services.Interfaces;
using Mde.Project.Mobile.Droid;
using static Android.Icu.Text.CaseMap;

[BroadcastReceiver(Enabled = true, Label = "Local Notifications Broadcast Receiver")]
public class AlarmHandler : BroadcastReceiver
{
    public override void OnReceive(Context context, Intent intent)
    {
        if (intent?.Extras != null)
        {
            string id = intent.Extras.GetString(AndroidNotificationManager.IdKey);
            string title = intent.GetStringExtra(AndroidNotificationManager.TitleKey);
            string message = intent.GetStringExtra(AndroidNotificationManager.MessageKey);

            AndroidNotificationManager manager = AndroidNotificationManager.Instance ?? new AndroidNotificationManager();
            manager.Show(id, title, message);
        }
    }
}