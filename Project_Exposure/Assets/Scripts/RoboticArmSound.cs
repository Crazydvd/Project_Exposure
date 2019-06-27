using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoboticArmSound : MonoBehaviour
{
    FMOD.Studio.EventInstance _sound;

    void Start()
    {
        _sound = FMODUnity.RuntimeManager.CreateInstance("event:/Roboticarms");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_sound, transform, GetComponent<Rigidbody>());
    }

    public void StartSound()
    {
        _sound.start();
        _sound.release();
    }

    public void StopSound()
    {
        _sound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }
}
