using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardedVideoButton : MonoBehaviour
{
    private const string ACTION_NAME = "rewarded_video";

    private void OnEnable()
    {
        Timer.Schedule(this, 0.1f, AddEvents);
    }

    private void AddEvents()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (AdmobController.instance.rewardBasedVideo != null)
        {
            AdmobController.instance.rewardBasedVideo.OnAdRewarded += HandleRewardBasedVideoRewarded;
        }
#endif
    }

    public void OnClick()
    {
        if (IsAvailableToShow())
        {
            AdmobController.instance.ShowRewardBasedVideo();
        }
        else if (!IsActionAvailable())
        {
            int remainTime = (int)(GameConfig.instance.rewardedVideoPeriod - CUtils.GetActionDeltaTime(ACTION_NAME));
            Toast.instance.ShowMessage("Please wait " + remainTime + " seconds for the next ad");
        }
        else
        {
            Toast.instance.ShowMessage("Ad is not available now, please wait..");
        }

        Sound.instance.PlayButton();
    }

    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        int amount = GameConfig.instance.rewardedVideoAmount;
        GameManager.Apple += amount;
        Toast.instance.ShowMessage("You've received " + amount + " apples", 2);
        CUtils.SetActionTime(ACTION_NAME);
    }

    public bool IsAvailableToShow()
    {
        return IsActionAvailable() && IsAdAvailable();
    }

    private bool IsActionAvailable()
    {
        return CUtils.IsActionAvailable(ACTION_NAME, GameConfig.instance.rewardedVideoPeriod);
    }

    private bool IsAdAvailable()
    {
        if (AdmobController.instance.rewardBasedVideo == null) return false;
        bool isLoaded = AdmobController.instance.rewardBasedVideo.IsLoaded();
        return isLoaded;
    }

    private void OnDisable()
    {
#if UNITY_ANDROID || UNITY_IOS
        if (AdmobController.instance.rewardBasedVideo != null)
        {
            AdmobController.instance.rewardBasedVideo.OnAdRewarded -= HandleRewardBasedVideoRewarded;
        }
#endif
    }
}
