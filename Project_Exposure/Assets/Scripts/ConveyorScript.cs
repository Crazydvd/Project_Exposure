using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConveyorScript : MonoBehaviour
{
    [SerializeField] bool _corner = false;
    [SerializeField] GameObject _cornerPoint;
    [SerializeField] float _rotateAngle = -20f;
    [SerializeField] float _speed = 0.5f;

    void OnTriggerStay(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer).ToUpper() == "SHARDS")
        {
            if (_corner)
            {
                other.transform.RotateAround(_cornerPoint.transform.position, _cornerPoint.transform.up, _rotateAngle * Time.deltaTime);
            }
            else
            {
                other.transform.position += (-transform.right * _speed) * Time.deltaTime;
            }
        }
    }
}
