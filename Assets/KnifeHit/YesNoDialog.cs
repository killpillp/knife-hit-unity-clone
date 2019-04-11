using UnityEngine;
using System.Collections;
using System;

public class YesNoDialog : Dialog{
    public Action onYesClick;
    public Action onNoClick;
    public virtual void OnYesClick()
    {
        if (onYesClick != null) onYesClick();
        Sound.instance.PlayButton();
        Close();
    }

    public virtual void OnNoClick()
    {
        if (onNoClick != null) onNoClick();
        Sound.instance.PlayButton();
        Close();
    }
}
