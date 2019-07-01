using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowPlayer : MonoBehaviour
{
    GameObject _player;

    void Start()
    {
        _player = Camera.main.gameObject;
    }

    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, 0.1f);
    }
}
