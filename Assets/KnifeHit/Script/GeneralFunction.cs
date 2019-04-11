using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
//Paul Cornel//
public class GeneralFunction : MonoBehaviour {

	public static GeneralFunction intance;
    public Text appleLbl;
	float EscTime;

	void Awake()
	{
		if (intance != null) 
		{
			DestroyImmediate (this.gameObject);
		} else {
			intance = this;
			DontDestroyOnLoad (this.gameObject);

			GeneralFunction.intance.appleLbl.text = GameManager.Apple + "";
		}
	}
	void Update()
	{
		if (Input.GetKeyUp (KeyCode.Escape)) {
				if((Time.time-EscTime) < 3f)
				{
					Application.Quit ();
				}
				else 
				{
					Toast.instance.ShowMessage ("Press again to exit game", 2f);
					EscTime = Time.time;

				}
			}
		}

	public void LoadSceneByName(string sceneName)
	{
		SceneManager.LoadScene (sceneName);
	}
	public void LoadSceneWithLoadingScreen(string sceneName)
	{
        CUtils.LoadScene(1, true);
	}
	
	public string LoadedSceneName
	{
		get{return SceneManager.GetActiveScene().name;}
	}
}

public static class MSExtentionMethos
{ 
	public static Rect getWorldRect(this RectTransform transform)
	{
		Vector2 size = Vector2.Scale(transform.rect.size, transform.lossyScale);
		Rect rect = new Rect(transform.position.x, Screen.height - transform.position.y, size.x, size.y);
		rect.x -= (transform.pivot.x * size.x);
		rect.y -= ((1.0f - transform.pivot.y) * size.y);
		return rect;
	}
	public static string ToOrdinal(this long value)
	{
		string extension = "th";
		long last_digits = value % 100;
		if (last_digits < 11 || last_digits > 13)
		{
			switch (last_digits % 10)
			{
				case 1:
					extension = "st";
					break;
				case 2:
					extension = "nd";
					break;
				case 3:
					extension = "rd";
					break;
			}
		}

		return extension;
	}
	public static string WrapText(this string sentence,int columnWidth)
	{

		string[] words = sentence.Split(' ');

		System.Text.StringBuilder newSentence = new System.Text.StringBuilder();


		string line = "";
		foreach (string word in words)
		{
			if ((line + word).Length > columnWidth)
			{
				newSentence.AppendLine(line);
				line = "";
			}

			line += string.Format("{0} ", word);
		}

		if (line.Length > 0)
			newSentence.Append(line);

		return newSentence.ToString ();
	}
	public static Texture2D AlphaBlend(this Texture2D[] aBottom)
	{
		Texture2D main = aBottom [0];


		for(int j=1;j<aBottom.Length;j++)
		{
			Color[] bData = main.GetPixels();
			Color[] tData = aBottom[j].GetPixels();
		
			int count = bData.Length;
			Color[] rData = new Color[count];

			for(int i = 0; i < count; i++)
			{
				Color B = bData[i];
				Color T = tData[i];
				float srcF = T.a;
				float destF = 1f - T.a;
				float alpha = srcF + destF * B.a;
				Color R = (T * srcF + B * B.a * destF)/alpha;
				R.a = alpha;
				rData[i] = R;
			}

			main.SetPixels(rData);
			main.Apply();
		}
		return main;
	}
}