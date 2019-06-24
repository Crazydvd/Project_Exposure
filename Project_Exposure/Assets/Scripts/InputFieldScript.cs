using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputFieldScript : MonoBehaviour
{
    InputField _text;
    void Start()
    {
        _text = GetComponent<InputField>();
    }

    public void AddText(string pText){
        if (_text.text.Length < 16)
        {
            _text.text += pText;
        }
    }

    public void Backspace(){
        if (_text.text.Length > 0)
        {
            _text.text = _text.text.Substring(0, (_text.text.Length - 1));
        }
    }
}
