using System.Collections;
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
        Invoke("SelfDestruct", 3f);
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
        if (Time.timeScale < 1 && Time.timeScale > 0)
        { // on overcharge
            if (!_slowdown && _rigidbody.velocity.magnitude > 0)
            {
                _velocity = _rigidbody.velocity;
                _rigidbody.isKinematic = true;
                _slowdown = true;
            }
            transform.position += _velocity * (Time.deltaTime / Time.timeScale);
        }

        if (_slowdown && Mathf.Approximately(Time.timeScale, 1f))
        { // end of overcharge
            _rigidbody.isKinematic = false;
            _rigidbody.velocity = _velocity;
            _slowdown = false;
        }
    }

    public void SetVelocity(Vector3 pVelocity)
    {
        _velocity = pVelocity;
    }

    public void SelfDestruct()
    {
        if (!PierceShotMode)
        {
            ShootScript.Multiplier = 1;
            Destroy(gameObject);
        }
    }
}
