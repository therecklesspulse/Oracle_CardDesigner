using UnityEngine;
using UnityEditor;
using System.Collections;
using System;

[ExecuteInEditMode]
public class CardExporter : MonoBehaviour {

    [SerializeField]
    bool exportCard = false;
    [SerializeField]
    bool exportSet = false;

    [SerializeField]
    Camera mainCamera;
    [SerializeField]
    int targetWidth = 375;
    [SerializeField]
    int targetHeight = 525;

    void OnEnable()
    {
        mainCamera = Camera.main;
    }

    void OnValidate()
    {
        if (exportCard)
        {
            ExportCameraImage();
            exportCard = false;
        }

        if (exportCard)
        {
            ExportSet();
            exportSet = false;
        }
    }

    void ExportCameraImage()
    {
        TakeScreenShot();
    }

    void ExportSet()
    {

    }

    private void TakeScreenShot()
    {

        /*RenderTexture rt = new RenderTexture(targetWidth, targetHeight, 24);
        RenderTexture.active = rt;
        mainCamera.targetTexture = rt;
        mainCamera.Render();

        Texture2D cardImage = new Texture2D(targetWidth, targetHeight, TextureFormat.RGB24, false);      
        cardImage.ReadPixels(new Rect(0, 0, targetWidth, targetHeight), 0, 0);
        cardImage.Apply();

        mainCamera.targetTexture = null;
        RenderTexture.active = null;
        DestroyImmediate(rt);


        // Encode texture into PNG
        byte[] bytes = cardImage.EncodeToPNG();

        // save in memory
        string filename = "patata.png";
        string path = Application.dataPath  + "/Resources/Cards/" + filename;

        System.IO.File.WriteAllBytes(path, bytes);*/

        Application.CaptureScreenshot(Application.dataPath + "/Resources/Cards/patata.png");
    }
}

//Windows Store Apps: Application.persistentDataPath points to <user>\AppData\Local\Packages\<productname>\LocalState.
//Mac: The persistentDataPath is written into ~/Library/Application Support/company name/product name.
//IOS and Android: persistentDataPath will point to a public directory on the device.
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
    }
}
