using GoogleMobileAds.Api;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EasyMobile;
//Paul Cornel//
public class GamePlayManager : MonoBehaviour {

	
	public static GamePlayManager instance;
	[Header("Circle Setting")]
	public Circle[] circlePrefabs;
	public Bosses[] BossPrefabs;

	public Transform circleSpawnPoint;
	[Range(0f,1f)]public float circleWidthByScreen=.5f;

	[Header("Knife Setting")]
	public Knife knifePrefab;
	public Transform KnifeSpawnPoint;
	[Range(0f,1f)]public float knifeHeightByScreen=.1f;

	public GameObject ApplePrefab;
	[Header("UI Object")]
	public Text lblScore;
	public Text lblStage;
	public List<Image> stageIcons;
	public Color stageIconActiveColor;
	public Color stageIconNormalColor;

	[Header("UI Boss")]

	public GameObject bossFightStart;
	public GameObject bossFightEnd;
	public AudioClip[] bossFightStartSounds;
	public AudioClip[] bossFightEndSounds;
	[Header("Ads Show")]
	public GameObject adsShowView;
	public Image adTimerImage;
	public Text adSocreLbl;


	[Header("GameOver Popup")]
	public GameObject gameOverView;
	public Text gameOverSocreLbl,gameOverStageLbl;
	public GameObject newBestScore;
	public AudioClip gameOverSfx;
	[Space(50)]

	public int cLevel = 0;
	public bool isDebug=false;
	string currentBossName="";
	Circle currentCircle;
	Knife currentKnife;
	bool usedAdContinue;
	public int totalSpawnKnife
	{
		get
		{ 
			return _totalSpawnKnife;
		}
		set
		{
			_totalSpawnKnife = value;

		}
	}
	int _totalSpawnKnife;

	void Awake()
	{	
		Application.targetFrameRate = 60;
		QualitySettings.vSyncCount = 0;
		if (instance == null) {
			instance = this;		
		}
	}
	void Start () {
		startGame ();
        CUtils.ShowInterstitialAd();
    }

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
            AdmobController.instance.rewardBasedVideo.OnAdClosed += HandleRewardBasedVideoClosed;
        }
#endif
    }

    bool doneWatchingAd = false;
    public void HandleRewardBasedVideoRewarded(object sender, Reward args)
    {
        if (usedAdContinue)
        {
            doneWatchingAd = true;
            AdShowSucessfully();
        }
    }

    public void HandleRewardBasedVideoClosed(object sender, System.EventArgs args)
    {
        if (usedAdContinue)
        {
            if (doneWatchingAd == false)
            {
                adsShowView.SetActive(false);
                usedAdContinue = false;
                showGameOverPopup();
            }
        }
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
            AdmobController.instance.rewardBasedVideo.OnAdClosed -= HandleRewardBasedVideoClosed;
        }
