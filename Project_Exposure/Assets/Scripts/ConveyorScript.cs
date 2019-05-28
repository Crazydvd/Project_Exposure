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
    Material _conveyorMaterial;
    float _timeElapsed = 0;

    public System.Action<Transform> MoveObject { get; private set; }

    void Start()
    {
        _cornerPoint = _corner ? transform.GetChild(0) : null;

        MoveObject = _corner ? moveCorner : (System.Action<Transform>) moveStraight;

        _conveyorMaterial = GetComponent<Renderer>().material;

        TimePerUnit = _speed;
    }

    void Update()
    {
        timeElapsed = _timeElapsed += Time.deltaTime;
    }

    void moveStraight(Transform pObject)
    {
        // _deltaTime / _Time == Seconds per Unit
        // _deltaTime * Speed == Units per Second
        // (Time = 1 / Speed) && (Speed = 1 / Time)
        pObject.position += -transform.right * Time.deltaTime * _speed;
    }

    void moveCorner(Transform pObject)
    {
        pObject.RotateAround(_cornerPoint.position, _cornerPoint.up, _rotateAngle * Time.deltaTime);
    }

    //TODO: 
    /**Find a way to set the speed seperately per conveyorbelt
     **/
    /**Also find a way to make the conveyorBelt be affected by slowdown (so need to use DeltaTime in a way)
     * **/
    /**and keep in mind that the speed of objects on the belt should match the belt (beltSpeed = objectSpeed / 2)
     * **/
    public float TimePerUnit
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
            return _conveyorMaterial.GetFloat("_TimeElapsed");
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
}
