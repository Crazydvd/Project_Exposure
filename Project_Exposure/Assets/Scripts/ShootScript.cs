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
    public static float Multiplier = 1;

    [SerializeField] bool _shootingEnabled = true;
    [SerializeField] bool _rayCastAccuracy = true;

    [SerializeField] GameObject _bulletSpawnPoint;
    [SerializeField] GameObject _bulletType1;
    [SerializeField] GameObject _bulletType2;
    [SerializeField] GameObject _bulletType3;

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

    [SerializeField] Text _pierceShotCounter;
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
        Multiplier = 1;

        _waves.Add(Frequency.LOW, _bulletType1);
        _waves.Add(Frequency.MEDIUM, _bulletType2);
        _waves.Add(Frequency.HIGH, _bulletType3);

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
        if(Time.timeScale == 0f){
            return;
        }

        if (!finger.IsOverGui && _shootingEnabled)
        {
            int layerMask = ~LayerMask.GetMask("Player", "PostProcessing"); // don't hit the Player layer or Camera
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
            _rotationDelay = 0;

            if (IsPoweredUp())
            {
                bullet.GetComponent<BulletScript>().SetPoweredUp(true);
            }

            if (_pierceMode)
            {
                bullet.GetComponent<BulletScript>().PierceShotMode = true;

                _pierceShots--;
                _pierceShotCounter.text = _pierceShots.ToString();
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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _knobScript.SetLow();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _knobScript.SetMedium();
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _knobScript.SetHigh();
            return;
        }
    }

    public Frequency SwitchWave(int pMode)
    {
        return _shootingFrequency = (Frequency) pMode;
    }

    public Frequency SwitchWave(Frequency pMode)
    {
        return _shootingFrequency = pMode;
    }

    public void EnablePierceShot()
    {
        _pierceMode = true;
        _pierceShots = _pierceShotAmmo;
        _pierceShotCounter.text = _pierceShots.ToString();
        _pierceShotCounter.gameObject.SetActive(true);
    }

    public void DisablePierceShot()
    {
        _pierceMode = false;
        _pierceShotCounter.gameObject.SetActive(false);
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

    public bool IsPoweredUp()
    {
        return (_pierceMode || _batteryMode);
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
