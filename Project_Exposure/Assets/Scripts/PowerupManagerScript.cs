using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManagerScript : MonoBehaviour
{
    [SerializeField] ShootScript _playerScript;
    [SerializeField] float _overchargeTimeSpeed = 0.2f;

    GameObject _overchargeUI; 

    void Start()
    {
        _overchargeUI = GameObject.Find("Canvas").transform.Find("Overcharge").gameObject;
    }

    void disableOvercharge()
    {
        Time.timeScale = 1f;
        _overchargeUI.SetActive(false);
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
        _overchargeUI.SetActive(true);
        Time.timeScale = _overchargeTimeSpeed;
        Invoke("disableOvercharge", _playerScript.OverchargeCooldownTime * Time.timeScale);// / Time.timeScale);
    }
}
