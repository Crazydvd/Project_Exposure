using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    void OnCollisionEnter(Collision other)
    {
        Shatter();
    }

    void Shatter()
    {
        //just destroy it for now
        Destroy(gameObject);
    }
}
