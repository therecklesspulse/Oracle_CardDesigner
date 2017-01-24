using UnityEngine;
using System;
using System.Collections;
using System.IO;

public static class Oracle{

    public static bool _initialized = false;
    static string[] _legend;
    static string[][] _database;

    static string _fileName = "";
    public static int CardToShowID = 1;

    public static int NumberOfCards = 0;

    // I N I T I A L I Z A T I O N

    public static void InitializeOracle()
    {
        CalculateNumberOfCards();
        LoadDatabase();
    }

    static void CalculateNumberOfCards()
    {
        int n = -1; //Do not count column names line
        string path = PathForDocumentsFile(_fileName);
        //Debug.Log("Path: " + path);
        if (File.Exists(path))
        {
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);

            while (!sr.EndOfStream)
            {
                sr.ReadLine();
                n++;
            }

            sr.Close();
            file.Close();
        }

        NumberOfCards = n;
    }

    static void LoadDatabase()
    {
        string path = PathForDocumentsFile(_fileName);
        if (File.Exists(path))
        {
            
            FileStream file = new FileStream(path, FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(file);

            // LEGEND
            string line = sr.ReadLine();
            _legend = line.Split(',');
            
            // DATABASE
            _database = new string[NumberOfCards][];
            int i = 0;
            while(!sr.EndOfStream)
            {
                line = sr.ReadLine();
                string wordBuffer = "";
                bool scapeFlag = false;
                _database[i] = new string[_legend.Length];
                int j = 0;

                foreach (char c in line)
                {
                    if(j >= _legend.Length)
                    {
                        Debug.Log("Error loading csv file. Unescaped comma at entry " + i+1);
                        break;
                    }

                    if (c == '\\' && !scapeFlag)
                    {
                        scapeFlag = true;
                    }
                    else if(c == ',' && !scapeFlag)
                    {
                        _database[i][j] = wordBuffer;
                        wordBuffer = "";
                        j++;
                    }
                    else
                    {
                        wordBuffer += c;
                        scapeFlag = false;
                    }
                }

                if (j >= _legend.Length)
                    Debug.LogWarning("Error loading csv file. Unescaped comma at entry " + i + 1);
                else
                    _database[i][j] = wordBuffer;

                i++;
            }

            // DEBUG

            // - Print the whole database
            /*string print = "";
            foreach (string[] sa in _database)
            {
                foreach (string s in sa)
                {
                    print += s + ", ";
                }
                Debug.Log(print);
                print = "";
            }*/
            

            // Close the file
            sr.Close();
            file.Close();

            _initialized = true;
        }
        else
        {
            Debug.LogWarning("The oracle csv file was not found. Please place the file on the Assets folder.");
        }
    }

    public static void UpdateFileName(string newFileName)
    {
        _fileName = newFileName + ".txt";
    }


    // F U N C T I O N A L I T Y


    public static string GetValue(string sourceField)
    {
        if (!_initialized)
            return "";

        CardToShowID = Mathf.Clamp(CardToShowID, 1, NumberOfCards);

        int cardID = CardToShowID-1;

        //string result = "unknown";
        string result = "";

        bool success = false;
        int targetColumn = 0;
        foreach(string s in _legend)
        {
            if (sourceField == s)
            {
                success = true;
                break;
            }
            else
            {
                targetColumn++;
            }
        }

        if (success)
            result = _database[cardID][targetColumn];
        else
            Debug.LogWarning("Field not found. Please make sure that the names on the sourcing fields of your elements in the editor match the first row's names on the csv file.");

        return result;
    }


    public static bool CheckValue(string fieldToCheck)
    {
        if (!_initialized)
            return false;

        CardToShowID = Mathf.Clamp(CardToShowID, 1, NumberOfCards);

        int cardID = CardToShowID - 1;
        bool result = false;

        bool success = false;
        int targetColumn = 0;
        foreach (string s in _legend)
        {
            if (fieldToCheck == s)
            {
                success = true;
                break;
            }
            else
            {
                targetColumn++;
            }
        }

        if (success)
            result = _database[cardID][targetColumn] != "";
        else
            Debug.LogWarning("Field not found. Please make sure that the names on the sourcing fields of your elements in the editor match the first row's names on the csv file.");

        return result;
    }


    public static string GetKeyValue(int cardNbr)
    {
        if (!_initialized)
            return "";

        return _database[cardNbr-1][0];
    }





    // UTILITIES
    public static string PathForDocumentsFile(string filename)
    {
        string path = Application.dataPath + "/" + filename;
        //path = path.Substring(0, path.LastIndexOf('/'));
        //return Path.Combine(path, filename);
        return path;
    }

}
