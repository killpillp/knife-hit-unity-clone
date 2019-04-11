using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Sound))]
[CanEditMultipleObjects]
public class SoundControllerInspector : BaseInspector
{
    public SerializedProperty buttonClips;
    public SerializedProperty otherClips;
    private void OnEnable()
    {
        buttonClips = serializedObject.FindProperty("buttonClips");
        otherClips = serializedObject.FindProperty("otherClips");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        ShowArrayProperty(buttonClips, typeof(Sound.Button), "Button Clips");
        ShowArrayProperty(otherClips, typeof(Sound.Others), "Other Clips");

        serializedObject.ApplyModifiedProperties();
    }
}