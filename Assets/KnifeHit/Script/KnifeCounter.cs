using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//Paul Cornel//
public class KnifeCounter : MonoBehaviour 
{
	public GameObject knifeIcon;
	public Color activeColor;
	public Color deactiveColor;
	public static KnifeCounter intance;

	List<GameObject> iconList;
	void Awake()
	{
		if (intance == null) {
			intance = this;
			iconList = new List<GameObject> ();
		}
		else
			Destroy (gameObject);
	}
	public void setUpCounter(int totalKnife)
	{
		foreach (var item in iconList) {
			Destroy (item);
		}
		iconList.Clear ();

		for (int i = 0; i < totalKnife; i++) 
		{
			GameObject temp = Instantiate<GameObject> (knifeIcon, transform);
			temp.GetComponent<Image> ().color = activeColor;
			iconList.Add (temp);
		}
	}
	public void  setHitedKnife(int val)
	{
		for (int i = 0; i <iconList.Count; i++) {
			iconList[i].GetComponent<Image> ().color =i<val?deactiveColor:activeColor;
		}
	}
}
