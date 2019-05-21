using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManagerScript : MonoBehaviour
{
    [SerializeField] ShootScript _playerScript;

    bool _overcharge = false;
    private void Update()
    {

    }

    private void DisableOvercharge(){
        Time.timeScale = 1f;
    }

    public void ActivatePierceShot(){
        _playerScript.EnablePierceShot();
    }

    public void ActivateOvercharge(){
        Debug.Log("ZA WARUDO");
        Time.timeScale = 0.2f;
        Invoke("DisableOvercharge", 1f / Time.timeScale);
    }
}
