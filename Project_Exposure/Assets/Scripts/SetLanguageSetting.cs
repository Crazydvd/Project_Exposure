using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLanguageSetting : MonoBehaviour
{
    [SerializeField] Language _language;

    public void SetLanguage()
    {
        LanguageSettings.Language = (Language) _language;
    }
}
