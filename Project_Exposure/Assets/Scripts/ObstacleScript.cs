using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObstacleScript : MonoBehaviour
{
    [Header("Shatter Options")]
    [SerializeField] float _shatterForce = 10f;
    [SerializeField] float _shatterDelay = 0.5f;
    [SerializeField] float _shakeForce = 0.05f;

    [Header("Frequency")]
    [SerializeField] Frequency _frequency = Frequency.MEDIUM;
    [SerializeField] Material _lowFreqMaterial;
    [SerializeField] Material _mediumFreqMaterial;
    [SerializeField] Material _highFreqMaterial;
    [SerializeField] Material _lowFreqStandard;
    [SerializeField] Material _mediumFreqStandard;
    [SerializeField] Material _highFreqStandard;

    [Header("Specific Objeccts")]
    [SerializeField] GameObject _content;
    [SerializeField] bool _isVending;
    [SerializeField] bool _isBuddy;
    [SerializeField] BuddyScript _buddyScript;

    [Header("Score")]
    [SerializeField] int _scoreForBreaking = 10;
    [SerializeField] [Range(0, 1)] float _scoreLossPercent = 0.05f;

    TutorialZoneScript _tutorialZone;
    ScreenShake _screenShake;
    Vector3 _oldPosVector;
    Text _scoreUI;
    float _timeBeforeShatter = 0.0f;
    bool _shakingNShatter;
    bool _shaking;

    void Start()
    {
        _screenShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScreenShake>();
        _scoreUI = GameObject.Find("Score").GetComponent<Text>();
        setMaterial();
        transform.GetChild(0).gameObject.layer = LayerMask.NameToLayer("Obstacles");
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

        transform.GetChild(0).localPosition = shakeVector;

        _timeBeforeShatter += Time.deltaTime;

        if (_timeBeforeShatter > (pDestroy ? _shatterDelay : _shatterDelay / 2))
        {
            if (pDestroy)
            {
                Shatter();
            }
            else
            {
                transform.GetChild(0).localPosition = _oldPosVector;
                _timeBeforeShatter = 0.0f;
                _shaking = false;
            }
        }
    }

    void launchContent()
    {
        if (_content)
        {
            Transform contentContainer = _content.transform;
            //Shatter and destroy. Decimate and obliterate. Annihilate and eradicate. Erase from existence.
            for (int i = 0; i < contentContainer.childCount; i++)
            {
                Transform child = contentContainer.GetChild(i);
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

                float upward = (float) Random.Range(-100, 100) / 10;

                childRigid.AddForce(-transform.right * (_shatterForce / 2) + new Vector3(0, upward, 0), ForceMode.Impulse);
                childRigid.angularVelocity = new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f)) * 2;
            }
        }
    }

    public void EnableShake(bool pDestroy)
    {
        _oldPosVector = transform.GetChild(0).localPosition;

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

    public void Shatter(bool pLoseScore = false)
    {
        //if a tutorial zone is linked to this object, resume gameplay on shatter
        _tutorialZone?.RemoveObstacle();

        GameObject.Find("Canvas").GetComponentInChildren<ShatterMultiplierVisualScript>().SetFollowObject(transform.GetChild(0)); // oof sorry, didn't wanna drag another thing in the editor

        Transform shardsContainer = transform.GetChild(0).transform;

        launchContent();

        //Shatter and destroy. Decimate and obliterate. Annihilate and eradicate. Erase from existence.
        for (int i = 0; i < shardsContainer.childCount; i++)
        {
            Transform child = shardsContainer.GetChild(i);
            Rigidbody childRigid = child.GetComponent<Rigidbody>();

            child.gameObject.SetActive(true);
            child.gameObject.layer = 11; //layer 11 == shards
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
            //Set the material for the shards
            switch (_frequency)
            {
                case Frequency.LOW:
                    child.GetComponent<Renderer>().material = _lowFreqStandard;
                    break;
                case Frequency.MEDIUM:
                    child.GetComponent<Renderer>().material = _mediumFreqStandard;
                    break;
                case Frequency.HIGH:
                    child.GetComponent<Renderer>().material = _highFreqStandard;
                    break;
            }

            Vector3 direction = (child.position - transform.position).normalized;
            Vector3 randomizedDirection = new Vector3(direction.x * Random.Range(0.5f, 1.5f), direction.y * Random.Range(0.5f, 1.5f), direction.z * Random.Range(0.5f, 1.5f));

            childRigid.AddForce(randomizedDirection * _shatterForce, ForceMode.Impulse);
            childRigid.angularVelocity = new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f)) * 2;
        }


        _screenShake.StartShake(12f, 0.1f);
        shardsContainer.DetachChildren();
        //Play sounds
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/" + _frequency.ToString().ToLower() + "_shattering", gameObject);

        if (_isVending)
        {
            FMODUnity.RuntimeManager.PlayOneShotAttached("event:/Vending machine", gameObject);
        }

        if (_isBuddy)
        {
            _buddyScript.InitiateCrash();
        }

        //GameObject.FindGameObjectWithTag("Player").GetComponent<ShootScript>().AddEnergy(); // regain energy
        ScoreScript score = _scoreUI.GetComponent<ScoreScript>();
        if (!pLoseScore)
        {
            score.IncreaseScore(_scoreForBreaking * ShootScript.Multiplier); // add score
            ShootScript.Multiplier += 1; // increase multiplier
        }
        else
        {
            ShootScript.Multiplier = 1;
            //score.DecreaseScore(30);
            score.MultiplyScore(1 - _scoreLossPercent);
        }

        Destroy(gameObject);
    }

    public Frequency GetFreq()
    {
        return _frequency;
    }

    public void SetTutorialZone(TutorialZoneScript pTutorialZoneScript)
    {
        _tutorialZone = pTutorialZoneScript;
    }
}
