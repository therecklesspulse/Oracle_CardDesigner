using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[ExecuteInEditMode]
public class ProgramableIcons : DynamicElement {

    public const string ICON_TAG = "Icon";

    [SerializeField]
    protected string subFolder = "";

    [SerializeField]
    protected string sourceField = "";

    [SerializeField]
    protected bool vertical = false;
    [SerializeField]
    protected bool reverse = false;
    [SerializeField]
    protected Order order;
    protected enum Order { none, alphabetic, definition }


    [SerializeField]
    protected string associationsFileName = "";

    [SerializeField]
    protected Association[] associations;

    [SerializeField]
    protected string[] modifiers;

    [SerializeField]
    protected Association assoc = new Association();

    private List<GameObject> _iconsPool = new List<GameObject>();
    private int _nRequested = 0;

    public void InitializePool()
    {
        _iconsPool = new List<GameObject>();
    }

    public override void Refresh()
    {
        base.Refresh();
        foreach (GameObject go in _iconsPool)
            go.SetActive(false);
        _nRequested = 0;

        string prog = Oracle.GetValue(sourceField);
        int d = 0;
        foreach(char c in prog)
        {
            string iconName = GetAssociatedValue(c);
            // When asking for an undefined character, it shall be ignored
            if (iconName != "")
            {
                //GameObject icon = new GameObject("Icon_"+c, typeof(RectTransform));
                GameObject icon = RequestIconVessel();
                RectTransform rect = (RectTransform)icon.transform;
                Image img = icon.GetComponent<Image>();

                icon.name = "Icon_" + c;
                icon.tag = ICON_TAG;
                icon.transform.SetParent(transform);
                icon.transform.position = transform.position;
                rect.sizeDelta = ((RectTransform)transform).sizeDelta;
                img.sprite = Resources.Load<Sprite>(subFolder + "/" + iconName);
                
                if (vertical)
                {
                    if (reverse)
                        img.transform.position += new Vector3(0, d * rect.sizeDelta.y, 0);
                    else
                        img.transform.position -= new Vector3(0, d * rect.sizeDelta.y, 0);
                }
                else
                {
                    if(reverse)
                        img.transform.position -= new Vector3(d * rect.sizeDelta.x, 0, 0);
                    else
                        img.transform.position += new Vector3(d * rect.sizeDelta.x, 0, 0);
                }
                d++;
            }
            else
            {
                //Check for modifyer character maybe?
            }
        }
    }

    GameObject RequestIconVessel()
    {
        _nRequested++;
        if (_iconsPool.Count < _nRequested)
        {
            _iconsPool.Add(new GameObject("NewVesselIcon", typeof(RectTransform)));
            _iconsPool[_nRequested - 1].AddComponent<Image>();
        }
        _iconsPool[_nRequested - 1].SetActive(true);
        return _iconsPool[_nRequested-1];
    }

    void OnValidate()
    {
        Refresh();
    }

    public void LoadAssociations(string filename)
    {
        Debug.Log("Loading file: " + filename + ".txt");
        string path = Oracle.PathForDocumentsFile(filename + ".txt");
        if (File.Exists(path))
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);

            int count = 0;
            while (!sr.EndOfStream)
            {
                sr.ReadLine();
                count++;
            }

            associations = new Association[count];
            int i = 0;
            file.Position = 0;
            sr.DiscardBufferedData();
            while (!sr.EndOfStream)
            {
                string line = sr.ReadLine();
                Debug.Log(line);
                string[] aline = line.Split(',');
                Debug.Log(aline[0] + ", " + aline[1]);
                Association a = new Association();
                a.command = aline[0];
                a.icon = aline[1];
                associations[i] = a;
                i++;
            }

            sr.Close();
            file.Close();
        }
        else
        {
            Debug.LogWarning("Associations file not found.");
        }
    }

    public void ExportAssociations(string filename)
    {
        Debug.Log("Exporting file: " + filename + ".txt");
        string path = Oracle.PathForDocumentsFile(filename + ".txt");
        FileStream file = new FileStream(path, FileMode.Create, FileAccess.Write, FileShare.ReadWrite);
        StreamWriter sw = new StreamWriter(file);
        foreach(Association a in associations)
        {
            if(!a.Equals(associations[associations.Length-1]))
                sw.WriteLine(a.command + "," + a.icon);
            else
                sw.Write(a.command + "," + a.icon);
        }
        sw.Close();
        file.Close();
    }

    private string GetAssociatedValue(char command)
    {
        string result = "";
        for(int i = 0; i < associations.Length; i++){
            if (associations[i].command == command.ToString())
            {
                result = associations[i].icon;
                break;
            }
        }
        return result;
    }

}



[CustomEditor(typeof(ProgramableIcons))]
[CanEditMultipleObjects]
public class ProgramableIconsEditor : Editor
{
    SerializedProperty subFolder;

    SerializedProperty sourceField;

    SerializedProperty vertical;
    SerializedProperty reverse;

    SerializedProperty associationsFileName;

    SerializedProperty associations;
    SerializedProperty modifiers;

    SerializedProperty arraySize;


    protected void OnEnable()
    {
        subFolder = serializedObject.FindProperty("subFolder");

        sourceField = serializedObject.FindProperty("sourceField");

        vertical = serializedObject.FindProperty("vertical");
        reverse = serializedObject.FindProperty("reverse");

        associationsFileName = serializedObject.FindProperty("associationsFileName");

        associations = serializedObject.FindProperty("associations");
        modifiers = serializedObject.FindProperty("modifiers");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();

        serializedObject.Update();

        EditorGUILayout.LabelField("Sourcing Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(subFolder);
        EditorGUILayout.PropertyField(sourceField);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Placement Settings", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(vertical);
        EditorGUILayout.PropertyField(reverse);

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Programmable Settings", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        {
            EditorGUILayout.LabelField("Filename:  ", EditorStyles.wordWrappedLabel);
            EditorGUILayout.PropertyField(associationsFileName, GUIContent.none);
            EditorGUILayout.LabelField(".txt", EditorStyles.helpBox);
        }
        EditorGUILayout.EndHorizontal();
        if (GUILayout.Button("Load From File"))
        {
            ((ProgramableIcons)serializedObject.targetObject).LoadAssociations(associationsFileName.stringValue);
        }
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Do NOT type file extensions.", EditorStyles.helpBox);
        EditorGUILayout.PropertyField(associations, true);
        EditorGUILayout.Space();
        if (GUILayout.Button("Export Associations csv"))
        {
            ((ProgramableIcons)serializedObject.targetObject).ExportAssociations(associationsFileName.stringValue);
        }

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Modifiers", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(modifiers, true);

        serializedObject.ApplyModifiedProperties();
    }
}