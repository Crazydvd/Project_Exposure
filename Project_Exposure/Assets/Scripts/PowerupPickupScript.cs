using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupPickupScript : MonoBehaviour
{
    [SerializeField] bool _pierce = false;
    [SerializeField] bool _overcharge = false;
    [SerializeField] bool _battery = false;

    PowerupManagerScript _powerupManagerScript;
    TutorialZoneScript _tutorialZoneScript;

    void Start()
    {
        _powerupManagerScript = GetComponentInParent<PowerupManagerScript>();
    }

    void OnTriggerEnter(Collider other)
    {
        string layer = LayerMask.LayerToName(other.gameObject.layer).ToUpper();

        if (layer == "PROJECTILES" || layer == "POSTPROCESSING")
        {
            if (_pierce)
            {
                _powerupManagerScript.ActivatePierceShot();
            }
            else if (_overcharge)
            {
                _powerupManagerScript.ActivateOvercharge();
            }
            else if (_battery)
            {
                _powerupManagerScript.ActivateBattery();
            }

            _tutorialZoneScript?.RemoveObstacle();

            Destroy(gameObject);
        }
    }

    public void SetTutorialZone(TutorialZoneScript pTutorialZoneScript){
        _tutorialZoneScript = pTutorialZoneScript;
    }
}