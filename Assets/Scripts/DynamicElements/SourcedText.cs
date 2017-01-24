using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class SourcedText : SourcedElement {

    [SerializeField]
    Text _text;

    void OnEnable()
    {
        _text = GetComponent<Text>();
    }

    public override void Refresh()
    {
        base.Refresh();
        _text.text = Oracle.GetValue(sourceField);
    }

    void OnValidate()
    {
        Refresh();
    }
}


[CustomEditor(typeof(SourcedText))]
[CanEditMultipleObjects]
public class SourcedTextEditor : SourcedElementEditor
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

        serializedObject.Update();

        EditorGUILayout.LabelField("Text Info", EditorStyles.boldLabel);
        //EditorGUILayout.PropertyField(text);
        EditorGUILayout.LabelField("Text: ", ((Text)text.objectReferenceValue).text, EditorStyles.helpBox);

        serializedObject.ApplyModifiedProperties();
    }
}
