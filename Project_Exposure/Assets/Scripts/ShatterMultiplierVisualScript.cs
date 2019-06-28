using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShatterMultiplierVisualScript : MonoBehaviour
{
    [SerializeField] float _timerTime = 1f;
    RectTransform _rectTransform;
    Text _text;
    Color _originalColor;
    GameObject _followObject;
    float _timer;

    void Start()
    {
        _text = GetComponent<Text>();
        _originalColor = _text.color;
        _rectTransform = GetComponent<RectTransform>();
    }

    void Update()
    {
        if (_followObject)
        {
            transform.position = Camera.main.WorldToScreenPoint(_followObject.transform.position) + new Vector3(0, 15f - _timer * 30f, 0);
            _text.color -= new Color(0, 0, 0, Time.deltaTime);
        }
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0)
            {
                Destroy(_followObject);
                transform.position = new Vector3(10000, 10000, 10000); // way off the screen
            }
        }
    }

    public void SetFollowObject(Transform pObject)
    {
        if (ShootScript.Multiplier > 1)
        {
            _text.text = "X" + ShootScript.Multiplier;
            _text.color = _originalColor;
            float increase = 0.03f * ShootScript.Multiplier;
            if (increase > 0.5f)
            {
                increase = 0.5f;
            }
            _rectTransform.localScale = new Vector3(0.5f + increase, 0.5f + increase, 0.5f + increase);
            _followObject = new GameObject();
            _timer = _timerTime;
            _followObject.transform.position = pObject.position;
        }
    }
}
