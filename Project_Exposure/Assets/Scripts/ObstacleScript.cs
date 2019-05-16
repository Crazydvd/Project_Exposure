using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [SerializeField] float _speed = 1f;
    [SerializeField] float _shatterForce = 10f;
    [SerializeField] float _shatterDelay = 0.25f;
    [SerializeField] Frequency _frequency = Frequency.MEDIUM;

    [SerializeField] Material _lowFreqMaterial;
    [SerializeField] Material _mediumFreqMaterial;
    [SerializeField] Material _highFreqMaterial;
    [SerializeField] List<GameObject> _tutorialZones;

    Vector3 _originalShakePos;
    Vector3 _pointOfImpact;
    float _timeBeforeShatter = 0.0f;
    bool _shakingNShatter;
    bool _shaking;

    void Start()
    {
        /**
        Renderer renderer = GetComponentInChildren<Renderer>();

        switch (_frequency)
        {
            case Frequency.LOW:
                renderer.material = _lowFreqMaterial;
                break;
            case Frequency.MEDIUM:
                renderer.material = _mediumFreqMaterial;
                break;
            case Frequency.HIGH:
                renderer.material = _highFreqMaterial;
                break;
        }
        /**/

        //TEMPORARY:
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        Material material = null;

        switch (_frequency)
        {
            case Frequency.LOW:
                material = _lowFreqMaterial;
                break;
            case Frequency.MEDIUM:
                material = _mediumFreqMaterial;
                break;
            case Frequency.HIGH:
                material = _highFreqMaterial;
                break;
        }

        foreach (Renderer renderer in renderers)
        {
            renderer.material = material;
        }
    }

    void Update()
    {
        if (_shakingNShatter)
        {
            Shake(true);
        }
        else if (_shaking)
        {
            Shake(false);
        }
    }

    private void Shake(bool pDestroy)
    {
        switch (_shakingNShatter)
        {
            case true:
                transform.SetPositionAndRotation(new Vector3(_originalShakePos.x + Random.Range(-0.15f, 0.15f), _originalShakePos.y + Random.Range(-0.15f, 0.15f), _originalShakePos.z + Random.Range(-0.15f, 0.15f)), transform.rotation);
                break;
            case false:
                transform.SetPositionAndRotation(new Vector3(_originalShakePos.x + Random.Range(-0.1f, 0.1f), _originalShakePos.y + Random.Range(-0.1f, 0.1f), _originalShakePos.z + Random.Range(-0.1f, 0.1f)), transform.rotation);
                break;
        }

        _timeBeforeShatter += Time.deltaTime;

        if (_timeBeforeShatter > _shatterDelay)
        {
            if (pDestroy)
            {
                Shatter();
            }
            else
            {
                _shaking = false;
                _timeBeforeShatter = 0.0f;
            }
        }
    }

    public void EnableShake(bool pDestroy)
    {
        _originalShakePos = transform.position;

        switch (pDestroy)
        {
            case true:
                _shakingNShatter = true;
                break;
            case false:
                _shaking = true;
                break;
        }
    }

    public void Shatter()
    {
        //if a tutorial zone is linked to this object, resume gameplay on shatter
        if (_tutorialZones.Count > 0)
        {
            _tutorialZones[0].GetComponent<TutorialZoneScript>().ReenablePlayer();
        }

        Transform shardsContainer = transform.GetChild(0).transform;

        //Shatter and destroy. Decimate and obliterate. Annihilate and eradicate. Erase from existence.
        for (int i = 0; i < shardsContainer.childCount; i++)
        {
            Transform child = shardsContainer.GetChild(i);
            Rigidbody childRigid = child.GetComponent<Rigidbody>();

            if (childRigid == null)
                Debug.Log("YOU FORGOT TO ADD KINEMATIC RIGIDBODY TO THE CHILD!!!");

            child.gameObject.SetActive(true);
            childRigid.isKinematic = false;
            child.GetComponent<Renderer>().material = shardsContainer.GetComponent<Renderer>().material;

            Vector3 direction = (child.position - _pointOfImpact).normalized;
            Vector3 randomizedDirection = new Vector3(direction.x * Random.Range(0.5f, 1.5f), direction.y * Random.Range(0.5f, 1.5f), direction.z * Random.Range(0.5f, 1.5f));

            childRigid.AddForce(randomizedDirection * _shatterForce, ForceMode.Impulse);
            childRigid.angularVelocity = new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f)) * 2;
        }

        shardsContainer.DetachChildren();
        Destroy(gameObject);
    }

    public Frequency GetFreq()
    {
        return _frequency;
    }

    public void SetPOI(Vector3 pVec)
    {
        _pointOfImpact = pVec;
    }
}
