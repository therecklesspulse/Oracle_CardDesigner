using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class ToogledText : ToogledElement {

    [SerializeField]
    Text _text;

    void OnEnable()
    {
        _text = GetComponent<Text>();
    }

    public override void Refresh()
    {
        base.Refresh();
        _text.enabled = Oracle.CheckValue(checkField);
    }

    void OnValidate()
    {
        Refresh();
    }
}




[CustomEditor(typeof(ToogledText))]
[CanEditMultipleObjects]
public class ToogledTextEditor : ToogledElementEditor
{
    SerializedProperty text;

    new void OnEnable()
    {
        base.OnEnable();
        text = serializedObject.FindProperty("_text");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //serializedObject.Update();

        EditorGUILayout.LabelField("Text Info", EditorStyles.boldLabel);
        //EditorGUILayout.PropertyField(img);
        EditorGUILayout.LabelField("Text: ", ((Text)text.objectReferenceValue).text, EditorStyles.helpBox);

        serializedObject.ApplyModifiedProperties();
    }
}