using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    [SerializeField] Transform _grabPosition = null;

    Transform _grabbedObject = null;
    Transform _canGrab = null;

    Transform _originalParent;
    Rigidbody _rigidbody;

    //[Header("Snap the object to the grab position")]
    //[SerializeField] bool _snapPosition = false;

    [Header("Pickupable Object")]
    [SerializeField] GameObject[] _pickupAbleObjects;
    [SerializeField] string[] _pickupAbleTags;
    [SerializeField] int[] _pickupAbleLayers;
    [SerializeField] string[] _pickupAbleNames;

    void Start()
    {
        if (_grabPosition.childCount > 0)
        {
            _grabbedObject = _grabPosition.GetChild(0);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Transform test = other.transform;
        if (_canGrab == null && _grabbedObject == null && checkPickupAble(test.gameObject))
        {
            _canGrab = test;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (_canGrab != null && (_canGrab == other.transform || _canGrab.GetChild(0) == other.transform))
        {
            _canGrab = null;
        }
    }

    public void Pickup()
    {
        if (_canGrab == null || _grabbedObject != null)
        {
            return;
        }

        _grabbedObject = _canGrab;
        (_rigidbody = _grabbedObject.GetComponent<Rigidbody>()).isKinematic = true;

        if (_grabbedObject.gameObject.name.ToLower().Contains("container"))
        {
            _grabbedObject.GetComponent<MoveAlongBeltScript>().Move = null;
            _originalParent = _grabbedObject.transform.parent.parent;
            _grabbedObject.parent.SetParent(_grabPosition);
        }
        else
        {
            _originalParent = _grabbedObject.transform.parent;
            _grabbedObject.SetParent(_grabPosition);
        }
    }

    public void Drop()
    {
        if (_grabbedObject == null)
        {
            return;
        }

        if (_grabbedObject.gameObject.name.ToLower().Contains("container"))
        {
            _grabbedObject.transform.parent.SetParent(_originalParent);
        }
        else
        {
            _grabbedObject.transform.SetParent(_originalParent);
        }

        _rigidbody.isKinematic = false;
        _grabbedObject = null;
    }

    bool checkPickupAble(GameObject pTest)
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
