using System.Collections;
using System;
using System.Collections.Generic;
using Assets.SimpleAndroidNotifications;
using UnityEngine;

public class Notify : MonoBehaviour
{
    DateTime timeToNotify;
    private string title = "Come and play";
    private string content = "Play now and get 5 Gems";
    private void Start()
    {
        timeToNotify = DateTime.Now.AddDays(1);
    }
    
    private void OnApplicationPause(bool pause)
    {
#if UNITY_ANDROID
        NotificationManager.CancelAll();
        if (pause)
        {
            DateTime timeToNotify2 = DateTime.Now.AddSeconds(10);
            TimeSpan time = timeToNotify - DateTime.Now;
            TimeSpan time2 = timeToNotify2 - DateTime.Now;
            NotificationManager.SendWithAppIcon(time, title, content, Color.blue, NotificationIcon.Bell);
            NotificationManager.SendWithAppIcon(time2, title, content, Color.blue, NotificationIcon.Message);
        }
#endif
    }
}
