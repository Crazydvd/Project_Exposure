using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerAnimationScript : MonoBehaviour
{
    [Header("The Animator which will play the animation")]
    [SerializeField] Animator _animator = null;

    [Header("The name of the animationState to play")]
    [SerializeField] string _animationState;

    [Header("Whether or not every object can trigger the animation")]
    [SerializeField] bool _allTrigger = false;

    [Header("Objects that can trigger the zone")]
    [SerializeField] GameObject[] _pickupAbleObjects;
    [SerializeField] string[] _pickupAbleTags;
    [SerializeField] int[] _pickupAbleLayers;
    [SerializeField] string[] _pickupAbleNames;

    void OnTriggerEnter(Collider other)
    {
        if ((_allTrigger || checkTrigger(other.gameObject)) && _animator != null)
        {
            _animator.Play(_animationState);
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
