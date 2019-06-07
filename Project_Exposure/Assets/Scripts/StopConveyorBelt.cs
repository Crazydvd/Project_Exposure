using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopConveyorBelt : ControlConveyorBelt
{
    enum STOPMODE
    {
        STOP,
        START,
    }

    [Header("What to do when something enters the trigger")]
    [SerializeField] STOPMODE _stopMode = STOPMODE.STOP;

    [Header("reverse the effect when you leave the trigger?")]
    [SerializeField] bool _reverse = false;

    [Space]
    [Header("Whether or not every object can trigger the zone")]
    [SerializeField] bool _allTrigger = true;

    [Header("Objects that can trigger the zone")]
    [SerializeField] GameObject[] _triggerAbleObjects;
    [SerializeField] string[] _triggerAbleTags;
    [SerializeField] int[] _triggerAbleLayers;
    [SerializeField] string[] _triggerAbleNames;

    void OnTriggerEnter(Collider other)
    {
        if (_allTrigger || checkTrigger(other.gameObject))
        {
            if (_stopMode == STOPMODE.STOP)
            {
                StopBelt();
            }
            else
            {
                StartBelt();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (_reverse && (_allTrigger || checkTrigger(other.gameObject)))
        {
            if (_stopMode == STOPMODE.STOP)
            {
                StartBelt();
            }
            else
            {
                StopBelt();
            }
        }
    }

    bool checkTrigger(GameObject pTest)
    {
        foreach (GameObject @object in _triggerAbleObjects)
        {
            if (pTest == @object)
            {
                return true;
            }
        }

        foreach (string tag in _triggerAbleTags)
        {
            if (pTest.tag == tag)
            {
                return true;
            }
        }

        foreach (int layer in _triggerAbleLayers)
        {
            if (pTest.layer == layer)
            {
                return true;
            }
        }

        foreach (string name in _triggerAbleNames)
        {
            if (pTest.name == name)
            {
                return true;
            }
        }

        return false;
    }
}
