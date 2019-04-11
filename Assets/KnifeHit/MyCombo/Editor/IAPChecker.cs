#pragma warning disable 0162 // code unreached.
#pragma warning disable 0168 // variable declared but not used.
#pragma warning disable 0219 // variable assigned but not used.
#pragma warning disable 0414 // private field assigned but not used.
#pragma warning disable 0618 // obslolete
#pragma warning disable 0108 
#pragma warning disable 0649 //never used
#pragma warning disable 0429 //never used

using UnityEngine;
using UnityEditor;
using System.IO;

[InitializeOnLoad]
public class IAPChecker : EditorWindow 
{
	private const string IAP = "IAP";
    private static IAPChecker Instance;

	public static void OpenWelcomeWindow()
	{
		var window = GetWindow<IAPChecker>(true);
        window.position = new Rect(700, 400, 380, 200);
    }

    public static bool IsOpen
    {
        get { return Instance != null; }
    }

    static IAPChecker()
	{
	}

	//call from Autorun
	public static void OpenPopupAdmobStartup()
	{
		EditorApplication.update += CheckItNow;
	}

	public static void CheckItNow()
	{
		if (Directory.Exists ("Assets/Plugins/UnityPurchasing/Bin"))
		{
			SetScriptingDefineSymbols ();

            if (Instance != null)
            {
                Instance.Close();
            }
		}
		else
		{ 
			OpenWelcomeWindow();
		}

		EditorApplication.update -= CheckItNow; 
	}

	static void SetScriptingDefineSymbols () 
	{
		SetSymbolsForTarget (BuildTargetGroup.Android, IAP);
		SetSymbolsForTarget (BuildTargetGroup.iOS, IAP); 
		SetSymbolsForTarget (BuildTargetGroup.WSA, IAP);
		SetSymbolsForTarget (BuildTargetGroup.Standalone, IAP);
		SetSymbolsForTarget (BuildTargetGroup.Tizen, IAP);
		SetSymbolsForTarget (BuildTargetGroup.tvOS, IAP);
		SetSymbolsForTarget (BuildTargetGroup.WebGL, IAP);
	}

	public void OnGUI()
    {
        GUILayoutUtility.GetRect(position.width, 50);

        GUI.skin.label.wordWrap = true;
        GUI.skin.label.alignment = TextAnchor.MiddleLeft;
        GUILayout.Label("1. Open Window->Services\n",  GUILayout.MaxWidth(350));
        GUILayout.Label("2. Create a Unity Project ID (if required)\n", GUILayout.MaxWidth(350));
        GUILayout.Label("3. Enable in-app purchasing & follow the instruction\n", GUILayout.MaxWidth(350));
        GUILayout.Label("4. Hit Import button and then Install Now the plugin", GUILayout.MaxWidth(350));
    }

    static void SetSymbolsForTarget(BuildTargetGroup target, string scriptingSymbol)
	{
		var s = PlayerSettings.GetScriptingDefineSymbolsForGroup(target);

		string sTemp = scriptingSymbol;

		if(!s.Contains(sTemp))
		{

			s = s.Replace(scriptingSymbol + ";","");

			s = s.Replace(scriptingSymbol,"");  

			s = scriptingSymbol + ";" + s;

			PlayerSettings.SetScriptingDefineSymbolsForGroup(target,s);
		}
	}

	void OnEnable()
	{
        Instance = this;
#if UNITY_5_3_OR_NEWER
        titleContent = new GUIContent("Please import Unity IAP to use this asset");
#endif
	}	
}
