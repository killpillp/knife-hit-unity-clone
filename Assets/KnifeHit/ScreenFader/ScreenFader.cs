using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class ScreenFader : MonoBehaviour {
    public static ScreenFader instance;
    public const float DURATION = 0.37f;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        GTil.Init(this);
    }

    public void FadeOut(Action onComplete)
    {
        GetComponent<Animator>().SetTrigger("fade_out");
        GetComponent<Image>().enabled = true;
        Timer.Schedule(this, DURATION, () =>
        {
            if (onComplete != null) onComplete();
        });
    }

    public void FadeIn(Action onComplete)
    {
        GetComponent<Animator>().SetTrigger("fade_in");
        Timer.Schedule(this, DURATION, () =>
        {
            GetComponent<Image>().enabled = false;
            if (onComplete != null) onComplete();
        });
    }

    public void GotoScene(int sceneIndex)
    {
        FadeOut(() =>
        {
            CUtils.LoadScene(sceneIndex);
        });
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnLevelFinishedLoading;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelFinishedLoading;
    }

    private void OnLevelFinishedLoading(Scene scene, LoadSceneMode mode)
    { 
        if (GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("ScreenFader_Out"))
        {
            FadeIn(null);
        }
    }
}
