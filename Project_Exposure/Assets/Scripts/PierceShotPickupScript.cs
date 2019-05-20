using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PierceShotPickupScript : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer).ToUpper() == "PLAYER") {
            GetComponentInParent<PowerupManagerScript>().ActivatePierceShot();
            Destroy(gameObject);
        }   
    }
}