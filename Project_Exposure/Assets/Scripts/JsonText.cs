using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class JsonText
{
    public static string GetText(string pType)
    {
        return JsonVariables.Instance[pType][LanguageSettings.Language.ToString()];
    }
}
