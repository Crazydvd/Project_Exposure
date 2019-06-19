using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KnobScript : MonoBehaviour
{
    [SerializeField] ShootScript _shootScript;
    [SerializeField] float _rotationSpeed = 0.1f;

    [Header("Low Frequency Images")]
    [SerializeField] Sprite _dutchLow = null;
    [SerializeField] Sprite _englishLow = null;
    [SerializeField] Sprite _germanLow = null;

    [Header("Medium Frequency Images")]
    [SerializeField] Sprite _dutchMedium = null;
    [SerializeField] Sprite _englishMedium = null;
    [SerializeField] Sprite _germanMedium = null;

    [Header("High Frequency Images")]
    [SerializeField] Sprite _dutchHigh = null;
    [SerializeField] Sprite _englishHigh = null;
    [SerializeField] Sprite _germanHigh = null;

    Sprite[] _dutchSprites = null;
    Sprite[] _englishSprites = null;
    Sprite[] _germanSprites = null;

    Vector3 _targetRotation = new Vector3(0,0, 180);
    bool _holding = false;

    Animator _animator;
    Image _backgroundImage;

    void Start()
    {
        _dutchSprites = new Sprite[]{ _dutchLow, _dutchMedium, _dutchHigh};
        _englishSprites = new Sprite[] { _englishLow, _englishMedium, _englishHigh };
        _germanSprites = new Sprite[] { _germanLow, _germanMedium, _germanHigh };

        _animator = transform.parent.GetComponentInChildren<Animator>();
        _backgroundImage = transform.parent.GetComponent<Image>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (_targetRotation.z < 135)
            {
                SetLow();
            }
            else if (_targetRotation.z > 135 && _targetRotation.z < 225)
            {
                SetMedium();
            }
            else if (_targetRotation.x >= 225)
            {
                SetHigh();
            }
        }
        else
        if (_holding)
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 offset = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            Debug.Log(angle);
            if (angle > 0 && angle > -180)
            {
                _targetRotation = new Vector3(0, 0, angle + 270);
            }
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_targetRotation), _rotationSpeed);
    }
    void playAnimation(string pFrequency)
    {
        _animator.Play(pFrequency + "freq");
    }

    void playAnimation(Frequency pFrequency)
    {
        playAnimation(pFrequency.ToString().ToLower());
    }

    Sprite[] getCurrentSprites(){
        switch(LanguageSettings.Language){
            case Language.NL:
                return _dutchSprites;
            case Language.EN:
                return _englishSprites;
            case Language.DE:
                return _germanSprites;
            default:
                return _dutchSprites;
        }
    }

    public void OnHold()
    {
        if (!_holding)
        {
            //_holding = true;
        }
    }

    public void SetHigh()
    {
        _targetRotation = new Vector3(0, 0, 270);
        _shootScript.SwitchWave(Frequency.HIGH);
        playAnimation(Frequency.HIGH);
        _backgroundImage.sprite = getCurrentSprites()[2];

        _holding = false;
    }

    public void SetMedium()
    {
        _targetRotation = new Vector3(0, 0, 180);
        _shootScript.SwitchWave(Frequency.MEDIUM);
        playAnimation(Frequency.MEDIUM);
        _backgroundImage.sprite = getCurrentSprites()[1];

        _holding = false;
    }

    public void SetLow()
    {
        _targetRotation = new Vector3(0, 0, 90);
        _shootScript.SwitchWave(Frequency.LOW);
        playAnimation(Frequency.LOW);
        _backgroundImage.sprite = getCurrentSprites()[0];

        _holding = false;
    }
}
