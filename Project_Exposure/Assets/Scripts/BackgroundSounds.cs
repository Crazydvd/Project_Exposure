using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSounds : MonoBehaviour
{
    FMOD.Studio.EventInstance _sound;

    void Start()
    {
        _sound = FMODUnity.RuntimeManager.CreateInstance("event:/Soundtrack 3");
        startSound();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F1))
        {
            stopSound();
            _sound = FMODUnity.RuntimeManager.CreateInstance("event:/Soundtrack 1");
            startSound();
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            stopSound();
            _sound = FMODUnity.RuntimeManager.CreateInstance("event:/Soundtrack 2");
            startSound();
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            stopSound();
            _sound = FMODUnity.RuntimeManager.CreateInstance("event:/Soundtrack 3");
            startSound();
        }
    }

    void startSound()
    {
        _sound.start();
        _sound.release();
    }

    void stopSound()
    {
        _sound.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
    }

    void OnDestroy()
    {
        stopSound();
    }
}
