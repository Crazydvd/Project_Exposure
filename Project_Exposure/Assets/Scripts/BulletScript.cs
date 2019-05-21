﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public bool PierceShotMode = false;

    Rigidbody _rigidbody;
    bool _slowdown;
    Vector3 _velocity;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!PierceShotMode && LayerMask.LayerToName(other.gameObject.layer).ToUpper() == "OBSTACLES")
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if(Time.timeScale < 1 && Time.timeScale > 0){
            if(!_slowdown && _rigidbody.velocity.magnitude > 0){
                _velocity = _rigidbody.velocity;
                _rigidbody.isKinematic = true;
                _slowdown = true;
            }
            transform.position += _velocity * (Time.deltaTime / Time.timeScale);
        }
        //Debug.Log(GetComponent<Rigidbody>().velocity.magnitude);
    }

    public void SetVelocity(Vector3 pVelocity){
        _velocity = pVelocity;
    }
}
