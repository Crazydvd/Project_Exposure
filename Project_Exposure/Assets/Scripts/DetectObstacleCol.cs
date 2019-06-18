using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObstacleCol : MonoBehaviour
{
    ObstacleScript _obstacle = null;
    ScreenShake _screenShake = null;

    Rigidbody _rigidbody = null;

    bool _shatterOnFall = true;

    void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _obstacle = GetComponentInParent<ObstacleScript>();
        _screenShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScreenShake>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "MainCamera")
        {
            _obstacle.Shatter(true);
            _screenShake.StartShake(24f, 0.2f);
        }
        else
        if (other.transform.tag.ToUpper() == _obstacle.GetFreq() + "FREQ")
        {
            _obstacle.EnableShake(true);
        }
        else if (other.gameObject.layer == 9) //layer 9 == Player (all bullets are on this layer)
        {
            _obstacle.EnableShake(false);
            ShootScript.Multiplier = 1;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (_rigidbody == null)
        {
            return;
        } 

        if (_shatterOnFall && _rigidbody?.velocity.magnitude > 1.8)
        {
            fallOnFloor();
        }
    }

    public void FallImmune()
    {
        _shatterOnFall = false;
        Invoke("resetFallImmunity", 1.0f);
    }

    void resetFallImmunity()
    {
        _shatterOnFall = true;
    }

    void fallOnFloor()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/bounceglass", gameObject);
        GetComponentInParent<ObstacleScript>().Shatter();
    }
}
