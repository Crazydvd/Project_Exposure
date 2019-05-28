using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnobScript : MonoBehaviour
{
    [SerializeField] ShootScript _shootScript;
    [SerializeField] float _rotationSpeed = 0.1f;
    Vector3 _targetRotation = Vector3.zero;
    bool _holding = false;

    Animator _animator;

    void Start()
    {
        _animator = transform.parent.GetComponentInChildren<Animator>();
    }

    void Update()
    {
        if (Input.GetMouseButtonUp(0))
        {
            if (_targetRotation.z > 45)
            {
                SetLow();
            }
            else if (_targetRotation.z > -45 && _targetRotation.z < 45)
            {
                SetMedium();
            }
            else if (_targetRotation.x >= -90)
            {
                SetHigh();
            }
            _holding = false;
        }
        else
        if (_holding)
        {
            Vector2 mousePosition = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            Vector2 offset = new Vector2(mousePosition.x - transform.position.x, mousePosition.y - transform.position.y);
            float angle = Mathf.Atan2(offset.y, offset.x) * Mathf.Rad2Deg;
            if (angle - 90 < 90 && angle - 90 > -90)
            {
                _targetRotation = new Vector3(0, 0, angle - 90);
            }
        }
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_targetRotation), _rotationSpeed);
    }

    public void OnHold()
    {
        if (!_holding)
        {
            _holding = true;
        }
    }

    public void SetHigh()
    {
        _targetRotation = new Vector3(0, 0, -90);
        _shootScript.SwitchWave(Frequency.HIGH);
        playAnimation(Frequency.HIGH);
    }

    public void SetMedium()
    {
        _targetRotation = new Vector3(0, 0, 0);
        _shootScript.SwitchWave(Frequency.MEDIUM);
        playAnimation(Frequency.MEDIUM);
    }

    public void SetLow()
    {
        _targetRotation = new Vector3(0, 0, 90);
        _shootScript.SwitchWave(Frequency.LOW);
        playAnimation(Frequency.LOW);
    }

    void playAnimation(string pFrequency)
    {
        _animator.Play(pFrequency + "freq");
    }

    void playAnimation(Frequency pFrequency)
    {
        playAnimation(pFrequency.ToString().ToLower());
    }


    //HACK: had to stop holding
    public bool HOLDING
    {
        set
        {
            _holding = value;
        }
    }
}
