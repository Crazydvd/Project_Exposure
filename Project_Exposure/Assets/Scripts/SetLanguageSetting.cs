using UnityEngine;

public class SetLanguageSetting : MonoBehaviour
{
    [SerializeField] Language _language = Language.NL;

    public void SetLanguage()
    {
        LanguageSettings.Language = _language;
    }
}
