using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using System.Collections;

[ExecuteInEditMode]
public class SourcedImage : SourcedElement {

    [SerializeField]
    protected string subFolder = "";

    [SerializeField]
    Image _img;

	void OnEnable () {
        _img = GetComponent<Image>();
    }
	

    override public void Refresh()
    {
        base.Refresh();
        if (subFolder != "")
            _img.sprite = Resources.Load<Sprite>(subFolder + "/" + Oracle.GetValue(sourceField));
        else
            _img.sprite = Resources.Load<Sprite>(Oracle.GetValue(sourceField));
    }

    void OnValidate()
    {
        Refresh();
    }
}




[CustomEditor(typeof(SourcedImage))]
[CanEditMultipleObjects]
public class SourcedImageEditor : SourcedElementEditor
{
    SerializedProperty subFolder;
    SerializedProperty img;
    
    new void OnEnable()
    {
        base.OnEnable();
        subFolder = serializedObject.FindProperty("subFolder");
        img = serializedObject.FindProperty("_img");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.LabelField("Image Settings", EditorStyles.boldLabel);
        //EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PropertyField(subFolder);
            //EditorGUILayout.LabelField("Resources/", EditorStyles.helpBox);
        //EditorGUILayout.EndHorizontal();
        //EditorGUILayout.PropertyField(img);
        EditorGUILayout.LabelField("Image name: ", ((Image)img.objectReferenceValue).sprite.name, EditorStyles.helpBox);

        serializedObject.ApplyModifiedProperties();
    }
}