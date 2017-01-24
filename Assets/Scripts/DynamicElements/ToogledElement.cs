using UnityEngine;
using UnityEditor;
using System.Collections;

public class ToogledElement : DynamicElement {

    [SerializeField]
    protected string checkField = "";

    //[SerializeField]
    //bool checkForCertainValues = false;
    //[SerializeField]
    //string trueValue = "";
    //[SerializeField]
    //string falseValue = "";

    public override void Refresh()
    {
        base.Refresh();
    }
}




[CustomEditor(typeof(ToogledElement))]
[CanEditMultipleObjects]
public class ToogledElementEditor : Editor
{
    SerializedProperty checkField;

    protected void OnEnable()
    {
        checkField = serializedObject.FindProperty("checkField");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        //serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Source Check Settings", EditorStyles.boldLabel);

        EditorGUILayout.PropertyField(checkField);

        serializedObject.ApplyModifiedProperties();
    }
}