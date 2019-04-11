//#define UAS
//#define CHUPA
#define SMA

using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(SellReadMe))]
public class SellReadMeInspector : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("1. Edit Game Settings (Admob, In-app Purchase..)", EditorStyles.boldLabel);

        if (GUILayout.Button("Edit Game Settings", GUILayout.MinHeight(40)))
        {
            Selection.activeObject = AssetDatabase.LoadMainAssetAtPath("Assets/KnifeHit/MyCombo/GameMaster.prefab");
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("2. Game Documentation", EditorStyles.boldLabel);

        if (GUILayout.Button("Open Full Documentation", GUILayout.MinHeight(40)))
        {
            Application.OpenURL("https://drive.google.com/open?id=1Ne1QP0qnoBvDjX2rdUByY08x0Cx1F41hsRER71ayEPk");
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Setup In-app Purchase Guide", GUILayout.MinHeight(40)))
        {
            Application.OpenURL("https://drive.google.com/open?id=1hcB7gxL-DYy12VOA-h78Xl5FshwM7jhRcjzGQnL6BJw");
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Build For iOS Guide", GUILayout.MinHeight(40)))
        {
            Application.OpenURL("https://drive.google.com/open?id=1rkgXuyFlJ2BhyNZkcn5ASuHunNExDwW5ypmFdXcd0uA");
        }

        EditorGUILayout.Space();

        EditorGUILayout.LabelField("3. My Other Great Source Codes", EditorStyles.boldLabel);
        if (GUILayout.Button("Pixel Art - Color by Number", GUILayout.MinHeight(30)))
        {
#if UAS
            Application.OpenURL("https://www.chupamobile.com/unity-arcade/pixel-art-color-by-number-top-free-game-20127");
#elif CHUPA
            Application.OpenURL("https://www.chupamobile.com/unity-arcade/pixel-art-color-by-number-top-free-game-20127");
#elif SMA
            Application.OpenURL("https://www.sellmyapp.com/downloads/pixel-art-color-by-number-top-free-game/");
#endif
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("4. Contact Us For Support", EditorStyles.boldLabel);
        EditorGUILayout.TextField("Email: ", "moana.gamestudio@gmail.com");
    }
}