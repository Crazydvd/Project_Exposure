using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Frequency : int
{
    LOW = 0,
    MEDIUM = 1,
    HIGH = 2,
}

public class ShootScript : MonoBehaviour
{
    public static float Multiplier = 0;

    [SerializeField] bool _shootingEnabled = true;
    [SerializeField] bool _rayCastAccuracy = true;

    [SerializeField] GameObject _bulletSpawnPoint;
    [SerializeField] GameObject _bulletType1;
    [SerializeField] GameObject _bulletType2;
    [SerializeField] GameObject _bulletType3;

    [Header("Particles here")]
    [SerializeField] GameObject _gunParticles;
    [SerializeField] GameObject _flashLow;
    [SerializeField] GameObject _flashMedium;
    [SerializeField] GameObject _flashHigh;
    GameObject _lastMuzzleFlash;

    [Header("Colors of the particle system")]
    [SerializeField] Gradient _colorLow;
    [SerializeField] Gradient _colorMedium;
    [SerializeField] Gradient _colorHigh;

    [Space]
    [SerializeField] float _speed = 300f;
    [SerializeField] float _minRayDistance = 2f;
    [SerializeField] float _maxRayDistance = 15f;
    [SerializeField] float _bulletPointDistance = 5f;

    [SerializeField] bool _pierceMode = false;
    [SerializeField] float _pierceShotAmmo = 5;
    [SerializeField] bool _batteryMode = false;
    [SerializeField] float _batteryCooldownTime = 5;
    [SerializeField] float _timeBeforeRotatingGunBack = 2;
    public float OverchargeCooldownTime = 15; // should be able to get from the powerupmanager
    float _pierceShots;
    float _rotationDelay;
    bool _rotateGun;

    GameObject _pierceUI;
    //[SerializeField] Text _energyCounter; Not used
    //[SerializeField] Slider _overheatBar;
    //[SerializeField] float _startEnergy = 80;
    //[SerializeField] float _energyGain = -1f;
    //[SerializeField] float _energyRegainSpeed = 0.25f;
    //[SerializeField] float _energyRegainDelay = 0.5f;
    //float _energyCount;
    //float _regainTime = 1f;
    //string _energyText;

    Dictionary<Frequency, GameObject> _waves = new Dictionary<Frequency, GameObject>();

    Frequency _shootingFrequency = Frequency.MEDIUM;

    KnobScript _knobScript;

    // Start is called before the first frame update
    void Start()
    {
        _pierceUI = GameObject.Find("Canvas").transform.Find("Pierce").gameObject;

        _waves.Add(Frequency.LOW, _bulletType1);
        _waves.Add(Frequency.MEDIUM, _bulletType2);
        _waves.Add(Frequency.HIGH, _bulletType3);
        _lastMuzzleFlash = _flashMedium;

        Lean.Touch.LeanTouch.OnFingerTap += OnFingerTap;

        _knobScript = GameObject.Find("Canvas").GetComponentInChildren<KnobScript>();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchWave();
        rotateGunToZero();
    }

    // Detect (touch/mouse) input
    void OnFingerTap(Lean.Touch.LeanFinger finger)
    {
        if (Time.timeScale == 0f)
        {
            return;
        }

        if (!finger.IsOverGui && _shootingEnabled)
        {
            int layerMask = 1 << 10; //Only raycast the obstacle layer
            //int layerMask = ~LayerMask.GetMask("Player", "PostProcessing", "Projectiles", "Shards"); // don't hit the Player layer or Camera
            Ray rayPoint = Camera.main.ScreenPointToRay(Input.mousePosition);
            Vector3 hitPoint;
            if ((Physics.Raycast(rayPoint, out RaycastHit hit, _maxRayDistance, layerMask) && _rayCastAccuracy))
            {
                transform.LookAt(hit.point);
                hitPoint = hit.point;
            }
            else
            {
                hitPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _bulletPointDistance));
                transform.LookAt(hitPoint);
                Debug.DrawLine(_bulletSpawnPoint.transform.position, hitPoint, Color.red);
            }

