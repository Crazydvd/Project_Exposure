using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManagerScript : MonoBehaviour
{
    [SerializeField] ShootScript _playerScript;

    bool _overcharge = false;

    void disableOvercharge()
    {
        Time.timeScale = 1f;
    }

    public void ActivatePierceShot()
    {
        _playerScript.EnablePierceShot();
    }

    public void ActivateBattery()
    {
        _playerScript.EnableBattery();
    }

    public void ActivateOvercharge()
    {
        Time.timeScale = 0.2f;
        Invoke("disableOvercharge", 15f * Time.timeScale);// / Time.timeScale);
    }
}
