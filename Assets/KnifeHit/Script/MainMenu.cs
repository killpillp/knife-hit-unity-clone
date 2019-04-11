using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
//Paul Cornel//
public class MainMenu : MonoBehaviour 
{
	[Header("Main View")]
	public Button giftButton;
	public Text giftLable;
	public CanvasGroup giftLableCanvasGroup;
	public GameObject giftBlackScreen;
	public GameObject giftParticle;
	public Image selectedKnifeImage;
	public AudioClip giftSfx;

	public static MainMenu intance;

	// Gift Setting

	int timeForNextGift = 60*8;
	int minGiftApple = 40;// Minimum Apple for Gift
	int maxGiftApple = 70;// Maxmum Apple for Gift
	void Awake()
	{
		intance = this;
	}
	void Start()
	{
        CUtils.ShowInterstitialAd();
		InvokeRepeating ("updateGiftStatus", 0f, 1f);
		KnifeShop.intance.UpdateUI ();
	}

	public void OnPlayClick()
	{
		SoundManager.instance.PlaybtnSfx ();
		GeneralFunction.intance.LoadSceneWithLoadingScreen ("GameScene");
	}
	public void RateGame()
	{
		SoundManager.instance.PlaybtnSfx ();
        CUtils.OpenStore();
	}

	void updateGiftStatus()
	{
		if (GameManager.GiftAvalible) {
			giftButton.interactable = true;
			LeanTween.alphaCanvas (giftLableCanvasGroup, 0f, .4f).setOnComplete (() => {
				LeanTween.alphaCanvas (giftLableCanvasGroup, 1f, .4f);
			});
			giftLable.text="READY!";
		} else {
			giftButton.interactable = false;
			giftLable.text = GameManager.RemendingTimeSpanForGift.Hours.ToString("00")+":"+
				GameManager.RemendingTimeSpanForGift.Minutes.ToString("00")+":"+
				GameManager.RemendingTimeSpanForGift.Seconds.ToString("00");
		}
	}
	[ContextMenu("Get Gift")]
	public void OnGiftClick()
	{
		SoundManager.instance.PlaybtnSfx ();
		int Gift = UnityEngine.Random.Range (minGiftApple, maxGiftApple);
        Toast.instance.ShowMessage("You got "+Gift+" Apples");
		GameManager.Apple += Gift;
		GameManager.NextGiftTime = DateTime.Now.AddMinutes(timeForNextGift);

        updateGiftStatus ();
		giftBlackScreen.SetActive (true);
		Instantiate<GameObject>(giftParticle);
		SoundManager.instance.PlaySingle (giftSfx);
		Invoke("HideGiftParticle",2f);
	}
	public void HideGiftParticle()
	{
		giftBlackScreen.SetActive (false);
	}
	public void OpenShopUI()
	{
		SoundManager.instance.PlaybtnSfx ();
		KnifeShop.intance.showShop ();	
	}
	public void OpenSettingUI()
	{
		SoundManager.instance.PlaybtnSfx ();
		SettingUI.intance.showUI();	
	}
}

