using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LanguageImageScript : MonoBehaviour
{
    [SerializeField] Sprite _dutch = null; 
    [SerializeField] Sprite _english = null; 
    [SerializeField] Sprite _german = null; 
    [SerializeField] Sprite _russian = null;
    [SerializeField] Sprite _bulgerian = null;

    void Start()
    {
        GetComponent<Image>().sprite = getImage(LanguageSettings.Language);
    }

    Sprite getImage(Language pLanguage)
    {
        switch (pLanguage)
        {
            case Language.NL:
                return _dutch;
            case Language.EN:
                return _english;
            case Language.DE:
                return _german;
            case Language.RU:
                return _russian;
            case Language.BG:
                return _bulgerian;
            default:
                return _dutch;
        }
    }
}
