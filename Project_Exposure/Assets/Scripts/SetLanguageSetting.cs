using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLanguageSetting : MonoBehaviour
{
    [SerializeField] int _language;

    public void SetLanguage()
    {
        LanguageSettings.Language = _language;
    }
}
