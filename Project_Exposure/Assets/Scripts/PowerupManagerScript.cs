using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManagerScript : MonoBehaviour
{
    [SerializeField] ShootScript _playerScript;
    [SerializeField] float _overchargeTimeSpeed = 0.2f;
    [SerializeField] [Range(0f, 1f)] float _pitch = 0.5f;

    GameObject _overchargeUI;
    FMOD.Studio.Bus _mainBus;

    void Start()
    {
        _overchargeUI = GameObject.Find("Canvas").transform.Find("Overcharge").gameObject;
        _mainBus = FMODUnity.RuntimeManager.GetBus("bus:/Main");
    }

    void disableOvercharge()
    {
        Time.timeScale = 1f;
        _overchargeUI.SetActive(false);

        FMOD.ChannelGroup group;
        _mainBus.getChannelGroup(out group);
        group.setPitch(1f);
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
        FMOD.ChannelGroup group;
        _mainBus.getChannelGroup(out group);
        group.setPitch(_pitch);
    }
}
