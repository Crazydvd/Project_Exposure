using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLanguageSetting : MonoBehaviour
{
    [SerializeField] int language;

    public void SetLanguage()
    {
        LanguageSettings.Language = language; 
    }
}
