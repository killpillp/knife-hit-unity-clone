using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Music))]
[CanEditMultipleObjects]
public class MusicControllerInspector : BaseInspector
{
    public SerializedProperty musicClips;
    private void OnEnable()
    {
        musicClips = serializedObject.FindProperty("musicClips");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();

        ShowArrayProperty(musicClips, typeof(Music.Type), "Music Clips");

        serializedObject.ApplyModifiedProperties();
    }
}