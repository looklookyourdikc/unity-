using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(OutlineBox))]
public class OutlineBoxEditor : Editor
{
    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(serializedObject.FindProperty("isActive"));
        EditorGUILayout.Space(4);

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("颜色", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.PropertyField(serializedObject.FindProperty("outlineColor"), GUIContent.none);
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        EditorGUILayout.Space(4);

        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("宽度", EditorStyles.boldLabel);
        EditorGUI.indentLevel++;
        EditorGUILayout.Slider(serializedObject.FindProperty("outlineWidth"), 0.001f, 0.15f, GUIContent.none);
        EditorGUI.indentLevel--;
        EditorGUILayout.EndVertical();

        serializedObject.ApplyModifiedProperties();
    }
}
