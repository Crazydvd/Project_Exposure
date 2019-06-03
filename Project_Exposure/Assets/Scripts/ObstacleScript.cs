using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleScript : MonoBehaviour
{
    [SerializeField] float _speed = 0.01f;
    [SerializeField] float _shatterForce = 10f;
    [SerializeField] float _shatterDelay = 0.5f;
    [SerializeField] float _shakeForce = 0.05f;
    [SerializeField] [Range(0f, 5f)] float _shakeSpeed = 2f;
    [SerializeField] Frequency _frequency = Frequency.MEDIUM;

    [SerializeField] Material _lowFreqMaterial;
    [SerializeField] Material _mediumFreqMaterial;
    [SerializeField] Material _highFreqMaterial;
    [SerializeField] bool _isBuddy;
    [SerializeField] BuddyScript _buddyScript;

    TutorialZoneScript _tutorialZone;
    ScreenShake _screenShake;
    Vector3 _oldPosVector;
    Text _scoreUI;
    float _timeBeforeShatter = 0.0f;
    float _shakeDelay;
    bool _shakingNShatter;
    bool _shaking;

    void Start()
    {
        _screenShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScreenShake>();
        _scoreUI = GameObject.Find("Score").GetComponent<Text>();
        setMaterial();
    }

    void OnValidate()
    {
        setMaterial();
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

    void setMaterial()
    {
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

    void shake(bool pDestroy)
    {
        Vector3 shakeVector = new Vector3(_oldPosVector.x + Random.Range(-_shakeForce, _shakeForce) / (pDestroy ? 1 : 2),
                                          _oldPosVector.y + Random.Range(-_shakeForce, _shakeForce) / (pDestroy ? 1 : 2),
                                          _oldPosVector.z + Random.Range(-_shakeForce, _shakeForce) / (pDestroy ? 1 : 2));
        //add a shake speed
        if (_shakeDelay >= (5 - (int) _frequency) - _shakeSpeed * Time.deltaTime)
        {
            transform.localPosition = shakeVector;
            _shakeDelay = 0;
        }
        _shakeDelay++;

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
        _tutorialZone?.GetComponent<TutorialZoneScript>().RemoveObstacle();

        Transform shardsContainer = transform.GetChild(0).transform;

        //Shatter and destroy. Decimate and obliterate. Annihilate and eradicate. Erase from existence.
        for (int i = 0; i < shardsContainer.childCount; i++)
        {
            Transform child = shardsContainer.GetChild(i);
            Rigidbody childRigid = child.GetComponent<Rigidbody>();

            child.gameObject.SetActive(true);
            child.gameObject.layer = 11;
            MoveAlongBeltScript moveAlongBeltScript = child.GetComponent<MoveAlongBeltScript>();
            if (moveAlongBeltScript == null)
            {
                child.gameObject.AddComponent<MoveAlongBeltScript>().StartSelfDestruct();
            }
            else
            {
                moveAlongBeltScript.StartSelfDestruct();
            }

            childRigid.isKinematic = false;
            child.GetComponent<Renderer>().material = shardsContainer.GetComponent<Renderer>().material;

            Vector3 direction = (child.position - transform.position).normalized;
            Vector3 randomizedDirection = new Vector3(direction.x * Random.Range(0.5f, 1.5f), direction.y * Random.Range(0.5f, 1.5f), direction.z * Random.Range(0.5f, 1.5f));

            childRigid.AddForce(randomizedDirection * _shatterForce, ForceMode.Impulse);
            childRigid.angularVelocity = new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f)) * 2;
        }


        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/" + _frequency + "_glassplaceholder", gameObject);
        _screenShake.StartShake(0.2f, 0.1f);
        shardsContainer.DetachChildren();

        if (_isBuddy)
        {
            _buddyScript.InitiateCrash();
        }

        GameObject.FindGameObjectWithTag("Player").GetComponent<ShootScript>().AddEnergy(); // regain energy

        _scoreUI.GetComponent<ScoreScript>().IncreaseScore(10f * ShootScript.Multiplier); // add score

        ShootScript.Multiplier += 1; // increase multiplier
        Debug.Log(ShootScript.Multiplier + " multi");
        Destroy(gameObject);
    }

    public Frequency GetFreq()
    {
        return _frequency;
    }

    public void SetTutorialZone(TutorialZoneScript pTutorialZoneScript){
        _tutorialZone = pTutorialZoneScript;
    }
}
