using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpIconScript : MonoBehaviour
{
    static int _activePowerUpCount = 0;

    Image _ring;
    RectTransform _rectTransform;

    [Header("Direction to move to (mirrored for 2nd)")]
    [SerializeField] Vector2 _amountToMove = new Vector2(-90, 0);

    [Header("Use unscaled Time")]
    [SerializeField] bool _unscaledTime = true;

    [Header("The speed unit / second")]
    [SerializeField] float _speed = 90;

    [Header("The type of the powerup")]
    [SerializeField] Type _type;

    Vector2 _dest;
    Vector2 _originalPos;

    bool _moving = false;
    bool _movingToDest = false;
    bool _reachedDest = false;

    bool _intitialized = false;

    static float _maxOverchargeTime;
    static float _overchargeTimeLeft;

    static float _maxPierceShots;

    public static float PierceShotsLeft { get; set; }

    enum Type
    {
        OVERCHARGE,
        PIERCE,
    }

    // Start is called before the first frame update
    void Start()
    {
        if (_intitialized)
        {
            return;
        }

        _ring = transform.GetChild(0).GetComponent<Image>();
        _rectTransform = GetComponent<RectTransform>();

        _originalPos = _rectTransform.anchoredPosition;

        _intitialized = true;
    }

    // Update is called once per frame
    void Update()
    {
        setRing();

        if (_activePowerUpCount == 2)
        {
            _moving = true;
            _movingToDest = true;
        }

        if (_moving)
        {
            if (_activePowerUpCount == 1)
            {
                _dest = _originalPos + _amountToMove;
                _movingToDest = false;
                _reachedDest = false;
            }

            if (_movingToDest)
            {
                if (_reachedDest)
                {
                    return;
                }

                move(_dest);

                if (_rectTransform.anchoredPosition == _dest)
                {
                    _reachedDest = true;
                }
            }
            else
            {
                move(_originalPos);
            }
        }
    }

    void OnEnable()
    {
        if (!_intitialized)
        {
            Start();
        }
        else
        {
            _rectTransform.anchoredPosition = _originalPos;
        }

        _activePowerUpCount++;

        //Move to left or right depending on picked up first or second respectively
        _dest = _originalPos + (_activePowerUpCount == 1 ? _amountToMove : -_amountToMove);
    }

    void OnDisable()
    {
        _moving = false;
        _movingToDest = false;
        _reachedDest = false;

        _activePowerUpCount--;
    }

    void move(Vector2 pDest)
    {
        float delta = _speed * (_unscaledTime ? Time.unscaledDeltaTime : Time.deltaTime);

        _rectTransform.anchoredPosition = Vector2.MoveTowards(_rectTransform.anchoredPosition, pDest, delta);
    }

    void setRing()
    {
        if (_type == Type.OVERCHARGE)
        {
            SetRing(_overchargeTimeLeft / _maxOverchargeTime);
            _overchargeTimeLeft -= Time.deltaTime;
        }
        else if (_type == Type.PIERCE)
        {
            SetRing(PierceShotsLeft / _maxPierceShots);
        }
    }

    public void SetRing(float pFillAmount)
    {
        _ring.fillAmount = pFillAmount;
    }

    public static float MaxOverchargeTime
    {
        get
        {
            return _maxOverchargeTime;
        }
        set
        {
            _maxOverchargeTime = value;
            _overchargeTimeLeft = value;
        }
    }

    public static float MaxPierceShots
    {
        get
        {
            return _maxPierceShots;
        }
        set
        {
            _maxPierceShots = value;
            PierceShotsLeft = value;
        }
    }
}
