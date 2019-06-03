using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdminControlsCameraScript : MonoBehaviour
{
    float timeAdded = 1;

    Animator _animator;
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            _animator.speed += 0.1f;
        }
        else if (Input.GetKeyUp(KeyCode.RightArrow))
        {
            _animator.speed = 1;
        }
    }
}
