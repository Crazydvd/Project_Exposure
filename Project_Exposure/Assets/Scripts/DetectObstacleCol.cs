using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObstacleCol : MonoBehaviour
{
    ObstacleScript _obstacle;
    ScreenShake _screenShake;

    void Start()
    {
        _obstacle = GetComponentInParent<ObstacleScript>();
        _screenShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScreenShake>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "MainCamera")
        {
            Debug.Log("u dede");
            _obstacle.Shatter();
            _screenShake.StartShake(0.4f, 0.2f);
        }
        else
        if (other.transform.tag.ToUpper() == _obstacle.GetFreq() + "FREQ")
        {
            _obstacle.EnableShake(true);
        }
        else if (other.gameObject.layer == 9) //layer 9 == Player (all bullets are on this layer)
        {
            _obstacle.EnableShake(false);
        }
    }
}
