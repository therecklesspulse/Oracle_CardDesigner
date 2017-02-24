using UnityEngine;
using UnityEditor;
using System.Collections;
using System;
using System.IO;

[ExecuteInEditMode]
public class CardExporter : MonoBehaviour {

    [SerializeField]
    bool exportCard = false;
    [SerializeField]
    bool exportSet = false;

    bool exportInPlayMode = false;

    [SerializeField]
    int targetWidth = 375;
    [SerializeField]
    int targetHeight = 525;

    const string CARD_PATH = "/Resources/Cards/";

    void OnEnable()
    {
        EditorApplication.update += OnEditorUpdate;
    }

    void OnValidate()
    {
        if (exportCard)
        {
            ExportCard();
            exportCard = false;
        }

        if (exportSet)
        {
            ExportSet();
            exportSet = false;
        }
    }

    void ExportCard()
    {
        TakeScreenshot();
    }

    void ExportSet()
    {
        //if (!exportInPlayMode)
        //{
            exportInPlayMode = true;
            UnityEditor.EditorApplication.isPlaying = true;
            StartCoroutine(TakeScreenshotsOfSet());
        //}
    }

    void OnDisable()
    {
        EditorApplication.update -= OnEditorUpdate;
    }

    void OnEditorUpdate() {}

    void Start()
    {
        if (exportInPlayMode)
        {
            //StartCoroutine(TakeScreenshotsOfSet());
            //exportInPlayMode = false;
        }        
    }

    private void TakeScreenshot()
    {
        /*Vector2 size = Handles.GetMainGameViewSize();
        Texture2D tex = new Texture2D((int)size.x, (int)size.y, TextureFormat.RGB24, false);
        tex.ReadPixels(new Rect(0, 0, (int)size.x, (int)size.y), 0, 0);
        tex.Apply();
        byte[] bytes = tex.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + CARD_PATH + Oracle.GetValue("Name") + ".png", bytes);*/
        Application.CaptureScreenshot(Application.dataPath + CARD_PATH + Oracle.GetValue("Name") + ".png");
        //AssetDatabase.Refresh();
    }

    IEnumerator TakeScreenshotsOfSet()
    {

        yield return new WaitForEndOfFrame();
        print("started");
        int activeCard = Oracle.CardToShowID;
        if (activeCard == 0) activeCard = 1;

        for (int i = 1; i <= Oracle.NumberOfCards; i++)
        {
            Oracle.CardToShowID = i;
            foreach (DynamicElement de in GetComponentsInChildren<DynamicElement>())
                de.Refresh();

            TakeScreenshot();
            yield return new  WaitForSeconds(1);
            
            print(Oracle.CardToShowID);       
        }

        UnityEditor.EditorApplication.isPlaying = false;

        Oracle.CardToShowID = activeCard;
        foreach (DynamicElement de in GetComponentsInChildren<DynamicElement>())
            de.Refresh();

        print("ended");
    }
}

[CustomEditor(typeof(CardExporter))]
[CanEditMultipleObjects]
public class CardExporterEditor : Editor
{
    SerializedProperty exportCard;
    SerializedProperty exportSet;

    void OnEnable()
    {
        exportCard = serializedObject.FindProperty("exportCard");
        exportSet = serializedObject.FindProperty("exportSet");
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Card Exporter", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Path: " + Application.dataPath + "/Resources/Cards");
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        GUILayout.FlexibleSpace();
        if (GUILayout.Button("Save Card"))
        {
            exportCard.boolValue = true;
        }
        if (GUILayout.Button("Save Set"))
        {
            exportSet.boolValue = true;
        }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
        AssetDatabase.Refresh();
    }
}
