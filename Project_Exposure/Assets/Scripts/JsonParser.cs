using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class JsonParser : MonoBehaviour
{
    [Tooltip("The name of the .json file")]
    public string FileName = "Language.json";

    string _path;
    public static string JsonString;

    void Start()
    {
        _path = Application.dataPath + "/Localization/" + FileName;
        JsonString = File.ReadAllText(_path);
    }
}

[System.Serializable]
public class JsonVariables
{
    public TEST TEST;

    public static JsonVariables Instance
    {
        get
        {
            return JsonUtility.FromJson<JsonVariables>(JsonParser.JsonString);
        }
    }
}

[System.Serializable]
public class TEST
{
    public string EN;
    public string NL;
}
