using UnityEngine;
using System.Collections;

public class NotificationManager : MonoBehaviour {


	void OnApplicationPause(bool status)
	{
		if(status) //paused
		{
			
			#if UNITY_ANDROID
			try{
				
				using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
				{
					using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) 
					{
						//obj_Activity.Call("CancelNotification",11223311);
						obj_Activity.Call("SendNotification","86400","Collect your daily reward and upgrade your plane!",11223312); //ovo da se otkomentarise
//						obj_Activity.Call("SendNotification","20","Collect your daily reward and upgrade your plane!",11223312); //test brojke
						//obj_Activity.Call("SendNotification","432000","Come back and smash some evil baboons!",11223313);
						obj_Activity.Call("SendNotification","432000","Help commander Ironpaw defeat evil goblins!",11223313); //ovo da se otkomentarise
						//						obj_Activity.Call("SendNotification","50","Help commander Ironpaw defeat evil goblins!",11223313); //test brojke
					}
				}
				
				if(!PlayerPrefs.HasKey("LikePanda") || !PlayerPrefs.HasKey("LikeWebelinx"))
				{
					using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
					{
						using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) 
						{
							obj_Activity.Call("SendNotification","259200","Visit our pages and earn extra stars",11223311);
							//							obj_Activity.Call("SendNotification","80","Visit our pages and earn extra stars",11223311); //test brojke
						}
					}
				}
				
				
			}
			catch{}
			#elif UNITY_IPHONE
			LocalNotification notif = new LocalNotification();
			if(!PlayerPrefs.HasKey("LikePanda") || !PlayerPrefs.HasKey("LikeWebelinx"))
			{
				notif.alertBody = "Visit our pages and earn extra stars";
				notif.fireDate = System.DateTime.Now.AddSeconds(259200);
				//notif.userInfo.Add("code",11223311);
				NotificationServices.ScheduleLocalNotification(notif);
			}
			notif.alertBody = "Collect your daily reward and upgrade your plane!";
			notif.fireDate = System.DateTime.Now.AddSeconds(86400);
			Debug.Log("NOTIF: " + notif.alertBody);
			//notif.userInfo.Add("code",11223312);
			NotificationServices.ScheduleLocalNotification(notif);
			
			notif.alertBody = "Help commander Ironpaw defeat evil goblins!";
			notif.fireDate = System.DateTime.Now.AddSeconds(432000);
			//notif.userInfo.Add("code",11223313);
			NotificationServices.ScheduleLocalNotification(notif);
			#endif
		}
		else
		{
			#if UNITY_ANDROID
			try{
				using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
				{
					using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) 
					{
						obj_Activity.Call("CancelNotification",11223311);
						obj_Activity.Call("CancelNotification",11223312);
						obj_Activity.Call("CancelNotification",11223313);
					}
				}
			}
			catch{}
			#elif UNITY_IPHONE
			Debug.Log("treba da otkaze prvu: ");
			CancelLocalNotification("Visit our pages and earn extra stars");
			Debug.Log("treba da otkaze drugu: ");
			CancelLocalNotification("Collect your daily reward and upgrade your plane!");
			Debug.Log("treba da otkaze trecu: ");
			CancelLocalNotification("Help commander Ironpaw defeat evil goblins!");
			#endif
		}
	}

	#if UNITY_IPHONE
	void CancelLocalNotification(string body)
	{
		Debug.Log("Notifikacija ima: " + NotificationServices.scheduledLocalNotifications.Length);
		LocalNotification[] niz = NotificationServices.scheduledLocalNotifications;
		
		foreach(LocalNotification notif in niz)
		{
			//Debug.Log("otkazivanje: " + notif.userInfo["code"].ToString());
			if(notif.alertBody == body)
				NotificationServices.CancelLocalNotification(notif);
		}
	}
	#endif

	void OnApplicationQuit()
	{
		#if UNITY_ANDROID
		try{
			
			using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
			{
				using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) 
				{
					//obj_Activity.Call("CancelNotification",11223311);
					obj_Activity.Call("SendNotification","86400","Collect your daily reward and upgrade your plane!",11223312); //ovo da se otkomentarise
					//						obj_Activity.Call("SendNotification","20","Collect your daily reward and upgrade your plane!",11223312); //test brojke
					//obj_Activity.Call("SendNotification","432000","Come back and smash some evil baboons!",11223313);
					obj_Activity.Call("SendNotification","432000","Help commander Ironpaw defeat evil goblins!",11223313); //ovo da se otkomentarise
					//						obj_Activity.Call("SendNotification","50","Help commander Ironpaw defeat evil goblins!",11223313); //test brojke
				}
			}
			
			if(!PlayerPrefs.HasKey("LikePanda") || !PlayerPrefs.HasKey("LikeWebelinx"))
			{
				using (AndroidJavaClass cls_UnityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer")) 
				{
					using (AndroidJavaObject obj_Activity = cls_UnityPlayer.GetStatic<AndroidJavaObject>("currentActivity")) 
					{
						obj_Activity.Call("SendNotification","259200","Visit our pages and earn extra stars",11223311);
						//							obj_Activity.Call("SendNotification","80","Visit our pages and earn extra stars",11223311); //test brojke
					}
				}
			}
			
			
		}
		catch{}
		#elif UNITY_IPHONE
		LocalNotification notif = new LocalNotification();
		if(!PlayerPrefs.HasKey("LikePanda") || !PlayerPrefs.HasKey("LikeWebelinx"))
		{
			notif.alertBody = "Visit our pages and earn extra stars";
			notif.fireDate = System.DateTime.Now.AddSeconds(259200);
			//notif.userInfo.Add("code",11223311);
			NotificationServices.ScheduleLocalNotification(notif);
		}
		notif.alertBody = "Collect your daily reward and upgrade your plane!";
		notif.fireDate = System.DateTime.Now.AddSeconds(86400);
		Debug.Log("NOTIF: " + notif.alertBody);
		//notif.userInfo.Add("code",11223312);
		NotificationServices.ScheduleLocalNotification(notif);
		
		notif.alertBody = "Help commander Ironpaw defeat evil goblins!";
		notif.fireDate = System.DateTime.Now.AddSeconds(432000);
		//notif.userInfo.Add("code",11223313);
		NotificationServices.ScheduleLocalNotification(notif);
		#endif
	}
}
