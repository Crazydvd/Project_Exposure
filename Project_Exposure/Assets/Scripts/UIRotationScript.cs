using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIRotationScript : MonoBehaviour
{
    [SerializeField] float _rotationSpeed = 5f;
    RectTransform _rectTransform;
    Quaternion _targetRotation;

    void Start()
    {
        _rectTransform = GetComponent<RectTransform>();   
    }

    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(_targetRotation.eulerAngles), _rotationSpeed);
    }

    public void SetZRotation(float pAmount){
        _targetRotation = Quaternion.Euler(0, 0, pAmount);
    }
}
