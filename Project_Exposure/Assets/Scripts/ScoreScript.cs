using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    RectTransform _rectTransform;
    Text _textUI;
    float _score;

    // animation properties
    float _originalHeight;
    bool _textAnimationUpscale;
    bool _textAnimationDownscale;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();
        _textUI = GetComponent<Text>();
        _textUI.text = "0";//JsonText.GetText("SCORE") + ": 0";
        _originalHeight = _rectTransform.rect.height;
    }

    void Update()
    {
        if (_textAnimationUpscale)
        {
            if (_rectTransform.rect.height < _originalHeight + 30)
            {
                _rectTransform.sizeDelta = new Vector2(_rectTransform.rect.width, _rectTransform.rect.height + 3);
            }
            else
            {
                _textAnimationUpscale = false;
                _textAnimationDownscale = true;
            }
        }
        if (_textAnimationDownscale)
        {
            if (_rectTransform.rect.height > _originalHeight)
            {
                _rectTransform.sizeDelta = new Vector2(_rectTransform.rect.width, _rectTransform.rect.height - 3);
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
        _score = Mathf.Clamp(Mathf.Floor(_score), 0, _score);
        _textUI.text = /*JsonText.GetText("SCORE") + ": " + */"" + _score;
        if (!_textAnimationUpscale && !_textAnimationDownscale)
        {
            _textAnimationUpscale = true;
        }
    }

    public void DecreaseScore(float pDecrease)
    {
        IncreaseScore(-pDecrease);
    }

    public void MultiplyScore(float pFactor)
    {
        _score *= pFactor;
        _score = Mathf.Floor(_score);
        _textUI.text = /*JsonText.GetText("SCORE") + ": " + */ "" + _score;
        if (!_textAnimationUpscale && !_textAnimationDownscale)
        {
            _textAnimationUpscale = true;
        }
    }

    public float GetScore()
    {
        return _score;
    }
}
