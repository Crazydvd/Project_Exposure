using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManagerScript : MonoBehaviour
{
    [SerializeField] ShootScript _playerScript;
    [SerializeField] float _overchargeTimeSpeed = 0.2f;

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
        Time.timeScale = _overchargeTimeSpeed;
        Invoke("disableOvercharge", _playerScript.OverchargeCooldownTime * Time.timeScale);// / Time.timeScale);
    }
}
