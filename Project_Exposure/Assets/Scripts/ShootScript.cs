using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum Frequency
{
    LOW = 0,
    MEDIUM = 1,
    HIGH = 2,
}

public class ShootScript : MonoBehaviour
{
    [SerializeField] bool _rayMode = false;
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

    [SerializeField] Text _energyCounter;
    [SerializeField] float _startEnergy = 80;
    float _energyCount;
    string _originalEnergyText;

    GameObject _previousHit;
    float _rayTimer = 0;
    float _rayTimerMax = 0.5f;

    Dictionary<Frequency, GameObject> _waves = new Dictionary<Frequency, GameObject>();

    Frequency _shootingFrequency = Frequency.MEDIUM;

    // Start is called before the first frame update
    void Start()
    {
        _energyCount = _startEnergy;
        _originalEnergyText = _energyCounter.text + ": ";
        _energyCounter.text = _originalEnergyText + _energyCount;

        _waves.Add(Frequency.LOW, _bulletType1);
        _waves.Add(Frequency.MEDIUM, _bulletType2);
        _waves.Add(Frequency.HIGH, _bulletType3);
    }

    // Update is called once per frame
    void Update()
    {
        SwitchWave();

        if (!EventSystem.current.IsPointerOverGameObject()) // check if mouse isn't hovering over button
        {
            //Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 10f);

            //transform.LookAt(mouseWorldPosition);

            if (!_rayMode)
            {
                if (Input.GetMouseButtonDown(0) && _energyCount > 0)
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

                    if (_pierceMode){
                        bullet.GetComponent<BulletScript>().PierceShotMode = true;
                    }

                    if (!_batteryMode)
                    {
                        RemoveEnergy();
                    }
                }
            }
            else
            {
                if (Input.GetMouseButton(0))
                {
                    int layerMask = LayerMask.GetMask("Obstacles");
                    if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, _maxRayDistance, layerMask))
                    {
                        GameObject hitObject = hit.transform.gameObject;

                        transform.LookAt(hitObject.transform.position);
                        transform.Rotate(Vector3.right, 90f);

                        _beam.SetActive(true);
                        if (GameObject.ReferenceEquals(_previousHit, hitObject))
                        {
                            _rayTimer += Time.deltaTime;
                            if (_rayTimer >= _rayTimerMax)
                            {
                                hitObject.transform.GetComponent<ObstacleScript>().Shatter();
                                _rayTimer = 0;
                            }
                        }
                        else
                        {
                            _rayTimer = 0;
                        }
                        _previousHit = hitObject;
                    }
                    else
                    {
                        _beam.SetActive(false);
                    }
                }
            }
        }
    }

    public void SwitchWave(int pMode = 0)
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || pMode == 1)
        {
            _shootingFrequency = Frequency.LOW;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) || pMode == 2)
        {
            _shootingFrequency = Frequency.MEDIUM;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) || pMode == 3)
        {
            _shootingFrequency = Frequency.HIGH;
            return;
        }
    }

    public void RemoveEnergy(float pAmount = 1)
    {
        _energyCount -= pAmount;
        _energyCounter.text = _originalEnergyText + _energyCount;
    }
    public void AddEnergy(float pAmount = 1)
    {
        _energyCount += pAmount;
        _energyCounter.text = _originalEnergyText + _energyCount;
    }

    public void EnablePierceShot(){
        _pierceMode = true;
        Invoke("DisablePierceShot", _pierceCooldownTime);
    }

    public void DisablePierceShot(){
        _pierceMode = false;
    }

    public void EnableBattery(){
        _batteryMode = true;
        Invoke("DisableBattery", _batteryCooldownTime);
    }

    public void DisableBattery(){
        _batteryMode = false;
    }
}
