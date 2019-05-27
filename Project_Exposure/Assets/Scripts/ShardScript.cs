using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShardScript : MonoBehaviour
{
    public System.Action<Transform> Action { get; set; }

    ConveyorScript _lastEntered = null;

    void Update()
    {
        Action?.Invoke(gameObject.transform);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.tag.ToLower() == "conveyorbelt")
        {
            _lastEntered = other.GetComponent<ConveyorScript>();
            Action = _lastEntered.Action;
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (Action == null)
        {
            if (other.transform.root.tag.ToLower() == "conveyorbelt")
            {
                _lastEntered = other.GetComponent<ConveyorScript>();
                Action = _lastEntered.Action;
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.transform.root.tag.ToLower() == "conveyorbelt")
        {
            if (_lastEntered == other.GetComponent<ConveyorScript>())
            {
                Action = null;
            }
        }
    }
}
