using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

[Serializable]
public struct ModifierAssociation
{
        public string command;
        public string modifierID;
    }

    [CustomPropertyDrawer(typeof(ModifierAssociation))]
    public class ModifiersAssociationDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Using BeginProperty / EndProperty on the parent property means that
            // prefab override logic works on the entire property.
            EditorGUI.BeginProperty(position, label, property);

            // Draw label
            //position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

            // Don't make child fields be indented
            var indent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            // Calculate rects
            var commandRect = new Rect(position.x + 15, position.y, 30, position.height);
            var iconRect = new Rect(position.x + 15 + 32, position.y, position.width * 0.76f, position.height);

            // Draw fields - passs GUIContent.none to each so they are drawn without labels
            EditorGUI.PropertyField(commandRect, property.FindPropertyRelative("command"), GUIContent.none);
            EditorGUI.PropertyField(iconRect, property.FindPropertyRelative("modifierID"), GUIContent.none);

            // Set indent back to what it was
            EditorGUI.indentLevel = indent;

            EditorGUI.EndProperty();
        }
    }
