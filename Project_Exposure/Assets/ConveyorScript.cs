using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ConveyorScript : MonoBehaviour
{
    [SerializeField] bool _corner = false;
    [SerializeField] GameObject _cornerPoint;
    [SerializeField] float _rotateAngle = -20f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame

     private void OnTriggerStay(Collider other)
     {
        if (LayerMask.LayerToName(other.gameObject.layer).ToUpper() == "Shards".ToUpper())
        {
            if (_corner)
            {
                other.transform.RotateAround(_cornerPoint.transform.position, _cornerPoint.transform.up, _rotateAngle * Time.deltaTime);
            }
            else
            {
                other.transform.position += (-transform.right * 0.01f);
            }
        }
    }
}
