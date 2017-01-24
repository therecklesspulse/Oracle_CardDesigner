using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class ToogledImage : ToogledElement {

    [SerializeField]
    Image _img;

    void OnEnable()
    {
        _img = GetComponent<Image>();
    }

    public override void Refresh()
    {
        base.Refresh();
        _img.enabled = Oracle.CheckValue(checkField);
    }

    void OnValidate()
    {
        Refresh();
    }
}




[CustomEditor(typeof(ToogledImage))]
[CanEditMultipleObjects]
public class ToogledImageEditor : ToogledElementEditor
{
    SerializedProperty img;

    new void OnEnable()
    {
        base.OnEnable();
        img = serializedObject.FindProperty("_img");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //serializedObject.Update();

        EditorGUILayout.LabelField("Image Info", EditorStyles.boldLabel);
        //EditorGUILayout.PropertyField(img);
        EditorGUILayout.LabelField("Image name: ", ((Image)img.objectReferenceValue).sprite.name, EditorStyles.helpBox);

        serializedObject.ApplyModifiedProperties();
    }
}