using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public bool PierceShotMode = false;

    Rigidbody _rigidbody;
    bool _slowdown;
    Vector3 _oldVelocity;
    Vector3 _velocity;
    bool _hitObstacle;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();

        Invoke("SelfDestruct", 5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer).ToUpper() == "WALLS")
        {
            SelfDestruct();
        }
        else if (LayerMask.LayerToName(other.gameObject.layer).ToUpper() == "OBSTACLES")
        {
            _hitObstacle = true;
            if (!PierceShotMode)
            {
                Destroy(gameObject);
            }
        }
    }

    void Update()
    {
        if (Time.timeScale < 1 && Time.timeScale > 0)
        { // on overcharge
            if (!_slowdown && _rigidbody.velocity.magnitude > 0)
            {
                _oldVelocity = _rigidbody.velocity;
                _velocity = _oldVelocity * (1 + (1 - Time.timeScale));
                _slowdown = true;
            }
        }
        _rigidbody.position += _velocity * Time.unscaledDeltaTime;

        if (_slowdown && Mathf.Approximately(Time.timeScale, 1f))
        { // end of overcharge
            _rigidbody.velocity = _oldVelocity;
            _slowdown = false;
        }
    }

    public void SetVelocity(Vector3 pVelocity)
    {
        _oldVelocity = pVelocity;
    }

    public void SelfDestruct()
    {
        if (!_hitObstacle)
        {
            ShootScript.Multiplier = 1;
        }
        Destroy(gameObject);
    }
}
