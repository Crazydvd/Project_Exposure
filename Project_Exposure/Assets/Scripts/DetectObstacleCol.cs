using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObstacleCol : MonoBehaviour
{
    ObstacleScript _obstacle;

    void Start()
    {
        _obstacle = GetComponentInParent<ObstacleScript>();
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "MainCamera")
        {
            Debug.Log("u dede");
            _obstacle.Shatter();
        }

        if (other.transform.tag.ToUpper() == _obstacle.GetFreq() + "FREQ")
        {
            _obstacle.SetPOI(other.GetContact(0).point);
            _obstacle.EnableShake(true);
        }
        else
        {
            _obstacle.EnableShake(false);
        }
    }
}
