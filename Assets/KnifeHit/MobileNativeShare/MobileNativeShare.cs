using UnityEngine;
using System.Collections;

#if UNITY_IOS || UNITY_ANDROID
using System.IO;
#endif

namespace EasyMobile
{
    public static class MobileNativeShare
    {
        /// <summary>
        /// Shares the image at the given path via native sharing UI.
        /// </summary>
        /// <param name="imagePath">Image path.</param>
        /// <param name="message">Message.</param>
        /// <param name="subject">Subject.</param>
        public static void ShareImage(string imagePath, string message, string subject = "")
        {
            // Share the screenshot via the native share utility
            #if UNITY_IOS
            iOSNativeShare.ShareImage(imagePath, message, subject);
            #elif UNITY_ANDROID
            AndroidNativeShare.ShareImage(imagePath, message, subject);
            #else
            if (Debug.isDebugBuild)
            Debug.Log("ShareImage: FAIL. Platform not supported");
            #endif
        }

        /// <summary>
        /// Captures the screenshot, saves as an PNG image and then share it
        /// to social networks via the native sharing utility.
        /// Note that this method should be called at the end of the frame
        /// (called within a coroutine after WaitForEndOfFrame())
        /// </summary>
        /// <returns>The filepath to the saved screenshot.</returns>
        /// <param name="filename">Filename to store the PNG image without extension, e.g. "Screenshot" to save the screenshot.</param>
        /// <param name="message">Message.</param>
        /// <param name="subject">Subject.</param>
        public static string ShareScreenshot(string filename, string message, string subject = "")
        {
            return ShareScreenshot(0, 0, Screen.width, Screen.height, filename, message, subject);
        }

        /// <summary>
        /// Captures the specified area of the screen, saves as an PNG image
        /// and then share it to social networks via the native sharing utility.
        /// The parameters specify the area of the screen are in pixels (screen space).
        /// Note that this method should be called at the end of the frame
        /// (called within a coroutine after WaitForEndOfFrame())
        /// </summary>
        /// <returns>The filepath to the saved screenshot.</returns>
        /// <param name="startX">Start x.</param>
        /// <param name="startY">Start y.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="filename">Filename to store the PNG image without extension, e.g. "Screenshot".</param>
        /// <param name="message">Message.</param>
        /// <param name="subject">Subject.</param>
        public static string ShareScreenshot(float startX, float startY, float width, float height, string filename, string message, string subject = "")
        {
            string filepath = SaveScreenshot(startX, startY, width, height, filename);

            // Share the screenshot via the native share utility
            if (filepath != null)
                ShareImage(filepath, message, subject);

            return filepath;
        }

        /// <summary>
        /// Generates a PNG image from the given Texture2D object, saves it to persistentDataPath using
        /// the given filename and then shares the image via the native sharing utility.
        /// </summary>
        /// <returns>The filepath to the saved image.</returns>
        /// <param name="tt">Tt.</param>
        /// <param name="filename">Filename to store the PNG image without extension, e.g. "Screenshot".</param>
        /// <param name="message">Message.</param>
        /// <param name="subject">Subject.</param>
        public static string ShareTexture2D(Texture2D tt, string filename, string message, string subject = "")
        {
            #if UNITY_IOS || UNITY_ANDROID
            // Encode texture into PNG
            byte[] bytes = tt.EncodeToPNG();

            // Save file to disk
            string filepath = Path.Combine(Application.persistentDataPath, filename + ".png");
            File.WriteAllBytes(filepath, bytes);

            // Share the screenshot via the native share utility
            ShareImage(filepath, message, subject);

            return filepath;
            #else
            if (Debug.isDebugBuild)
            Debug.Log("ShareImageByTexture2D: FAIL. Platform not supported");

            return null;
            #endif
        }

        /// <summary>
        /// Captures and saves the screenshot to the persistentDataPath as an PNG image with the given filename.
        /// Note that this method should be called at the end of the frame
        /// (called within a coroutine after WaitForEndOfFrame())
        /// </summary>
        /// <returns>The filepath to the saved screenshot.</returns>
        /// <param name="filename">Filename without extension, e.g. "Screenshot".</param>
        public static string SaveScreenshot(string filename)
        {
            return SaveScreenshot(0, 0, Screen.width, Screen.height, filename);
        }

        /// <summary>
        /// Captures and saves the specified area of the screen to the persistentDataPath with the given filename.
        /// Note that this method should be called at the end of the frame
        /// (called within a coroutine after WaitForEndOfFrame())
        /// </summary>
        /// <returns>The filepath to the saved screenshot.</returns>
        /// <param name="startX">Start x.</param>
        /// <param name="startY">Start y.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        /// <param name="filename">Filename without extension, e.g. "Screenshot".</param>
        public static string SaveScreenshot(float startX, float startY, float width, float height, string filename)
        {
            #if UNITY_IOS || UNITY_ANDROID
            // Take the required portion of the screen
            Texture2D tt = CaptureScreenshot(startX, startY, width, height);

            // Encode texture into PNG
            byte[] bytes = tt.EncodeToPNG();

            // Save file to disk
            string filepath = Path.Combine(Application.persistentDataPath, filename + ".png");
            File.WriteAllBytes(filepath, bytes);

            return filepath;
            #else
            if (Debug.isDebugBuild)
            Debug.Log("SaveScreenshot: FAIL. Platform not supported");

            return null;
            #endif
        }

        /// <summary>
        /// Captures the full screenshot and returns the generated Texture2D object.
        /// Note that this method should be called at the end of the frame
        /// (called within a coroutine after WaitForEndOfFrame())
        /// </summary>
        /// <returns>The full screenshot.</returns>
        public static Texture2D CaptureScreenshot()
        {
            return CaptureScreenshot(0, 0, Screen.width, Screen.height);
        }

        /// <summary>
        /// Captures the specified area of the screen and returns the generated Texture2D object.
        /// Note that this method should be called at the end of the frame
        /// (called within a coroutine after WaitForEndOfFrame())
        /// </summary>
        /// <returns>The screenshot.</returns>
        /// <param name="startX">Start x.</param>
        /// <param name="startY">Start y.</param>
        /// <param name="width">Width.</param>
        /// <param name="height">Height.</param>
        public static Texture2D CaptureScreenshot(float startX, float startY, float width, float height)
        {
            Texture2D tt = new Texture2D((int)width, (int)height, TextureFormat.RGB24, false);
            tt.ReadPixels(new Rect(startX, startY, width, height), 0, 0);
            tt.Apply();

            return tt;
        }
    }
}
