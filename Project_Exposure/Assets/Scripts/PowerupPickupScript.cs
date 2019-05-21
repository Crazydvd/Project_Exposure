using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupPickupScript : MonoBehaviour
{
    [SerializeField] bool _pierce = false;
    [SerializeField] bool _overcharge = false;
    [SerializeField] bool _battery = false;

    PowerupManagerScript _powerupManagerScript;

    private void Start()
    {
        _powerupManagerScript = GetComponentInParent<PowerupManagerScript>();   
    }
    void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer).ToUpper() == "PLAYER") {
            if (_pierce)
            {
                _powerupManagerScript.ActivatePierceShot();
            }
            if(_overcharge){
                _powerupManagerScript.ActivateOvercharge();
            }
            if(_battery){
                _powerupManagerScript.ActivateBattery();
            }
            Destroy(gameObject);
        }   
    }
}