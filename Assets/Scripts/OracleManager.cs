using UnityEngine;
using UnityEditor;
using System.Collections;

[ExecuteInEditMode]
public class OracleManager : MonoBehaviour {

    [SerializeField]
    string FileName = "";

    [SerializeField]
    int CardToShowID = 1;


    // Use this for initialization
    void OnEnable () {
        Oracle.UpdateFileName(FileName);
        Oracle.CardToShowID = CardToShowID;
        Oracle._initialized = false;
        Oracle.InitializeOracle();

        // Ready Programable Icons
        foreach (ProgramableIcons pi in GetComponentsInChildren<ProgramableIcons>())
        {
            int i = 0;
            while (i < pi.transform.childCount)
            {
                if (pi.transform.GetChild(i).tag == ProgramableIcons.ICON_TAG)
                    DestroyImmediate(pi.transform.GetChild(i).gameObject);
                else
                    i++;
            }

            pi.InitializePool();
            pi.Refresh();
        }
    }

    void OnValidate()
    {
        Oracle.UpdateFileName(FileName);
        if (CardToShowID < 1) CardToShowID = 1;
        else if (CardToShowID > Oracle.NumberOfCards) CardToShowID = Oracle.NumberOfCards;
        Oracle.CardToShowID = CardToShowID;
        Oracle._initialized = false;
        Oracle.InitializeOracle();

        foreach (DynamicElement de in GetComponentsInChildren<DynamicElement>())
            de.Refresh();
    }
}





[CustomEditor(typeof(OracleManager))]
[CanEditMultipleObjects]
public class OracleManagerEditor : Editor
{
    SerializedProperty fileName;
    SerializedProperty cardToShowID;

    void OnEnable()
    {
        fileName = serializedObject.FindProperty("FileName");
        cardToShowID = serializedObject.FindProperty("CardToShowID");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        //serializedObject.Update();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Oracle Settings", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(fileName);
        EditorGUILayout.LabelField(".txt", EditorStyles.helpBox, GUILayout.MaxWidth(32));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(cardToShowID);
        EditorGUILayout.LabelField("/" + Oracle.NumberOfCards);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
            GUILayout.FlexibleSpace();
            if (GUILayout.Button("|<", GUILayout.Width(30)))
            {
                cardToShowID.intValue = 1;
            }
            if (GUILayout.Button("<", GUILayout.Width(30)))
            {
                if(cardToShowID.intValue > 1)
                    cardToShowID.intValue--;
            }
            if (GUILayout.Button(">", GUILayout.Width(30)))
            {
                if(cardToShowID.intValue < Oracle.NumberOfCards)
                    cardToShowID.intValue++;
            }
            if (GUILayout.Button(">|", GUILayout.Width(30)))
            {
                cardToShowID.intValue = Oracle.NumberOfCards;
            }
        EditorGUILayout.EndHorizontal();

        serializedObject.ApplyModifiedProperties();
    }
}