using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum DialogType
{
    QuitGame,
    Pause,
    Settings,
    YesNo,
    Ok,
    Shop,
    Win,
    RateGame
};

public enum DialogShow
{
    DONT_SHOW_IF_OTHERS_SHOWING,
    REPLACE_CURRENT,
    STACK,
    SHOW_PREVIOUS,
    OVER_CURRENT
};

public class DialogController : MonoBehaviour
{
	public static DialogController instance;

    [HideInInspector]
	public Dialog current;
    [HideInInspector]
    public Dialog[] baseDialogs;

	public Action onDialogsOpened;
	public Action onDialogsClosed;
	public Stack<Dialog> dialogs = new Stack<Dialog>();

	public void Awake()
	{
        instance = this;
    }

	public void ShowDialog(int type)
	{
		ShowDialog((DialogType)type, DialogShow.DONT_SHOW_IF_OTHERS_SHOWING); 
	}

	public void ShowDialog(DialogType type, DialogShow option = DialogShow.REPLACE_CURRENT)
	{
		Dialog dialog = GetDialog(type);
		ShowDialog(dialog, option);
	}

	public void ShowYesNoDialog(string title, string content, Action onYesListener, Action onNoListenter, DialogShow option = DialogShow.REPLACE_CURRENT)
	{
		var dialog = (YesNoDialog)GetDialog(DialogType.YesNo);
        if (dialog.title != null) dialog.title.text = (title);
        if (dialog.message != null) dialog.message.text = (content);
		dialog.onYesClick = onYesListener;
        dialog.onNoClick = onNoListenter;
		ShowDialog(dialog, option);
	}

	public void ShowDialog(Dialog dialog, DialogShow option = DialogShow.REPLACE_CURRENT)
	{
		if (current != null)
		{
			if (option == DialogShow.DONT_SHOW_IF_OTHERS_SHOWING)
			{
				Destroy(dialog.gameObject);
				return;
			} 
            else if (option == DialogShow.REPLACE_CURRENT)
			{
                current.Close();
			} 
            else if (option == DialogShow.STACK)
			{
				current.Hide();
			}
		}

		current = dialog;
		if (option != DialogShow.SHOW_PREVIOUS)
		{
			current.onDialogOpened += OnOneDialogOpened;
			current.onDialogClosed += OnOneDialogClosed;
			dialogs.Push(current);
		}

		current.Show();

		if (onDialogsOpened != null)
			onDialogsOpened();
	}

	public Dialog GetDialog(DialogType type)
	{
        Dialog dialog = baseDialogs[(int)type];
		dialog.dialogType = type;
		return (Dialog)Instantiate(dialog, transform.position, transform.rotation);
	}

	public void CloseCurrentDialog()
	{
		if (current != null)
			current.Close();
	}

    public void CloseDialog(DialogType type)
    {
        if (current == null) return;
        if (current.dialogType == type)
        {
            current.Close();
        }
    }

	public bool IsDialogShowing()
	{
		return current != null;
	}

    public bool IsDialogShowing(DialogType type)
    {
        if (current == null) return false;
        return current.dialogType == type;
    }

	private void OnOneDialogOpened(Dialog dialog)
	{

	}

	private void OnOneDialogClosed(Dialog dialog)
	{
        if (current == dialog)
        {
            current = null;
            dialogs.Pop();
            if (onDialogsClosed != null && dialogs.Count == 0)
                onDialogsClosed();

            if (dialogs.Count > 0)
            {
                ShowDialog(dialogs.Peek(), DialogShow.SHOW_PREVIOUS);
            }
        }
	}

}
