using System;
using UnityEditor;
using UnityEngine;

public class BaseInspector : Editor
{
    public void ShowArrayProperty(SerializedProperty array, Type type, string label)
    {
        string[] names = (string[])Enum.GetNames(type);
        int beginSize = array.arraySize;
        array.arraySize = names.Length;

        if (beginSize < names.Length)
        {
            for (int i = beginSize; i < names.Length; i++)
            {
                array.GetArrayElementAtIndex(i).objectReferenceValue = null;
            }
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField(label, EditorStyles.boldLabel);
        for (int i = 0; i < names.Length; i++)
        {
            SerializedProperty item = array.GetArrayElementAtIndex(i);
            EditorGUILayout.PropertyField(item, new GUIContent(names[i]));
        }
    }
}