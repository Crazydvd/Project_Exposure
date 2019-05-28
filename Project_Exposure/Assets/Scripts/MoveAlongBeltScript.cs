using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongBeltScript : MonoBehaviour
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

    public void StartSelfDestruct(){
        Invoke("RemoveRigidbody", 15f);
    }

    public void RemoveRigidbody(){
        Destroy(GetComponent<Rigidbody>());
        Destroy(gameObject, 10f);
    }
}
