using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetShaderProperties : MonoBehaviour
{
    [SerializeField] Vector2 _speed = new Vector2(1, 0);
    [SerializeField] Vector3 _offset = new Vector3(0, 0, 0);

    Material _material = null;
    float _timeElapsed = 0;

    static byte _stencilRef = 1;

    void Start()
    {
        _material = GetComponent<Renderer>().material;

        _shaderSpeed = _speed;

        _stencilRef++;
        _stencilRef = (_stencilRef == 0) ? (byte) (_stencilRef + 1) : _stencilRef;

        _material.SetFloat("_StencilRef", _stencilRef);
        _material.SetVector("_OutlineOffset", new Vector4(_offset.x, _offset.y, _offset.z, 1));
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
