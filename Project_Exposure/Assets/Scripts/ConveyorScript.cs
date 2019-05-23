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
    static Material _conveyorMaterial;

    public System.Action<Transform> Action { get; private set; }

    void Start()
    {
        _cornerPoint = _corner ? transform.GetChild(0) : null;

        Action = _corner ? moveCorner : (System.Action<Transform>) moveStraight;

        if (_conveyorMaterial == null)
        {
            _conveyorMaterial = GetComponent<Renderer>().material;
        }
    }

    void moveStraight(Transform pShard)
    {
        pShard.position += (-transform.right * _speed) * Time.deltaTime;
    }

    void moveCorner(Transform pShard)
    {
        pShard.RotateAround(_cornerPoint.position, _cornerPoint.up, _rotateAngle * Time.deltaTime);
    }

    public static float Speed
    {
        get
        {
            return _conveyorMaterial.GetFloat("TimeScaleY");
        }
        set
        {
            _conveyorMaterial.SetFloat("TimeScaleY", value);
        }
    }

    public static Vector2 SpeedVector
    {
        get
        {
            return new Vector2(_conveyorMaterial.GetFloat("TimeScaleX"), _conveyorMaterial.GetFloat("TimeScaleY"));
        }
        set
        {
            _conveyorMaterial.SetFloat("TimeScaleX", value.x);
            _conveyorMaterial.SetFloat("TimeScaleY", value.y);
        }
    }
}
