using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAlongBeltScript : MonoBehaviour
{
    public System.Action<Transform> Move;

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
        if (other.transform.root.tag.ToLower() == "conveyorbelt" || other.tag.ToLower() == "conveyorbelt")
        {
            _lastEntered = other.GetComponent<ConveyorScript>() ?? other.gameObject.AddComponent<ConveyorScript>();
            Move = _lastEntered.MoveObject;
        }
    }

    void OnTriggerExit(Collider other)
    {
        //if (other.transform.root.tag.ToLower() == "conveyorbelt")
        //{
        //    if (_lastEntered == other.GetComponent<ConveyorScript>())
        //    {
        //        Move = null;
        //    }
        //}
    }

    public void StartSelfDestruct()
    {
        Destroy(gameObject, 25f);
    }
}
