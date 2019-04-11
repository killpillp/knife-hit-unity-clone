using UnityEngine;
using System.Collections;

namespace EasyMobile
{
    public static class AndroidNativeShare
    {
        public static void ShareImage(string imagePath, string message, string subject = "")
        {
            #if UNITY_ANDROID && !UNITY_EDITOR
            AndroidJavaObject fileObject = new AndroidJavaObject("java.io.File", imagePath);             
            bool fileExist = fileObject.Call<bool>("exists");

            if (!fileExist)
            {
                Debug.Log("Sharing failed: image file not found.");
                return;
            }

            AndroidJavaClass intentClass = new AndroidJavaClass("android.content.Intent");
            AndroidJavaObject intentObject = new AndroidJavaObject("android.content.Intent");

            intentObject.Call<AndroidJavaObject>("setAction", intentClass.GetStatic<string>("ACTION_SEND"));
            intentObject.Call<AndroidJavaObject>("setType", "image/*");

            // Fill in content
            AndroidJavaClass uriClass = new AndroidJavaClass("android.net.Uri");
            AndroidJavaObject uriObject = uriClass.CallStatic<AndroidJavaObject>("fromFile", fileObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_STREAM"), uriObject);
            intentObject.Call<AndroidJavaObject>("putExtra", intentClass.GetStatic<string>("EXTRA_TEXT"), message);

            AndroidJavaClass unity = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unity.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject jChooser = intentClass.CallStatic<AndroidJavaObject>("createChooser", intentObject, subject);
            currentActivity.Call("startActivity", jChooser);
            #endif
        }
    }
}
