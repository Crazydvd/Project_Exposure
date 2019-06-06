using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetShaderProperties : MonoBehaviour
{
    [SerializeField] Vector2 _speed = new Vector2(1, 0);

    Material _material = null;
    float _timeElapsed = 0;

    private bool test = false;

    void Start()
    {
        _material = GetComponent<Renderer>().material;

        _shaderSpeed = _speed;
    }

    void Update()
    {
        _timeElapsed += Time.deltaTime;
        _shaderTimeElapsed = _timeElapsed;
    }

    Vector2 _shaderSpeed
    {
        set
        {
            _material.SetVector("_Speed", new Vector4(value.x, value.y, 0, 0));
        }
    }

    float _shaderTimeElapsed
    {
        get
        {
            return _timeElapsed;
        }
        set
        {
            _material.SetFloat("_TimeElapsed", value);
        }
    }
}
