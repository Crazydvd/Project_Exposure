using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BuddyScript : MonoBehaviour
{
    [SerializeField] ObstacleScript _heldObject;
    SphereCollider _collider;
    Animator _animator;
    FMOD.Studio.EventInstance _hovering;

    void Start()
    {
        _collider = GetComponent<SphereCollider>();
        _animator = GetComponentInParent<Animator>();
        _hovering = FMODUnity.RuntimeManager.CreateInstance("event:/hovering");
        FMODUnity.RuntimeManager.AttachInstanceToGameObject(_hovering, transform, GetComponent<Rigidbody>());
        _hovering.start();
        _hovering.release();
    }

    public void InitiateCrash()
    {
        _collider.enabled = true;
        Rigidbody bodyBuddy = GetComponent<Rigidbody>() ?? gameObject.AddComponent<Rigidbody>();
        bodyBuddy.isKinematic = false;
        bodyBuddy.AddForce(-transform.right.normalized * 200);
        bodyBuddy.AddTorque(-transform.right.normalized * 100);
        if (_heldObject)
        {
            _heldObject.gameObject.AddComponent<Rigidbody>();
        }
        _animator.enabled = false;
        _hovering.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/dying", gameObject);
        Invoke("ShatterHeldObject", 1f);
    }

    public void ShatterHeldObject()
    {
        if (_heldObject)
        { 
            _heldObject.Shatter();
        }
    }
}
