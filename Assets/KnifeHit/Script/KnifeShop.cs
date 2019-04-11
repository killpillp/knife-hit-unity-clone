using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Paul Cornel//
public class KnifeShop : MonoBehaviour {
	public GameObject shopUIParent;
	public ShopItem shopKnifePrefab;
	public Transform shopPageContent;
	public Text unlockKnifeCounterLbl;
	public Button unlockNowBtn,unlockRandomBtn;
	public Image selectedKnifeImageUnlock;
	public Image selectedKnifeImageLock;
	public GameObject knifeBackeffect1,knifeBackeffect2;
	public int UnlockPrice=250,UnlockRandomPrice=250;
	public List<Knife> shopKnifeList;

	public static KnifeShop intance;
	public static ShopItem selectedItem;
	public AudioClip onUnlocksfx,RandomUnlockSfx;
	List<ShopItem> shopItems;
	ShopItem  selectedShopItem
	{
		get
		{ 
			return shopItems.Find ((obj) => { return obj.selected; });
		}
	}
	void Start() 
	{
		if (intance == null) 
		{
			intance = this;
			SetupShop ();
		}
	}
	[ContextMenu("Clear PlayerPref")]
	void ClearPlayerPrefs()
	{
		PlayerPrefs.DeleteAll ();
	}

	[ContextMenu("Add Apple")]
		void addApple()
		{
		GameManager.Apple += 500;
		}
	public void showShop()
	{
		shopUIParent.SetActive (true);
		if (!shopItems [GameManager.SelectedKnifeIndex].selected) {
			shopItems [GameManager.SelectedKnifeIndex].selected = true;
		}
		UpdateUI ();

        CUtils.ShowInterstitialAd();
	}
	void SetupShop ()
	{	
		unlockNowBtn.GetComponentInChildren<Text> ().text = UnlockPrice + "";
		unlockRandomBtn.GetComponentInChildren<Text> ().text = UnlockRandomPrice + "";

		shopItems = new List<ShopItem> ();
		for (int i = 0; i < shopKnifeList.Count; i++) {
			ShopItem temp = Instantiate<ShopItem> (shopKnifePrefab, shopPageContent);
			temp.setup (i, this);
			temp.name = i + "";
			shopItems.Add (temp);
		}

		shopItems [GameManager.SelectedKnifeIndex].OnClick ();
	}
	public void UpdateUI()
	{
		selectedKnifeImageUnlock.sprite = selectedShopItem.knifeImage.sprite;
		selectedKnifeImageLock.sprite = selectedShopItem.knifeImage.sprite;
		selectedKnifeImageUnlock.gameObject.SetActive (selectedShopItem.KnifeUnlock);
		selectedKnifeImageLock.gameObject.SetActive (!selectedShopItem.KnifeUnlock);

		knifeBackeffect1.SetActive (selectedShopItem.KnifeUnlock);
		knifeBackeffect2.SetActive (selectedShopItem.KnifeUnlock);

		int unlockCount = 0;
		if (shopItems.FindAll ((obj) => {	return obj.KnifeUnlock; })!=null) 
		{
			unlockCount = shopItems.FindAll ((obj) => {
				return obj.KnifeUnlock;
			}).Count;
		}
		unlockKnifeCounterLbl.text =	unlockCount+ "/"+shopKnifeList.Count;
		if (unlockCount == shopKnifeList.Count) 
		{
			unlockNowBtn.interactable = false;
			unlockRandomBtn.interactable = false;
		}

		GameManager.selectedKnifePrefab=shopKnifeList[GameManager.SelectedKnifeIndex];
		if (MainMenu.intance != null) {
			MainMenu.intance.selectedKnifeImage.sprite = GameManager.selectedKnifePrefab.GetComponent<SpriteRenderer> ().sprite;
		} 
	}
	public void UnlockKnife()
	{
		if (unlockingRandom)
			return;
		
		if (GameManager.Apple < UnlockPrice) 
		{
            Toast.instance.ShowMessage("Opps! Don't have enough apples");
			SoundManager.instance.PlaybtnSfx ();
			return;
		}
		if (selectedShopItem.KnifeUnlock) 
		{
            Toast.instance.ShowMessage("It's already unlocked!");
			SoundManager.instance.PlaybtnSfx ();
			return;
		}
		GameManager.Apple -= UnlockPrice;
		selectedShopItem.KnifeUnlock = true;
		selectedShopItem.UpdateUIColor ();
		GameManager.SelectedKnifeIndex = selectedShopItem.index;
		UpdateUI ();
		SoundManager.instance.PlaySingle (onUnlocksfx);

	}
	bool unlockingRandom=false;
	public void UnlockRandomKnife()
	{
		if (GameManager.Apple < UnlockRandomPrice) 
		{
            Toast.instance.ShowMessage("Opps! Don't have enough apples");
			SoundManager.instance.PlaybtnSfx ();
			return;
		}
		if(unlockingRandom)
		{
			return;
		}
		StartCoroutine (UnlockRandomCoKnife ());

	}
	IEnumerator UnlockRandomCoKnife()
	{
		unlockingRandom = true;
		List<ShopItem> lockedItems=shopItems.FindAll((obj) => {	return !obj.KnifeUnlock; });
		ShopItem randomSelect=null;
		for (int i = 0; i < lockedItems.Count *2; i++) 
		{
			randomSelect=lockedItems[Random.Range(0,lockedItems.Count)];

			if (!randomSelect.selected) {
				randomSelect.selected = true;
				SoundManager.instance.PlaySingle (RandomUnlockSfx);
			}
			yield return new WaitForSeconds (.2f);
		}

		GameManager.Apple -= UnlockRandomPrice;
		randomSelect.KnifeUnlock = true;
		randomSelect.UpdateUIColor ();
		GameManager.SelectedKnifeIndex = randomSelect.index;
		UpdateUI ();
		unlockingRandom = false;
		SoundManager.instance.PlaySingle (onUnlocksfx);

	}
}
