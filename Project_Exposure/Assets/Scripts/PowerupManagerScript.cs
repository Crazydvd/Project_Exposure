using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManagerScript : MonoBehaviour
{
    [SerializeField] ShootScript _playerScript;

    public void ActivatePierceShot(){
        _playerScript.EnablePierceShot();
    }
}
