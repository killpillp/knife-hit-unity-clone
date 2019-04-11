using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Paul Cornel//
public class SoundManager : MonoBehaviour {

	public static SoundManager instance;

	public AudioSource efxSource;
	public AudioClip btnSfx;
	public AudioClip timeSfx;
	// Use this for initialization
	void Awake () {

		if (instance == null) {
			instance = this;
			DontDestroyOnLoad (gameObject);
		} else {
			Destroy (gameObject);
		}
		
	}
	public void PlaySingle(AudioClip clip,float vol=1f)
	{
		//Set the clip of our efxSource audio source to the clip passed in as a parameter.
		if (GameManager.Sound && clip !=null) {
			AudioSource.PlayClipAtPoint (clip, Camera.main.transform.position, vol);
		}
		if (clip !=null)
		{

		//	Debug.LogError ("Sound No verible Null 6e");
		}
	}
	public void PlayTimerSound()
	{
		if (GameManager.Sound) {
			efxSource.clip = timeSfx;
			efxSource.Play ();
		}
	}
	public void StopTimerSound(){
		efxSource.Stop ();
		efxSource.clip = null;
	}
	public void PlaybtnSfx(){
		PlaySingle (btnSfx);
	}

	public void playVibrate()
	{
		if(GameManager.Vibration)
			Handheld.Vibrate ();

	}

}
