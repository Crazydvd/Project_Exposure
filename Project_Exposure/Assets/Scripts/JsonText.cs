﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class JsonText : MonoBehaviour
{
    [SerializeField] string _textType;

    void Start()
    {
        GetComponent<Text>().text = GetText(_textType);
    }

    public static string GetText(string pType)
    {
        return JsonVariables.Instance[pType][LanguageSettings.Language.ToString()];
    }
}
