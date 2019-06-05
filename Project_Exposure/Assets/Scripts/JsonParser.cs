using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

class JsonParser : MonoBehaviour
{
    [Tooltip("The name of the .json file")]
    public string FileName = "Language.json";

    string _path;

    void Start()
    {
        _path = Application.streamingAssetsPath + "/" + FileName;
        JsonString = File.ReadAllText(_path);

        //questionData = JsonMapper.ToObject(JsonString);

        //TextAsset file = Resources.Load("Language") as TextAsset;
        //JsonString = file.ToString();


    }

    public static string JsonString { get; private set; }
}

[System.Serializable]
public class JsonVariables
{
    public D_ENTRY[] D_ENTRIES;

    public Dictionary<string, string> this[string pIndex]
    {
        get
        {
            return GetDictionary[pIndex];
        }
    }

    static JsonVariables _instance = null;

    public static JsonVariables Instance
    {
        get
        {
            return _instance ?? (_instance = JsonUtility.FromJson<JsonVariables>(JsonParser.JsonString));
        }
    }

    Dictionary<string, Dictionary<string, string>> _dictionary = null;

    public Dictionary<string, Dictionary<string, string>> GetDictionary
    {
        get
        {
            return _dictionary ?? (_dictionary = CalculateDictionary.GetNestedDictionary(D_ENTRIES));
        }
    }
}

[System.Serializable]
public class DICTIONARYENTRY
{
    public string Key;
    public string Value;
}

[System.Serializable]
public class D_ENTRY
{
    public string Key;
    public DICTIONARYENTRY[] Value;

    Dictionary<string, string> _dictionary = null;

    public Dictionary<string, string> GetDictionary
    {
        get
        {
            return _dictionary ?? (_dictionary = CalculateDictionary.GetDictionary(Value));
        }
    }
}

public class CalculateDictionary
{
    public static Dictionary<string, string> GetDictionary(DICTIONARYENTRY[] pArray)
    {
        Dictionary<string, string> dictionary = new Dictionary<string, string>();

        foreach (DICTIONARYENTRY entry in pArray)
        {
            dictionary.Add(entry.Key, entry.Value);
        }

        return dictionary;
    }

    public static Dictionary<string, Dictionary<string, string>> GetNestedDictionary(D_ENTRY[] pArray)
    {
        Dictionary<string, Dictionary<string, string>> dictionary = new Dictionary<string, Dictionary<string, string>>();

        foreach (D_ENTRY entry in pArray)
        {
            dictionary.Add(entry.Key, entry.GetDictionary);
        }

        return dictionary;
    }
}
