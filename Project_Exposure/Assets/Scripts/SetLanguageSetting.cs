using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetLanguageSetting : MonoBehaviour
{
    //NOTE: Why not use an enumerator for this?
    [SerializeField] private int _language;

    public void SetLanguage() => LanguageSettings.Language = _language;
}
