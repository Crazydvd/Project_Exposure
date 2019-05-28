using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongBeltScript : MonoBehaviour
{
    public System.Action<Transform> Move { get; set; }

    ConveyorScript _lastEntered = null;
    Transform _transform = null;

    void Start()
    {
        _transform = gameObject.name.ToLower().Contains("container") ? transform.parent : transform;
    }

    void Update()
    {
        Move?.Invoke(_transform);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag.ToLower() == "conveyorbelt")
        {
            _lastEntered = other.GetComponent<ConveyorScript>();
            if (_lastEntered == null)
            {
                Debug.Log(gameObject.name);
            }
            Move = _lastEntered.MoveObject;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (Move == null)
        {
            if (other.transform.root.tag.ToLower() == "conveyorbelt")
            {
                _lastEntered = other.GetComponent<ConveyorScript>();
                Move = _lastEntered.MoveObject;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.root.tag.ToLower() == "conveyorbelt")
        {
            if (_lastEntered == other.GetComponent<ConveyorScript>())
            {
                Move = null;
            }
        }
    }
}
