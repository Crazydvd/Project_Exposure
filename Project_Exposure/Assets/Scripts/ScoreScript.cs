using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    Text _textUI;
    float _score;

    // animation properties
    float _originalTextSize;
    bool _textAnimationUpscale;
    bool _textAnimationDownscale;

    void Start()
    {
        _textUI = GetComponent<Text>();
        _textUI.text = JsonText.GetText("SCORE") + ": 0";
        _originalTextSize = _textUI.fontSize;
    }

    void Update()
    {
        if (_textAnimationUpscale)
        {
            if (_textUI.fontSize < _originalTextSize + 10)
            {
                _textUI.fontSize++;
            }
            else
            {
                _textAnimationUpscale = false;
                _textAnimationDownscale = true;
            }
        }
        if (_textAnimationDownscale)
        {
            if (_textUI.fontSize > _originalTextSize)
            {
                _textUI.fontSize--;
            }
            else
            {
                _textAnimationDownscale = false;
            }
        }
    }

    public void IncreaseScore(float pIncrease)
    {
        _score += pIncrease;
        _textUI.text = JsonText.GetText("SCORE") + ": " + _score;
        _textAnimationUpscale = true;
    }

    public float GetScore(){
        return _score;
    }
}
