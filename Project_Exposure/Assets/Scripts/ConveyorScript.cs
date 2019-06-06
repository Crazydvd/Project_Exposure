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

    Transform _cornerPoint = null;
    MoveAlongBeltScript _shard = null;
    Material _conveyorMaterial = null;
    float _timeElapsed = 0;

    public System.Action<Transform> MoveObject { get; private set; }
    float _oldSpeed;

    void Start()
    {
        _cornerPoint = _corner ? transform.GetChild(0) : null;

        MoveObject = _corner ? moveCorner : (System.Action<Transform>) moveStraight;

        _conveyorMaterial = GetComponent<Renderer>().material;

        Speed = _speed;
        _oldSpeed = _speed;
    }

    void Update()
    {
        timeElapsed = _timeElapsed += Time.deltaTime;
    }

    void moveStraight(Transform pObject)
    {
        pObject.position += -transform.right * Time.deltaTime * _speed;
    }

    void moveCorner(Transform pObject)
    {
        pObject.RotateAround(_cornerPoint.position, _cornerPoint.up, _rotateAngle * Time.deltaTime);
    }

    public float Speed
    {
        get
        {
            return _speed;
        }
        set
        {
            _speed = value;
            _conveyorMaterial.SetFloat("_SpeedY", value);
        }
    }

    float timeElapsed
    {
        get
        {
            return _timeElapsed;
        }
        set
        {
            _conveyorMaterial.SetFloat("_TimeElapsed", value);
        }
    }

    void OnDestroy()
    {
        Destroy(_conveyorMaterial);
    }

    public float GetOldSpeed()
    {
        return _oldSpeed;
    }
}
