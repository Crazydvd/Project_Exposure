using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimationScript : MonoBehaviour
{
    [Header("The Animator(s) which will play the animation")]
    [SerializeField] Animator[] _animators = null;

    [Header("The name of the animationState to play")]
    [SerializeField] string[] _animationStates = null;

    [Header("Whether or not every object can trigger the animation")]
    [SerializeField] bool _allTrigger = false;
    [Header("Bool for the pillarman")]
    [SerializeField] bool _isPillar;

    [Header("Objects that can trigger the zone")]
    [SerializeField] GameObject[] _pickupAbleObjects;
    [SerializeField] string[] _pickupAbleTags;
    [SerializeField] int[] _pickupAbleLayers;
    [SerializeField] string[] _pickupAbleNames;

    void OnTriggerEnter(Collider other)
    {

        if ((_allTrigger || checkTrigger(other.gameObject)) && _animators != null)
        {
            if (_isPillar)
            {
                _animators[0].speed = 0f;
                return;
            }

            for (int i = 0; i < _animators.Length; i++)
            {
                _animators[i].Play(_animationStates[i], 0);
            }
        }
    }

    bool checkTrigger(GameObject pTest)
    {
        foreach (GameObject @object in _pickupAbleObjects)
        {
            if (pTest == @object)
            {
                return true;
            }
        }

        foreach (string tag in _pickupAbleTags)
        {
            if (pTest.tag == tag)
            {
                return true;
            }
        }

        foreach (int layer in _pickupAbleLayers)
        {
            if (pTest.layer == layer)
            {
                return true;
            }
        }

        foreach (string name in _pickupAbleNames)
        {
            if (pTest.name == name)
            {
                return true;
            }
        }

        return false;
    }
}
