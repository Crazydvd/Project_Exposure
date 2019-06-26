using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NegativeScoreScript : MonoBehaviour
{
    RectTransform _rectTransform;
    GameObject _followObject;
    Text _text;
    Color _originalColor;
    bool _enabled;
    float _timer;
    float _timerMax = 1f;

    void Update()
    {
        if(_enabled){
            if (_followObject)
            {
                transform.position = Camera.main.WorldToScreenPoint(_followObject.transform.position) + new Vector3(0, 15f - _timer * 30f, 0);
            }
            else
            {
                _rectTransform.position -= new Vector3(0, -1f, 0);
            }
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
        transform.localScale = new Vector3(1, 1, 1);
        if (_text == null || _originalColor == null)
        {
            _text = GetComponent<Text>();
            _originalColor = _text.color;
            _rectTransform = GetComponent<RectTransform>();
        }

        _rectTransform.localPosition = Vector3.zero;
        _text.color = _originalColor;
        _enabled = true;
        _timer = _timerMax;
    }

    public void SetFollowObject(Transform pObject)
    {
        transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        _followObject = new GameObject();
        _followObject.transform.position = pObject.position;
    }

    public void DisableNow(){
        gameObject.SetActive(false);
        _followObject = null;
        _enabled = false;
    }
}
