using UnityEngine.SceneManagement;
using System;
using UnityEngine;

public class CUtils
{
    public static void OpenStore()
    {
#if UNITY_EDITOR || UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + GameConfig.instance.androidPackageID);
#elif UNITY_IPHONE
		Application.OpenURL("https://itunes.apple.com/app/id" + GameConfig.instance.iosAppID);
#elif UNITY_STANDALONE_OSX
		Application.OpenURL("https://itunes.apple.com/app/id" + GameConfig.instance.macAppID);
#elif UNITY_WSA
        if (JobWorker.instance.onLink2Store != null) JobWorker.instance.onLink2Store();
#endif
    }

    public static void OpenStore(string id)
    {
#if UNITY_EDITOR || UNITY_ANDROID
        Application.OpenURL("https://play.google.com/store/apps/details?id=" + id);
#elif UNITY_IPHONE
		Application.OpenURL("https://itunes.apple.com/app/id" + id);
#elif UNITY_STANDALONE_OSX
		Application.OpenURL("https://itunes.apple.com/app/id" + id);
#elif UNITY_WSA
        if (JobWorker.instance.onLink2Store != null) JobWorker.instance.onLink2Store();
#endif
    }

    public static void LikeFacebookPage(string faceID)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
	    Application.OpenURL("fb://page/" + faceID);
#else
        Application.OpenURL("https://www.facebook.com/" + faceID);
#endif
        SetLikeFbPage(faceID);
    }

    public static void SetBuyItem()
    {
        SetBool("buy_item", true);
    }

    public static void SetRemoveAds(bool value)
    {
        SetBool("remove_ads", value);
    }

    public static bool IsAdsRemoved()
    {
        return GetBool("remove_ads");
    }

    public static bool IsBuyItem()
    {
        return GetBool("buy_item", false);
    }

    public static void SetRateGame()
    {
        SetBool("rate_game", true);
    }

    public static bool IsGameRated()
    {
        return GetBool("rate_game", false);
    }

    public static void SetLikeFbPage(string id)
    {
        SetBool("like_page_" + id, true);
    }

    public static bool IsLikedFbPage(string id)
    {
        return GetBool("like_page_" + id, false);
    }

#region Double
    public static void SetDouble(string key, double value)
    {
        PlayerPrefs.SetString(key, DoubleToString(value));
    }

    public static double GetDouble(string key, double defaultValue)
    {
        string defaultVal = DoubleToString(defaultValue);
        return StringToDouble(PlayerPrefs.GetString(key, defaultVal));
    }

    public static double GetDouble(string key)
    {
        return GetDouble(key, 0d);
    }

    private static string DoubleToString(double target)
    {
        return target.ToString("R");
    }

    private static double StringToDouble(string target)
    {
        if (string.IsNullOrEmpty(target))
            return 0d;

        return double.Parse(target);
    }
#endregion

#region Bool
    public static void SetBool(string key, bool value)
    {
        PlayerPrefs.SetInt(key, value ? 1 : 0);
    }

    public static bool GetBool(string key, bool defaultValue = false)
    {
        int defaultVal = defaultValue ? 1 : 0;
        return PlayerPrefs.GetInt(key, defaultVal) == 1;
    }
#endregion


    public static double GetCurrentTime()
    {
        TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
        return span.TotalSeconds;
    }

    public static double GetCurrentTimeInDays()
    {
        TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
        return span.TotalDays;
    }

    public static double GetCurrentTimeInMills()
    {
        TimeSpan span = DateTime.Now.Subtract(new DateTime(1970, 1, 1, 0, 0, 0));
        return span.TotalMilliseconds;
    }

    public static T GetRandom<T>(params T[] arr)
    {
        return arr[UnityEngine.Random.Range(0, arr.Length)];
    }

    public static bool IsActionAvailable(String action, int time, bool availableFirstTime = true)
    {
        if (!PlayerPrefs.HasKey(action + "_time")) // First time.
        {
            if (availableFirstTime == false)
            {
                SetActionTime(action);
            }
            return availableFirstTime;
        }

        int delta = (int)(GetCurrentTime() - GetActionTime(action));
        return delta >= time;
    }

    public static double GetActionDeltaTime(String action)
    {
        if (GetActionTime(action) == 0)
            return 0;
        return GetCurrentTime() - GetActionTime(action);
    }

    public static void SetActionTime(String action)
    {
        SetDouble(action + "_time", GetCurrentTime());
    }

    public static void SetActionTime(String action, double time)
    {
        SetDouble(action + "_time", time);
    }

    public static double GetActionTime(String action)
    {
        return GetDouble(action + "_time");
    }

    public static void ShowInterstitialAd()
    {
        if (IsActionAvailable("show_ads", GameConfig.instance.adPeriod))
        {
#if UNITY_ANDROID || UNITY_IPHONE
            bool result = AdmobController.instance.ShowInterstitial();
            if (result == false) AdmobController.instance.RequestInterstitial();
            if (result) SetActionTime("show_ads");
#else
            if (JobWorker.instance.onShowInterstitial != null)
            {
                JobWorker.instance.onShowInterstitial();
                SetActionTime("show_ads");
            }
#endif
        }
    }

    public static void LoadScene(int sceneIndex, bool useScreenFader = false)
    {
        if (useScreenFader)
        {
            ScreenFader.instance.GotoScene(sceneIndex);
        }
        else
        {
            SceneManager.LoadScene(sceneIndex);
        }
    }

    public static void ReloadScene(bool useScreenFader = false)
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        LoadScene(currentIndex, useScreenFader);
    }
}
