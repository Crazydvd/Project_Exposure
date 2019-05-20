using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    public bool PierceShotMode = false;

    private void Start()
    {
        Destroy(gameObject, 5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (!PierceShotMode && LayerMask.LayerToName(other.gameObject.layer).ToUpper() == "OBSTACLES")
        {
            Destroy(gameObject);
        }
    }
}
