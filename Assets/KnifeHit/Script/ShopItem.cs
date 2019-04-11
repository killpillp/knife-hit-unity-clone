using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Paul Cornel//
public class ShopItem : MonoBehaviour {

	public int index;
	public Image bgImage;
	public Image knifeImage;
	public GameObject selectIamge;
	public Color unlockKnifeBGColor, lockKnifeBGColor;
	public Color unlockKnifeColor, lockKnifeColor;
	public AudioClip unlockKnifesfx, lockKnifesfx,confirmKnifeSfx;
	public bool KnifeUnlock
	{
		get
		{	
				if (index == 0)
					return true;
				return  PlayerPrefs.GetInt ("KnifeUnlock_" + index, 0) == 1;
		}
		set
		{ 
		
			PlayerPrefs.SetInt ("KnifeUnlock_" + index, value?1:0);
		}
	}
	public bool selected
	{
		get
		{
			return selectIamge.activeSelf;
		}
		set
		{ 
			if (value) {
				if(KnifeShop.selectedItem!=null)
					KnifeShop.selectedItem.selected = false;

				KnifeShop.selectedItem = this;
			}
			selectIamge.SetActive (value);
		}
	}

	KnifeShop shopRef;
	Knife knifeRef;
	public	void setup (int i,KnifeShop shop) 
	{
		shopRef=shop;
		index = i;
		knifeRef = shop.shopKnifeList [index];
		knifeImage.sprite = knifeRef.GetComponent<SpriteRenderer> ().sprite;
		UpdateUIColor ();
	}
	public void OnClick()
	{
		if (KnifeUnlock && selected) {
			shopRef.shopUIParent.SetActive (false);
				SoundManager.instance.PlaySingle (confirmKnifeSfx);
		}
		if (!selected) {
			selected = true;
			if(!KnifeUnlock )
				SoundManager.instance.PlaySingle (lockKnifesfx);
		} 
		if (KnifeUnlock) 
		{
			GameManager.SelectedKnifeIndex = index;
			SoundManager.instance.PlaySingle (unlockKnifesfx);
		}
		shopRef.UpdateUI ();

	}
	public void UpdateUIColor()
	{
		bgImage.color = KnifeUnlock ? unlockKnifeBGColor : lockKnifeBGColor;
		knifeImage.GetComponent<Mask> ().enabled = !KnifeUnlock;

		knifeImage.transform.GetChild(0).GetComponent<Image>().color = KnifeUnlock ? unlockKnifeColor : lockKnifeColor;
		knifeImage.transform.GetChild (0).gameObject.SetActive (!KnifeUnlock);
	}
}
