using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class Toast : MonoBehaviour
{
    public RectTransform backgroundTransform;
    public RectTransform messageTransform;

    public static Toast instance;
    [HideInInspector]
    public bool isShowing = false;

    private Queue<AToast> queue = new Queue<AToast>();

    private class AToast
    {
        public string msg;
        public float time;
        public AToast(string msg, float time)
        {
            this.msg = msg;
            this.time = time;
        }
    }

    private void Awake()
    {
        instance = this;
        SetEnabled(false);
    }

    public void SetMessage(string msg)
    {
        messageTransform.GetComponent<Text>().text = msg;
        Timer.Schedule(this, 0, () =>
        {
            backgroundTransform.sizeDelta = new Vector2(messageTransform.GetComponent<Text>().preferredWidth + 30, backgroundTransform.sizeDelta.y);
        });
    }

    private void Show(AToast aToast)
    {
        SetMessage(aToast.msg);
        SetEnabled(true);
        GetComponent<Animator>().SetBool("show", true);
        Invoke("Hide", aToast.time);
        isShowing = true;
    }

    public void ShowMessage(string msg, float time = 1.5f)
    {
        AToast aToast = new AToast(msg, time);
        queue.Enqueue(aToast);

        ShowOldestToast();
    }

    private void Hide()
    {
        GetComponent<Animator>().SetBool("show", false);
        Invoke("CompleteHiding", 1);
    }

    private void CompleteHiding()
    {
        SetEnabled(false);
        isShowing = false;
        ShowOldestToast();
    }

    private void ShowOldestToast()
    {
        if (queue.Count == 0) return;
        if (isShowing) return;

        AToast current = queue.Dequeue();
        Show(current);
    }

    private void SetEnabled(bool enabled)
    {
        foreach (Transform child in transform)
        {
            child.gameObject.SetActive(enabled);
        }
    }
}
