using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuddyScript : MonoBehaviour
{
    [SerializeField] ObstacleScript _heldObject;
    SphereCollider _collider;
    Animator _animator;

    void Start()
    {
        _collider = GetComponent<SphereCollider>();
        _animator = GetComponentInParent<Animator>();
    }

    public void InitiateCrash()
    {
        _collider.enabled = true;
        transform.parent.gameObject.AddComponent<Rigidbody>().AddForce(-transform.right.normalized * 200);
        _animator.enabled = false;
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
