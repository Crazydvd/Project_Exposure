using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinyWheely : MonoBehaviour
{
    [SerializeField] float _rotationSpeed = 120f;

    void Update()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).Rotate(Vector3.right, _rotationSpeed * Time.deltaTime); 
        }
    }
}
