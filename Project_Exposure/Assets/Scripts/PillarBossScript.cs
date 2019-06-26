using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarBossScript : MonoBehaviour
{
    [SerializeField] GameObject _platformShoot;
    [SerializeField] float _animationSpeed = 1f;
    Animator _animator;
    GameObject _mainCamera;
    void Start()
    {
        _animator = GetComponent<Animator>();
        _mainCamera = Camera.main.gameObject;

        SetAnimationSpeed(_animationSpeed);
    }

    void Update()
    {
        _platformShoot.transform.LookAt(_mainCamera.transform);
        _platformShoot.transform.rotation = Quaternion.Euler(0, _platformShoot.transform.rotation.eulerAngles.y, 0);
    }

    void SetAnimationSpeed(float pValue)
    {
        _animator.speed = pValue;
    }

    void OnValidate()
    {
        SetAnimationSpeed(_animationSpeed);
    }
}
