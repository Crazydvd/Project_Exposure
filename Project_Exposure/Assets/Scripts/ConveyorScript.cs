using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ConveyorScript : MonoBehaviour
{
    [Header("General variables")]
    [SerializeField] bool _corner = false;

    [Header("Normal variables")]
    [SerializeField] float _speed = 0.5f;

    [Header("Corner variables")]
    [SerializeField] float _rotateAngle = -20f;

    Transform _cornerPoint;
    ShardScript _shard;

    public System.Action<Transform> Action { get; private set; }

    void Start()
    {
        _cornerPoint = _corner ? transform.GetChild(0) : null;

        Action = _corner ? moveCorner : (System.Action<Transform>) moveStraight;
    }

    void moveStraight(Transform pShard)
    {
        pShard.position += (-transform.right * _speed) * Time.deltaTime;
    }

    void moveCorner(Transform pShard)
    {
        pShard.RotateAround(_cornerPoint.position, _cornerPoint.up, _rotateAngle * Time.deltaTime);
    }
}
