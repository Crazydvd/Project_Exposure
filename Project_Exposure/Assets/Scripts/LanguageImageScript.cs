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
        Language language = LanguageSettings.Language;
        Sprite sprite = getImage(language);

        while (language >= 0 && sprite != null)
        {
            language--;
            sprite = getImage(language);
        }

        if (sprite == null)
        {
            return;
        }

        GetComponent<Image>().sprite = sprite;
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
