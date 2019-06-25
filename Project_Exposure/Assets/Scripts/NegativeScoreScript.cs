using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NegativeScoreScript : MonoBehaviour
{
    RectTransform _rectTransform;
    Text _text;
    Color _originalColor;
    bool _enabled;
    float _timer;
    float _timerMax = 1f;

    void Update()
    {
        if(_enabled){
            _rectTransform.position -= new Vector3(0, -1f, 0);
            _text.color -= new Color(0, 0, 0, Time.deltaTime);

            if(_timer > 0){
                _timer -= Time.deltaTime;
            }
            else{
                DisableNow();
            }
        }    
    }
    public void EnableText(){
        gameObject.SetActive(true);

        if (_text == null || _originalColor == null)
        {
            _text = GetComponent<Text>();
            _originalColor = _text.color;
            _rectTransform = GetComponent<RectTransform>();
        }

        _rectTransform.localPosition = new Vector3(0, 0, 0);
        _text.color = _originalColor;
        _enabled = true;
        _timer = _timerMax;
    }

    public void DisableNow(){
        gameObject.SetActive(false);
        _enabled = false;
    }
}
