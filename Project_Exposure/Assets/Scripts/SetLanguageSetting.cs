using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLanguageSetting : MonoBehaviour
{
    [SerializeField] Language _language = Language.NL;

    void Start()
    {
        SetLanguage();
    }

    public void SetLanguage()
    {
        LanguageSettings.Language = _language;
    }
}
