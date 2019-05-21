using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [SerializeField] float _speed = 0.01f;
    [SerializeField] float _shatterForce = 10f;
    [SerializeField] float _shatterDelay = 0.5f;
    [SerializeField] float _shakeForce = 0.05f;
    [SerializeField] Frequency _frequency = Frequency.MEDIUM;

    [SerializeField] Material _lowFreqMaterial;
    [SerializeField] Material _mediumFreqMaterial;
    [SerializeField] Material _highFreqMaterial;
    [SerializeField] List<GameObject> _tutorialZones;

    ScreenShake _screenShake;
    Vector3 _oldPosVector;
    Vector3 _pointOfImpact;
    float _timeBeforeShatter = 0.0f;
    bool _shakingNShatter;
    bool _shaking;

    void Start()
    {
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        _screenShake = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<ScreenShake>();
        
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

    void OnValidate()
    {
        Start();
    }

    void Update()
    {
        if (_shakingNShatter)
        {
            shake(true);
        }
        else if (_shaking)
        {
            shake(false);
        }
    }

    void shake(bool pDestroy)
    {
        Vector3 shakeVector = new Vector3(_oldPosVector.x + Random.Range(-_shakeForce, _shakeForce) / (pDestroy ? 1 : 2),
                                          _oldPosVector.y + Random.Range(-_shakeForce, _shakeForce) / (pDestroy ? 1 : 2),
                                          _oldPosVector.z + Random.Range(-_shakeForce, _shakeForce) / (pDestroy ? 1 : 2));

        transform.localPosition = shakeVector;

        _timeBeforeShatter += Time.deltaTime;

        if (_timeBeforeShatter > (pDestroy ? _shatterDelay : _shatterDelay / 2))
        {
            if (pDestroy)
            {
                Shatter();
            }
            else
            {
                transform.localPosition = _oldPosVector;
                _timeBeforeShatter = 0.0f;
                _shaking = false;
            }
        }
    }

    public void EnableShake(bool pDestroy)
    {
        _oldPosVector = transform.localPosition;

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

            child.gameObject.SetActive(true);
            child.gameObject.layer = 11;
            if (child.GetComponent<ShardScript>() == null)
            {
                child.gameObject.AddComponent<ShardScript>();
            }

            childRigid.isKinematic = false;
            child.GetComponent<Renderer>().material = shardsContainer.GetComponent<Renderer>().material;

            Vector3 direction = (child.position - _pointOfImpact).normalized;
            Vector3 randomizedDirection = new Vector3(direction.x * Random.Range(0.5f, 1.5f), direction.y * Random.Range(0.5f, 1.5f), direction.z * Random.Range(0.5f, 1.5f));

            childRigid.AddForce(randomizedDirection * _shatterForce, ForceMode.Impulse);
            childRigid.angularVelocity = new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f)) * 2;
        }


        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/" + _frequency + "_glassplaceholder", gameObject);
        _screenShake.StartShake(0.2f, 0.1f);
        shardsContainer.DetachChildren();

        GameObject.FindGameObjectWithTag("Player").GetComponent<ShootScript>().AddEnergy(); // regain energy
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
