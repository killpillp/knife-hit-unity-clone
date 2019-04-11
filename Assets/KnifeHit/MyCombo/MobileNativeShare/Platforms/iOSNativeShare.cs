using UnityEngine;
using System.Collections;

#if UNITY_IOS && !UNITY_EDITOR
using System.Runtime.InteropServices;
#endif

namespace EasyMobile
{
    public static class iOSNativeShare
    {
        public struct ShareData
        {
            public string text;
            public string url;
            public string image;
            public string subject;
        }

        #if UNITY_IOS && !UNITY_EDITOR
        [DllImport("__Internal")]
        private static extern void _ShowShareView(ref ShareData conf);
        #endif

        public static void ShareImage(string imagePath, string message, string subject = "")
        {
            #if UNITY_IOS && !UNITY_EDITOR
            ShareData conf = new ShareData();
            conf.text = message; 
            conf.url = "";
            conf.image = imagePath;
            conf.subject = subject;

            _ShowShareView(ref conf);
            #endif
        }
    }
}

