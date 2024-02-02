using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.OS;

using Android.Support.V4.App;
using TaskStackBuilder = Android.Support.V4.App.TaskStackBuilder;
using Android.Content;
using Plant_Monitor.Views;
using Xamarin.Forms;
using Plant_Monitor.Services;

namespace Plant_Monitor.Droid
{
    [Activity(Label = "Plant_Monitor", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize, LaunchMode = LaunchMode.SingleTop)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);

            LoadApplication(new App());

            CreateNotificationFromIntent(Intent);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnNewIntent(Intent intent)
        {
            CreateNotificationFromIntent(intent);
        }

        void CreateNotificationFromIntent(Intent intent)
        {
            if (intent?.Extras != null)
            {
                string title = intent.GetStringExtra(AndroidNotificationManager.TitleKey);
                string message = intent.GetStringExtra(AndroidNotificationManager.MessageKey);
                DependencyService.Get<INotificationManager>().ReceiveNotification(title, message);
            }
        }


        //      static readonly int NOTIFICATION_ID = 1000;
        //      static readonly string CHANNEL_ID = "location_notification";
        //      internal static readonly string COUNT_KEY = "count";

        //      void CreateNotificationChannel()
        //{
        //          if (Build.VERSION.SdkInt < BuildVersionCodes.O)
        //	{
        //              return;
        //	}

        //          var channel_name = "Default";
        //          var description = "Default Channel Description";
        //          var channel = new NotificationChannel(CHANNEL_ID, channel_name, NotificationImportance.Default)
        //          { Description = description };

        //          var notificationManager = (NotificationManager)GetSystemService(NotificationService);
        //          notificationManager.CreateNotificationChannel(channel);
        //}

        //      void OnNotificationSend()
        //{
        //          //var valuesForActivity = new Bundle();
        //          //valuesForActivity.PutInt(COUNT_KEY);

        //          // When the user clicks the notification, SecondActivity will start up.
        //          //var resultIntent = new Intent(this, typeof(SecondActivity));

        //          // Pass some values to SecondActivity:
        //          //resultIntent.PutExtras(valuesForActivity);

        //          // Construct a back stack for cross-task navigation:
        //          //var stackBuilder = TaskStackBuilder.Create(this);
        //          //stackBuilder.AddParentStack(Class.FromType(typeof(SecondActivity)));
        //          //stackBuilder.AddNextIntent(resultIntent);

        //          // Create the PendingIntent with the back stack:
        //          //var resultPendingIntent = stackBuilder.GetPendingIntent(0, (int)PendingIntentFlags.UpdateCurrent);

        //          string message = "Message";

        //          // Build the notification:
        //          var builder = new NotificationCompat.Builder(this, CHANNEL_ID)
        //                        .SetAutoCancel(true) // Dismiss the notification from the notification area when the user clicks on it
        //                        //.SetContentIntent(resultPendingIntent) // Start up this activity when the user clicks the intent.
        //                        .SetContentTitle("Plant Monitor") // Set the title
        //                        //.SetNumber(count) // Display the count in the Content Info
        //                        .SetSmallIcon(Resource.Drawable.icon_about) // This is the icon to display
        //                        .SetContentText(message); // the message to display.

        //          // Finally, publish the notification:
        //          var notificationManager = NotificationManagerCompat.From(this);
        //          notificationManager.Notify(NOTIFICATION_ID, builder.Build());
        //      }

    }
}