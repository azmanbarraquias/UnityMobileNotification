using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Notifications.Android;
using UnityEngine;

// https://docs.unity3d.com/Packages/com.unity.mobile.notifications@1.0/manual/index.html

public class NotificationManager : MonoBehaviour
{
       
    private void Start()
    {
        CreateNotificationChannel();
        SendNotificationM("Hello", "Welcome to unity", DateTime.Now.AddSeconds(1));
        //HandleReceivedNotification();

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();
    }

    public void CreateNotificationChannel()
    {
        /*  Create a notification channel 
         *  
         *  Every local notification must belong to a notification channel. 
         *  Notification channels are only supported on Android 8.0 Oreo and above. 
         *  On older Android versions, this package emulates notification channel behavior.
         *  Settings such as priority (Importance) set for notification channels apply to individual 
         *  notifications even on Android versions prior to 8.0. 
         */


        var channel1 = new AndroidNotificationChannel()
        {
            Id = "jazznotif1", // Channel ID
            Name = "Default Channel", // Channel name
            Description = "Reminds the player to play the game", // Description of this notification
            Importance = Importance.High, // Importance of this notification
            CanBypassDnd = true,
            CanShowBadge = true,
            EnableLights = true,
            EnableVibration = true,
            LockScreenVisibility = LockScreenVisibility.Public,
            VibrationPattern = new long[] { 200, 400, 600 }

        };

        AndroidNotificationCenter.RegisterNotificationChannel(channel1);
    }

    public void SendNotificationM(string title, string content, DateTime fireTime)
    {
        /* Send a simple notification
         * 
         * This example shows you how to schedule a simple text notification and send it to the 
         * notification channel you created in the previous step.
         */

        var notification1 = new AndroidNotification()
        {
            Title = title, // title of this notification
            Text = content, // content of this notification
            FireTime = fireTime,  // date time of this notification shows
            // repeatInterval = TimeSpan
            SmallIcon = "mylogo"
        };

        //notification1.GroupAlertBehaviour = // type, GroupAlertBehaviours
        //notification1.GroupSummary = // type, bool
        //notification1.Group = // type, string
        //notification1.UsesStopwatch = // type, bool
        //notification1.ShouldAutoCancel = // type,bool
        //notification1.Number = // type, int
        notification1.Color = Color.red; // type, Color?
        //notification1.NotificationStyle = // type, NotificationStyle
        //notification1.LargeIcon =  // type, string
        //notification1.RepeatInterval =  // type, TimeSpan?
        //notification1.FireTime =  // type, DateTime
        notification1.SmallIcon = "mylogo"; // type, string
        //notification1.Text =  // type, string
        //notification1.Title = // type, string
        //notification1.SortKey =  // type, string
        //notification1.IntentData =  // type, string



        AndroidNotificationCenter.SendNotification(notification1, "jazznotif1");

        // Avaialbe method from AndroidNotificationCenter
        //public static void CancelAllDisplayedNotifications();
        //public static void CancelAllNotifications();
        //public static void CancelAllScheduledNotifications();
        //public static void CancelDisplayedNotification(int id);
        //public static void CancelNotification(int id);
        //public static void CancelScheduledNotification(int id);
        //public static NotificationStatus CheckScheduledNotificationStatus(int id);
        //public static void DeleteNotificationChannel(string id);
        //public static AndroidNotificationIntentData GetLastNotificationIntent();
        //public static AndroidNotificationChannel GetNotificationChannel(string id);
        //public static AndroidNotificationChannel[] GetNotificationChannels();
        //public static bool Initialize();
        //public static void RegisterNotificationChannel(AndroidNotificationChannel channel);
        //public static int SendNotification(AndroidNotification notification, string channel);
        //public static void SendNotificationWithExplicitID(AndroidNotification notification, string channel, int id);
        //public static void UpdateScheduledNotification(int id, AndroidNotification notification, string channel);
    }

    public void HandleReceivedNotification()
    {
        // Handle received notifications while the app is running

        // You can subscribe to the AndroidNotificationCenter. 
        // OnNotificationReceived event to receive a callback whenever the device receives a 
        // remote notification while your app is running.

        AndroidNotificationCenter.NotificationReceivedCallback receivedNotificationHandler
            = delegate (AndroidNotificationIntentData data)
            {
                var msg = "Notification received : " + data.Id + "\n";
                msg += "\n Notification received: ";
                msg += "\n .Title: " + data.Notification.Title;
                msg += "\n .Body: " + data.Notification.Text;
                msg += "\n .Channel: " + data.Channel;
                Debug.Log(msg);
            };

        AndroidNotificationCenter.OnNotificationReceived += receivedNotificationHandler;
    }

    public void IdentifierAndCheckNotification()
    {
        var notification = new AndroidNotification();
        notification.Title = "SomeTitle";
        notification.Text = "SomeText";
        notification.FireTime = System.DateTime.Now.AddMinutes(5);

        var newNotification = new AndroidNotification();
        notification.Title = "SomeTitle";
        notification.Text = "SomeText";
        notification.FireTime = System.DateTime.Now.AddMinutes(5);

        // Unity assigns a unique identifier to each notification after you schedule it. 
        // You can use the identifier to track the notification status or to cancel it. 
        // Notification status tracking only works on Android 6.0 Marshmallow and above.
        var identifier = AndroidNotificationCenter.SendNotification(notification, "channel_id");

        // Use the following code example to check if your app has delivered the notification to the 
        // device and perform any actions depending on the result.

        if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Scheduled)
        {
            // Replace the currently scheduled notification with a new notification.
            AndroidNotificationCenter.UpdateScheduledNotification(identifier, newNotification, "channel_id");
        }
        else if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Delivered)
        {
            //Remove the notification from the status bar
            AndroidNotificationCenter.CancelNotification(identifier);
        }
        else if (AndroidNotificationCenter.CheckScheduledNotificationStatus(identifier) == NotificationStatus.Unknown)
        {
            AndroidNotificationCenter.SendNotification(newNotification, "channel_id");
        }
    }

    public void SaveCustomDataAndRetrieveFeedback()
    {
        // Save custom data and retrieve it when the user opens the app from the notification

        // To store arbitrary string data in a notification object set the IntentData property.
        var notification = new AndroidNotification();
        notification.IntentData = "{\"title\": \"Notification 1\", \"data\": \"200\"}";
        AndroidNotificationCenter.SendNotification(notification, "channel_id");


        // If the user opens the app from the notification, 
        // you can retrieve it any and any data it has assigned to it like this:

        var notificationIntentData = AndroidNotificationCenter.GetLastNotificationIntent();

        if (notificationIntentData != null)
        {
            var id = notificationIntentData.Id;
            var channel = notificationIntentData.Channel;
            var notification1 = notificationIntentData.Notification;
        }

        // If the app is opened in any other way, GetLastNotificationIntent returns null.
    }

    #region Button's
    public void SendNotificationButton1()
    {
        SendNotificationM("We miss you 5", "Have you played yet? 5", DateTime.Now.AddSeconds(5));
    }


    public void SendNotificationButton2()
    {
        SendNotificationM("We miss you 10", "Have you played yet? 10", DateTime.Now.AddSeconds(10));
    }
    public void SendNotificationButton3()
    {
        SendNotificationM("We miss you 15", "Have you played yet? 15", DateTime.Now.AddSeconds(15));
    }

    #endregion Button's
}
