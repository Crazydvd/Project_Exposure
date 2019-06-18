using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObstacleCol : MonoBehaviour
{
    ObstacleScript _obstacle;
    ScreenShake _screenShake;

    Rigidbody _rigidbody;

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
        if (collision.gameObject.tag.ToLower() == "floor" && _rigidbody.velocity.magnitude > 1.2)
        {
            fallOnFloor();
        }
    }

    void fallOnFloor()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/bounceglass", gameObject);
        GetComponentInParent<ObstacleScript>().Shatter();
    }
}
