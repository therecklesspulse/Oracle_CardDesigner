using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;


public class SourcedElement : DynamicElement {

    [SerializeField]
    protected string sourceField = "";


    //[SerializeField]
    //protected bool dontShowIfNull = true;

    //To locate the element in a custom folder hierarchy
    //[SerializeField]
    //protected bool useSpecificSubFolder = false;

    public override void Refresh()
    {
        base.Refresh();
    }
}





[CustomEditor(typeof(SourcedElement))]
[CanEditMultipleObjects]
public class SourcedElementEditor : Editor
{
    SerializedProperty sourceField;

    protected void OnEnable()
    {
        sourceField = serializedObject.FindProperty("sourceField");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        //serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Source Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(sourceField);

        serializedObject.ApplyModifiedProperties();
    }
}