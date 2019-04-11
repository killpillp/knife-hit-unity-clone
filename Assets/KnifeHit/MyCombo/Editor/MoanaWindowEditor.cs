using UnityEngine;
using UnityEditor;

public class MoanaWindowEditor
{
    [MenuItem("Moana Games/Clear all playerprefs")]
    static void ClearAllPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
    }

    [MenuItem("Moana Games/Add 1000 apples")]
    static void AddApple()
    {
        GameManager.Apple += 1000;
        PlayerPrefs.Save();
    }

    [MenuItem("Moana Games/Set apple to 0")]
    static void SetBalanceZero()
    {
        GameManager.Apple = 0;
        PlayerPrefs.Save();
    }
}