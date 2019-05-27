using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerEnableObjectsScript : MonoBehaviour
{
    enum Mode
    {
        ENABLE,
        DISABLE,
        TOGGLE,
    }


    [Header("Objects that you want to interact with")]
    [SerializeField] GameObject[] _objects;

    [SerializeField] Mode _currentMode = Mode.TOGGLE;

    [Space]
    [Header("Whether or not every object can trigger the animation")]
    [SerializeField] bool _allTrigger = false;

    [Header("Objects that can trigger the zone")]
    [SerializeField] GameObject[] _pickupAbleObjects;
    [SerializeField] string[] _pickupAbleTags;
    [SerializeField] int[] _pickupAbleLayers;
    [SerializeField] string[] _pickupAbleNames;

    void OnTriggerEnter(Collider other)
    {
        if (_allTrigger || checkTrigger(other.gameObject))
        {
            switch (_currentMode)
            {
                case Mode.ENABLE:
                    enableObjects();
                    break;
                case Mode.DISABLE:
                    disableObjects();
                    break;
                case Mode.TOGGLE:
                    toggleObjects();
                    break;
            }
        }
    }

    void enableObjects()
    {
        foreach (GameObject @object in _objects)
        {
            @object.SetActive(true);
        }
    }

    void disableObjects()
    {
        foreach (GameObject @object in _objects)
        {
            @object.SetActive(false);
        }
    }

    void toggleObjects()
    {
        foreach (GameObject @object in _objects)
        {
            @object.SetActive(!@object.activeSelf);
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
