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
            _heldObject.transform.GetChild(0).gameObject.AddComponent<Rigidbody>();
            Rigidbody rigidbody = _heldObject.transform.GetChild(0).gameObject.GetComponent<Rigidbody>();
            rigidbody.useGravity = true;
            rigidbody.isKinematic = false;
        }
        _animator.enabled = false;
        _hovering.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/dying", gameObject);

    }
}
