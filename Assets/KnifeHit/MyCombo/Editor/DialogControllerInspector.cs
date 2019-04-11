using System;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DialogController))]
[CanEditMultipleObjects]
public class DialogControllerInspector : BaseInspector
{
    public SerializedProperty baseDialogs;
    private void OnEnable()
    {
        baseDialogs = serializedObject.FindProperty("baseDialogs");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        serializedObject.Update();
        ShowArrayProperty(baseDialogs, typeof(DialogType), "Dialogs");
        serializedObject.ApplyModifiedProperties();
    }
}