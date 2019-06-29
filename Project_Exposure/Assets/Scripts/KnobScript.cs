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
    StreamVideo _streamVideo;
    Image _backgroundImage;

    void Start()
    {
        _dutchSprites = new Sprite[]{ _dutchLow, _dutchMedium, _dutchHigh};
        _englishSprites = new Sprite[] { _englishLow, _englishMedium, _englishHigh };
        _germanSprites = new Sprite[] { _germanLow, _germanMedium, _germanHigh };

        _streamVideo = transform.parent.GetComponentInChildren<StreamVideo>();
        _animator = transform.parent.GetComponentInChildren<Animator>();

        _backgroundImage = transform.parent.GetComponent<Image>();
    }

    void Update()
    {   
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_targetRotation), _rotationSpeed);
    }

    void playAnimation(string pFrequency)
    {
        _animator.Play(pFrequency.ToLower() + "freq");
    }

    void playAnimation(Frequency pFrequency)
    {
        playAnimation(pFrequency.ToString());
        //_streamVideo.Play(pFrequency);
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
        if (_shootScript.GetWave() == Frequency.HIGH)
        {
            return;
        }

        _targetRotation = new Vector3(0, 0, 270);
        _shootScript.SwitchWave(Frequency.HIGH);
        playAnimation(Frequency.HIGH);
        _backgroundImage.sprite = getCurrentSprites()[2];
        FMODUnity.RuntimeManager.PlayOneShot("event:/knob");

        _holding = false;
    }

    public void SetMedium()
    {
        if (_shootScript.GetWave() == Frequency.MEDIUM)
        {
            return;
        }

        _targetRotation = new Vector3(0, 0, 180);
        _shootScript.SwitchWave(Frequency.MEDIUM);
        playAnimation(Frequency.MEDIUM);
        _backgroundImage.sprite = getCurrentSprites()[1];
        FMODUnity.RuntimeManager.PlayOneShot("event:/knob");

        _holding = false;
    }

    public void SetLow()
    {
        if (_shootScript.GetWave() == Frequency.LOW)
        {
            return;
        }

        _targetRotation = new Vector3(0, 0, 90);
        _shootScript.SwitchWave(Frequency.LOW);
        playAnimation(Frequency.LOW);
        _backgroundImage.sprite = getCurrentSprites()[0];
        FMODUnity.RuntimeManager.PlayOneShot("event:/knob");

        _holding = false;
    }
}
