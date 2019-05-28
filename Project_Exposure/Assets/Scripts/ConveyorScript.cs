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
    MoveAlongBeltScript _shard;
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

    void moveStraight(Transform pObject)
    {
        pObject.position += (-transform.right * _speed) * Time.deltaTime;
    }

    void moveCorner(Transform pObject)
    {
        pObject.RotateAround(_cornerPoint.position, _cornerPoint.up, _rotateAngle * Time.deltaTime);
    }

    public static float Speed
    {
        get
        {
            return _conveyorMaterial.GetFloat("_SpeedY");
        }
        set
        {
            _conveyorMaterial.SetFloat("_SpeedY", value);
        }
    }

    public static Vector2 SpeedVector
    {
        get
        {
            return new Vector2(_conveyorMaterial.GetFloat("_SpeedX"), _conveyorMaterial.GetFloat("_SpeedY"));
        }
        set
        {
            _conveyorMaterial.SetFloat("_SpeedX", value.x);
            _conveyorMaterial.SetFloat("_SpeedY", value.y);
        }
    }
}
