using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [SerializeField]
    float _speed = 1f;

    [SerializeField]
    Frequency _frequency = Frequency.MEDIUM;

    [SerializeField]
    Material _lowFreqMaterial;
    [SerializeField]
    Material _mediumFreqMaterial;
    [SerializeField]
    Material _highFreqMaterial;

    Renderer _renderer;
    Animator _animator;


    void Start()
    {
        _animator = GetComponent<Animator>();
        _renderer = GetComponent<Renderer>();
        _animator.speed = _speed;

        switch (_frequency)
        {
            case Frequency.LOW:
                _renderer.material = _lowFreqMaterial;
                break;
            case Frequency.MEDIUM:
                _renderer.material = _mediumFreqMaterial;
                break;
            case Frequency.HIGH:
                _renderer.material = _highFreqMaterial;
                break;
            default:
                break;
        }        
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "MainCamera")
        {
            Debug.Log("u dede");
            Shatter();
        }

        switch (_frequency)
        {
            case Frequency.LOW:
                if (other.transform.tag == "LowFreq")
                    Shatter();
                break;
            case Frequency.MEDIUM:
                if (other.transform.tag == "MediumFreq")
                    Shatter();
                break;
            case Frequency.HIGH:
                if (other.transform.tag == "HighFreq")
                    Shatter();
                break;
            default:
                break;
        }
    }

    void Shatter()

    {
        //just destroy it for now
        Destroy(gameObject);
    }
}
