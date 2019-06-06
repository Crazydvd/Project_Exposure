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

    [SerializeField] bool _rayCastAccuracy = true;
    [SerializeField] GameObject _beam;

    [SerializeField] GameObject _bulletSpawnPoint;
    [SerializeField] GameObject _bulletType1;
    [SerializeField] GameObject _bulletType2;
    [SerializeField] GameObject _bulletType3;

    [SerializeField] float _speed = 300f;
    [SerializeField] float _minRayDistance = 2f;
    [SerializeField] float _maxRayDistance = 15f;
    [SerializeField] float _bulletPointDistance = 5f;

    [SerializeField] bool _pierceMode = false;
    [SerializeField] float _pierceCooldownTime = 5;
    [SerializeField] bool _batteryMode = false;
    [SerializeField] float _batteryCooldownTime = 5;
    public float OverchargeCooldownTime = 15; // should be able to get from the powerupmanager

    [SerializeField] Text _energyCounter;
    [SerializeField] float _startEnergy = 80;
    [SerializeField] float _energyGain = 10f;
    [SerializeField] float _energyRegainSpeed = 0.25f;
    [SerializeField] float _energyRegainDelay = 0.5f;
    float _energyCount;
    float _regainTime = 1f;
    string _energyText;

    GameObject _previousHit;
    float _rayTimer = 0;
    float _rayTimerMax = 0.5f;

    Dictionary<Frequency, GameObject> _waves = new Dictionary<Frequency, GameObject>();

    Frequency _shootingFrequency = Frequency.MEDIUM;

    KnobScript _knobScript;

    // Start is called before the first frame update
    void Start()
    {
        Multiplier = 1;
        SetText();

        _waves.Add(Frequency.LOW, _bulletType1);
        _waves.Add(Frequency.MEDIUM, _bulletType2);
        _waves.Add(Frequency.HIGH, _bulletType3);

        _knobScript = GameObject.Find("Canvas").GetComponentInChildren<KnobScript>();
    }

    // Update is called once per frame
    void Update()
    {
        SwitchWave();

        if (_regainTime > 0)
        {
            _regainTime -= Time.deltaTime;
        }

        if (_energyCount > 0 && _regainTime <= 0)
        {
            RemoveEnergy(_energyRegainSpeed);
        }
        //Debug.Log(Input.touchCount);

        if ((!EventSystem.current.IsPointerOverGameObject() && !isPointerOverUIObject()) || (Input.touchCount > 1 && !EventSystem.current.IsPointerOverGameObject(Input.touches[1].fingerId))) // check if mouse isn't hovering over button
        {
            if ((Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) && _energyCount < 100) //was > 0
            {
                int layerMask = ~LayerMask.GetMask("Player"); // don't hit the Player layer
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


                GameObject bullet = Instantiate(_waves[_shootingFrequency], _bulletSpawnPoint.transform.position, Quaternion.LookRotation(transform.forward));
                Rigidbody bulletRigidbody = bullet.GetComponent<Rigidbody>();
                bulletRigidbody.AddForce((hitPoint - _bulletSpawnPoint.transform.position).normalized * _speed);
                FMODUnity.RuntimeManager.PlayOneShot("event:/" + _shootingFrequency + "_shot");
                if (!_batteryMode)
                {
                    AddEnergy(_energyGain);
                    _regainTime = _energyRegainDelay;
                }

                if (_pierceMode)
                {
                    bullet.GetComponent<BulletScript>().PierceShotMode = true;
                }

                if (IsPoweredUp())
                {
                    bullet.GetComponent<BulletScript>().SetPoweredUp(true);
                }

                //if (!_batteryMode)
                //{
                //    RemoveEnergy();
                //}
            }
        }
    }

    // ensure clicking is blocked for touch
    bool isPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
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

    public void RemoveEnergy(float pAmount = 1)
    {
        _energyCount -= pAmount;
        _energyCounter.text = _energyText + (int) _energyCount;
    }
    public void AddEnergy(float pAmount = 1)
    {
        _energyCount += pAmount;
        _energyCounter.text = _energyText + (int) _energyCount;
    }

    public void EnablePierceShot()
    {
        _pierceMode = true;
        Invoke("DisablePierceShot", _pierceCooldownTime);
    }

    public void DisablePierceShot()
    {
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

    public void SetText()
    {
        _energyCount = _startEnergy;
        _energyText = JsonText.GetText("ENERGYTEXT") + ": ";
        _energyCounter.text = _energyText + (int) _energyCount;
    }

    public bool IsPoweredUp()
    {
        return (_pierceMode || _batteryMode);
    }
}
