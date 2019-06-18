using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class JsonText : MonoBehaviour
{
    [SerializeField] string _textType;
    Text _text;

    void Start()
    {
        _text = GetComponent<Text>();
        _text.text = GetText(_textType);
    }

    void OnEnable()
    {
        _text = _text ?? GetComponentInChildren<Text>();
        ReloadText();
    }

    public void ReloadText()
    {
        _text.text = GetText(_textType);
    }

    public static string GetText(string pType)
    {
        return JsonVariables.Instance?[pType][LanguageSettings.Language.ToString()] ?? "UNDEFINED";
    }
}