#endif
    }

    public void startGame()
	{
		GameManager.score = 0;
		GameManager.Stage = 1;
		GameManager.isGameOver = false;
		usedAdContinue = false;
		if (isDebug) {
			GameManager.Stage = cLevel;
		}
		setupGame ();
	}
	public void UpdateLable()
	{
		lblScore.text = GameManager.score+"";
		if (GameManager.Stage % 5 == 0) {
			for (int i = 0; i < stageIcons.Count-1; i++) {
				stageIcons [i].gameObject.SetActive(false);
			}
			stageIcons [stageIcons.Count-1].color = stageIconActiveColor;
			lblStage.color = stageIconActiveColor;
			lblStage.text = currentBossName;
		}
		else {
			lblStage.text = "STAGE "+GameManager.Stage;
			for (int i = 0; i < stageIcons.Count; i++) {
				stageIcons [i].gameObject.SetActive(true);
				stageIcons [i].color = GameManager.Stage % stageIcons.Count <= i ? stageIconNormalColor : stageIconActiveColor;
			}
			lblStage.color = stageIconNormalColor;
		}
	}
	public void setupGame()
	{
		spawnCircle ();
		KnifeCounter.intance.setUpCounter (currentCircle.totalKnife);

		totalSpawnKnife=0;
		StartCoroutine(GenerateKnife ());
	}
	void Update () {
		if (currentKnife == null)
			return;
		if (Input.GetMouseButtonDown (0) && !currentKnife.isFire) {
			KnifeCounter.intance.setHitedKnife (totalSpawnKnife);
			currentKnife.ThrowKnife ();
			StartCoroutine(GenerateKnife ());
		}

	}
	public void spawnCircle()
	{
		GameObject tempCircle;
		if (GameManager.Stage % 5 == 0) {
			Bosses b = BossPrefabs [Random.Range (0, BossPrefabs.Length)];
			tempCircle = Instantiate<Circle> (b.BossPrefab, circleSpawnPoint.position, Quaternion.identity, circleSpawnPoint).gameObject;
			currentBossName = "Boss : " + b.Bossname;
			UpdateLable ();
			OnBossFightStart ();
		} else {
			if (GameManager.Stage > 50) {
				tempCircle = Instantiate<Circle> (circlePrefabs [Random.Range(11,circlePrefabs.Length-1)], circleSpawnPoint.position, Quaternion.identity, circleSpawnPoint).gameObject;
			} else {
				tempCircle = Instantiate<Circle> (circlePrefabs [GameManager.Stage - 1], circleSpawnPoint.position, Quaternion.identity, circleSpawnPoint).gameObject;
			}
		}

		tempCircle.transform.localScale = Vector3.one;
		float circleScale = (GameManager.ScreenWidth * circleWidthByScreen) / tempCircle.GetComponent<SpriteRenderer> ().bounds.size.x;
		tempCircle.transform.localScale = Vector3.one * .2f;
		LeanTween.scale (tempCircle, new Vector3 (circleScale, circleScale, circleScale), .3f).setEaseOutBounce ();
		//tempCircle.transform.localScale = Vector3.one*circleScale;
		currentCircle = tempCircle.GetComponent<Circle>();

	}
	public IEnumerator OnBossFightStart()
	{
		bossFightStart.SetActive (true);
		SoundManager.instance.PlaySingle (bossFightStartSounds[Random.Range(0,bossFightEndSounds.Length-1)],1f);
		yield return new WaitForSeconds (2f);
		bossFightStart.SetActive (false);
		setupGame ();
	}

	public IEnumerator OnBossFightEnd()
	{
		bossFightEnd.SetActive (true);
		SoundManager.instance.PlaySingle (bossFightEndSounds[Random.Range(0,bossFightEndSounds.Length-1)],1f);
		yield return new WaitForSeconds (2f);
		bossFightEnd.SetActive (false);
		setupGame ();
	}
	public IEnumerator GenerateKnife()
	{
		//yield return new WaitForSeconds (0.1f);
		yield return new WaitUntil (() => {
			return KnifeSpawnPoint.childCount==0;
		});
			if (currentCircle.totalKnife > totalSpawnKnife && !GameManager.isGameOver ) {
				totalSpawnKnife++;
				GameObject tempKnife;
				if (GameManager.selectedKnifePrefab == null) {
					tempKnife = Instantiate<Knife> (knifePrefab, new Vector3 (KnifeSpawnPoint.position.x, KnifeSpawnPoint.position.y - 2f, KnifeSpawnPoint.position.z), Quaternion.identity, KnifeSpawnPoint).gameObject;
				} else {
					tempKnife = Instantiate<Knife> (GameManager.selectedKnifePrefab, new Vector3 (KnifeSpawnPoint.position.x, KnifeSpawnPoint.position.y - 2f, KnifeSpawnPoint.position.z), Quaternion.identity, KnifeSpawnPoint).gameObject;
			
				}
				tempKnife.transform.localScale = Vector3.one;
				float knifeScale = (GameManager.ScreenHeight * knifeHeightByScreen) / tempKnife.GetComponent<SpriteRenderer> ().bounds.size.y;
				tempKnife.transform.localScale = Vector3.one * knifeScale;
				LeanTween.moveLocalY (tempKnife, 0, 0.1f);
				tempKnife.name ="Knife"+totalSpawnKnife; 
				currentKnife = tempKnife.GetComponent<Knife> ();
			}

	}
	public void NextLevel()
	{
		Debug.Log ("Next Level");
		if (currentCircle != null) {
			currentCircle.destroyMeAndAllKnives ();
		}
		if (GameManager.Stage % 5 == 0) {
			GameManager.Stage++;
			StartCoroutine (OnBossFightEnd ());

		} else {
			GameManager.Stage++;
			if (GameManager.Stage % 5 == 0) {
				StartCoroutine (OnBossFightStart ());
			} else {
				Invoke ("setupGame", .3f);
			}
		}
	}

	IEnumerator currentShowingAdsPopup;
	public void GameOver()
	{
		GameManager.isGameOver = true;

		if (usedAdContinue || !IsAdAvailable()) {
			showGameOverPopup ();
		} else {
			currentShowingAdsPopup = showAdPopup ();
			StartCoroutine (currentShowingAdsPopup);
		}
	}
	public IEnumerator showAdPopup()
	{
		adsShowView.SetActive (true);
		adSocreLbl.text = GameManager.score+"";
		SoundManager.instance.PlayTimerSound ();
		for (float i=1f; i>0; i-=0.01f) 
		{
			adTimerImage.fillAmount =i;
			yield return new WaitForSeconds (0.1f);
		}
		CancleAdsShow ();
		SoundManager.instance.StopTimerSound ();
	}
	public void OnShowAds()
	{
        doneWatchingAd = false;

        SoundManager.instance.StopTimerSound();
        SoundManager.instance.PlaybtnSfx();
        usedAdContinue = true;
        StopCoroutine(currentShowingAdsPopup);

#if (UNITY_ANDROID || UNITY_IOS) && !UNITY_EDITOR
        AdmobController.instance.ShowRewardBasedVideo();
#else
        HandleRewardBasedVideoRewarded(null, null);
#endif
    }
	public  void AdShowSucessfully()
    {
        adsShowView.SetActive(false);
        totalSpawnKnife--;
		GameManager.isGameOver = false;
		print (currentCircle.hitedKnife.Count);
		print (totalSpawnKnife);
		KnifeCounter.intance.setHitedKnife (totalSpawnKnife);
		if (KnifeSpawnPoint.childCount == 0) {		
			StartCoroutine (GenerateKnife ());
		}
	}
	public  void CancleAdsShow()
	{
		SoundManager.instance.StopTimerSound ();
		SoundManager.instance.PlaybtnSfx ();
		StopCoroutine (currentShowingAdsPopup);
		adsShowView.SetActive (false);
		showGameOverPopup ();
	}
	public void showGameOverPopup()
	{
		gameOverView.SetActive (true);
		gameOverSocreLbl.text = GameManager.score+"";
		gameOverStageLbl.text = "Stage "+GameManager.Stage;

		if (GameManager.score >= GameManager.HighScore) {
			GameManager.HighScore = GameManager.score;
			newBestScore.SetActive (true);
		} else {
			newBestScore.SetActive (false);
		}

        CUtils.ShowInterstitialAd();
	}
	public void OpenShop()
	{
		SoundManager.instance.PlaybtnSfx ();
		KnifeShop.intance.showShop ();
	}
	public void RestartGame()
	{
		SoundManager.instance.PlaybtnSfx ();
		GeneralFunction.intance.LoadSceneByName ("GameScene");
	}
	public void BackToHome()
	{
		SoundManager.instance.PlaybtnSfx ();
		GeneralFunction.intance.LoadSceneByName ("HomeScene");
	}
	public void FBClick()
	{
		SoundManager.instance.PlaybtnSfx ();
        StartCoroutine(CROneStepSharing());
    }
	public void ShareClick()
	{
		SoundManager.instance.PlaybtnSfx ();
        StartCoroutine(CROneStepSharing());
	}
	public void SettingClick()
	{
		SoundManager.instance.PlaybtnSfx ();
		SettingUI.intance.showUI ();
	}

    IEnumerator CROneStepSharing()
    {
        yield return new WaitForEndOfFrame();
        MobileNativeShare.ShareScreenshot("screenshot", "");
    }
}

[System.Serializable]
public class Bosses{
	public string Bossname;
	public Circle BossPrefab;
}
