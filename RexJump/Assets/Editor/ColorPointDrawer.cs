using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(ColorPoint))]
public class ColorPointDrawer : PropertyDrawer 
{
    public override void OnGUI (Rect pos, SerializedProperty property, GUIContent label)
    {
        Rect contentPos = EditorGUI.PrefixLabel (pos, label); // Remove overlapping label
        EditorGUI.indentLevel = 0;
        EditorGUI.PropertyField (contentPos, property.FindPropertyRelative("position"), GUIContent.none);
    }
}
