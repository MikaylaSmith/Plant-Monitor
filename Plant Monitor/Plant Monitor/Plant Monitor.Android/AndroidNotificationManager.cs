/***********
* Class: AndroidNotificationManager
*
* Purpose:
*	The purpose of this class is to manage sending out notifications for the 
*	health status of a particular plant for Android operating systems
*
* Manager Functions:
*	None
*		
* Methods:
*   Initialize()
*       Creates notification channel and makes a singleton instance for the system.
*       
*	SendNotification(string, string)
*		Sends information for notification formatting to the Dependensy Service, which 
*		in turn sends info to either Android notification system. 
*		
*	ReceiveNotification(string, string)
*	    Get a notification and format it.
*	    
*	Show(string, string)
*	    Use NotificationCompat to create notification specifics and then display it to user.
*	    
*	CreateNotificationChannel()
*	    Make a way to deliver the notifications that get sent out. 
*	    
*	GetNotifyTime(DateTime)
*	    Convert into UTC so that times don't have to deal with determining time zone. 
*	    
* Code from Microsoft:
* https://learn.microsoft.com/en-us/xamarin/xamarin-forms/app-fundamentals/local-notifications
*
***********/

using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using System;
using Xamarin.Forms;
using Plant_Monitor.Services;

using AndroidApp = Android.App.Application;
using Android.Graphics;

[assembly: Dependency(typeof(Plant_Monitor.Droid.AndroidNotificationManager))]
namespace Plant_Monitor.Droid
{
    public class AndroidNotificationManager : INotificationManager
    {
        const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";

        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized = false;
        int messageId = 0;
        int pendingIntentId = 0;

        NotificationManager manager;

        public event EventHandler NotificationReceived;

        public static AndroidNotificationManager Instance { get; private set; }

        public AndroidNotificationManager() => Initialize();

        /* Purpose: Create singleton instance for notifications and create the notification channel 
		 * Input: None
		 * Output: Channel created, singleton instance created 
		 */
        public void Initialize()
        {
            if (Instance == null)
            {
                CreateNotificationChannel();
                Instance = this;
            }
        }

        /* Purpose: Pass from interface connection to displaying the notification and its details and formatting
		 * Input: string, string, (optional) DateTime
		 * Output: Notification sends 
		 */
        public void SendNotification(string title, string message, DateTime? notifyTime = null)
        {
            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }

            if (notifyTime != null)
            {
                Intent intent = new Intent(AndroidApp.Context, typeof(AlarmHandler));
                intent.PutExtra(TitleKey, title);
                intent.PutExtra(MessageKey, message);

                PendingIntent pendingIntent = PendingIntent.GetBroadcast(AndroidApp.Context, pendingIntentId++, intent, PendingIntentFlags.CancelCurrent);
                long triggerTime = GetNotifyTime(notifyTime.Value);
                AlarmManager alarmManager = AndroidApp.Context.GetSystemService(Context.AlarmService) as AlarmManager;
                alarmManager.Set(AlarmType.RtcWakeup, triggerTime, pendingIntent);
            }
            else
            {
                Show(title, message);
            }
        }

        /* Purpose: Get Notifications (?)
		 * Input: string, string
		 * Output: Notification invoked.
		 */
        public void ReceiveNotification(string title, string message)
        {
            var args = new NotificationEventArgs()
            {
                Title = title,
                Message = message,
            };
            NotificationReceived?.Invoke(null, args);
        }

        /* Purpose: Format and display the notification 
		 * Input: string, string
		 * Output: Notification displayed.
		 */
        public void Show(string title, string message)
        {
            Intent intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, message);

            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId++, intent, PendingIntentFlags.UpdateCurrent);

            NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(title)
                .SetContentText(message)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.icon_about))
                .SetSmallIcon(Resource.Drawable.icon_about)
                .SetPriority(NotificationCompat.PriorityMax) 
                .SetVisibility((int)NotificationVisibility.Public)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);

            Notification notification = builder.Build();
            manager.Notify(messageId++, notification);
        }

        /* Purpose: Create a channel through which notifications are delivered to the user. 
		 * Input: None
		 * Output: A channel is created for notifications to use. 
		 */
        void CreateNotificationChannel()
        {
            manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Max)
                {
                    Description = channelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }

        /* Purpose: Reformat the time, if one is used, to be in UTC
		 * Input: DateTime
		 * Output: time
		 */
        long GetNotifyTime(DateTime notifyTime)
        {
            DateTime utcTime = TimeZoneInfo.ConvertTimeToUtc(notifyTime);
            double epochDiff = (new DateTime(1970, 1, 1) - DateTime.MinValue).TotalSeconds;
            long utcAlarmTime = utcTime.AddSeconds(-epochDiff).Ticks / 10000;
            return utcAlarmTime;
        }
    }


    /***********
    * Class: AlarmHandler
    *
    * Purpose:
    *	The purpose of this class is to manage sending out notifications at a specific time
    *
    * Manager Functions:
    *   None
    *		
    * Methods:
    *	 OnRecieve(Context, Intent)
    *		Send out notification at specified time.
    *
    ***********/
    [BroadcastReceiver(Enabled = true, Label = "Notifications Broadcast Receiver")]
    public class AlarmHandler : BroadcastReceiver
	{
        /* Purpose: Send out notifications at time 
		 * Input: Context, Intent
		 * Output: Notification sent 
		 */
        public override void OnReceive(Context context, Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.GetStringExtra(AndroidNotificationManager.TitleKey);
                string message = intent.GetStringExtra(AndroidNotificationManager.MessageKey);

                AndroidNotificationManager manager = AndroidNotificationManager.Instance ?? new AndroidNotificationManager();
                manager.Show(title, message);
            }
        }
    }
}