            if ((transform.position - hitPoint).magnitude < _minRayDistance)
            {
                hitPoint = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _bulletPointDistance));
                transform.LookAt(hitPoint);
            }

            //Shoot the shit here
            GameObject bullet = Instantiate(_waves[_shootingFrequency], _bulletSpawnPoint.transform.position, Quaternion.LookRotation(transform.forward));
            Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
            bulletRigidbody.AddForce((hitPoint - _bulletSpawnPoint.transform.position).normalized * _speed);
            FMODUnity.RuntimeManager.PlayOneShot("event:/" + _shootingFrequency + "_shot");
            _lastMuzzleFlash.GetComponent<ParticleSystem>().Play();
            _rotationDelay = 0;

            if (_pierceMode)
            {
                bullet.GetComponent<BulletScript>().PierceShotMode = true;

                _pierceShots--;

                PowerUpIconScript.PierceShotsLeft = _pierceShots;

                if (_pierceShots <= 0)
                {
                    DisablePierceShot();
                }
            }

        }
    }

    void OnDisable()
    {
        Lean.Touch.LeanTouch.OnFingerTap -= OnFingerTap;
    }

    public void SwitchWave()
    {
        ParticleSystem.ColorOverLifetimeModule particleColor = _gunParticles.GetComponent<ParticleSystem>().colorOverLifetime;
        particleColor.enabled = true;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _knobScript.SetLow();
            _lastMuzzleFlash = _flashLow;
            particleColor.color = _colorLow;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _knobScript.SetMedium();
            _lastMuzzleFlash = _flashMedium;
            particleColor.color = _colorMedium;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _knobScript.SetHigh();
            _lastMuzzleFlash = _flashHigh;
            particleColor.color = _colorHigh;
            return;
        }
    }

    public Frequency SwitchWave(int pMode)
    {
        return _shootingFrequency = (Frequency) pMode;
    }

    public Frequency SwitchWave(Frequency pMode)
    {
        ParticleSystem.ColorOverLifetimeModule particleColor = _gunParticles.GetComponent<ParticleSystem>().colorOverLifetime;
        particleColor.enabled = true;

        switch (pMode)
        {
            case Frequency.LOW:
                _lastMuzzleFlash = _flashLow;
                particleColor.color = _colorLow;
                break;
            case Frequency.MEDIUM:
                _lastMuzzleFlash = _flashMedium;
                particleColor.color = _colorMedium;
                break;
            case Frequency.HIGH:
                _lastMuzzleFlash = _flashHigh;
                particleColor.color = _colorHigh;
                break;
            default:
                break;
        }

        return _shootingFrequency = pMode;
    }

    public Frequency GetWave()
    {
        return _shootingFrequency;
    }

    public void EnablePierceShot()
    {
        _pierceUI.SetActive(true);

        _pierceMode = true;
        _pierceShots = _pierceShotAmmo;

        PowerUpIconScript.MaxPierceShots = _pierceShotAmmo;
    }

    public void DisablePierceShot()
    {
        _pierceUI.SetActive(false);

        _pierceMode = false;
    }

    public void EnableBattery()
    {
        _batteryMode = true;
        Invoke("DisableBattery", _batteryCooldownTime);
    }

    public void DisableBattery()
    {
        _batteryMode = false;
    }

    public void EnableGun()
    {
        _shootingEnabled = true;
    }

    public void DisableGun()
    {
        _shootingEnabled = false;
    }

    public void RotateGunBack()
    {
        _rotateGun = true;
    }

    void rotateGunToZero()
    {
        if (_rotationDelay < _timeBeforeRotatingGunBack)
        {
            _rotationDelay += Time.deltaTime;
        }
        else
        {
            _rotateGun = true;
        }

        if (!_rotateGun)
            return;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, Quaternion.identity, 0.1f);

        if (transform.localRotation.x - Quaternion.identity.x < 0.1f)
        {
            _rotateGun = false;
        }
    }
}
