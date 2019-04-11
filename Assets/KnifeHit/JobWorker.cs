using UnityEngine;
using System;

public class JobWorker : MonoBehaviour
{
    public Action<string> onEnterScene;
    public Action onLink2Store;
    public Action onDailyGiftReceived;
    public Action onShowBanner;
    public Action onCloseBanner;
    public Action onShowFixedBanner;
    public Action onShowInterstitial;

    public static JobWorker instance;

    private void Awake()
    {
        instance = this;
    }
}