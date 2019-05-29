using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundSounds : MonoBehaviour
{
    void Start()
    {
        FMODUnity.RuntimeManager.PlayOneShot("event:/conveyerbeltloop");
        FMODUnity.RuntimeManager.PlayOneShot("event:/backgroundnoise");
    }
}
