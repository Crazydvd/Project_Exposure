using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public enum Frequency
{
    LOW = 0,
    MEDIUM = 1,
    HIGH = 2,
}

public class ShootScript : MonoBehaviour
{
    [SerializeField] private bool _rayMode = false;
    [SerializeField] private GameObject _beam;

    [SerializeField] private GameObject _bulletSpawnPoint;
    [SerializeField] private GameObject _bulletType1;
    [SerializeField] private GameObject _bulletType2;
    [SerializeField] private GameObject _bulletType3;

    [SerializeField] private float _speed = 5000f;
    private GameObject _previousHit;
    private float _rayTimer = 0;
    private float _rayTimerMax = 0.5f;
    private Dictionary<Frequency, GameObject> _waves = new Dictionary<Frequency, GameObject>();
    private Frequency _shootingFrequency = Frequency.MEDIUM;

    // Start is called before the first frame update
    private void Start()
    {
        _waves.Add(Frequency.LOW, _bulletType1);
        _waves.Add(Frequency.MEDIUM, _bulletType2);
        _waves.Add(Frequency.HIGH, _bulletType3);
    }

    // Update is called once per frame
    private void Update()
    {
        SwitchWave();

        if (!EventSystem.current.IsPointerOverGameObject()) // check if mouse isn't hovering over button
        {
            Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 5f);

            transform.LookAt(mouseWorldPosition);
            transform.Rotate(Vector3.right, 90f);

            if (!_rayMode)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    GameObject bullet = Instantiate(_waves[_shootingFrequency], _bulletSpawnPoint.transform.position, Quaternion.LookRotation(transform.up), transform.parent);
                    bullet.GetComponent<Rigidbody>().AddForce(transform.up * _speed);
                }
            }
            else if (Input.GetMouseButton(0))
            {
                int layerMask = LayerMask.GetMask("Obstacles");
                if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hit, Mathf.Infinity, layerMask))
                {
                    Transform hitTransform = hit.transform;
                    transform.LookAt(hitTransform.position);
                    transform.Rotate(Vector3.right, 90f);

                    _beam.SetActive(true);
                    if (ReferenceEquals(_previousHit, hitTransform.gameObject))
                    {
                        _rayTimer += Time.deltaTime;
                        if (_rayTimer >= _rayTimerMax)
                        {
                            hitTransform.GetComponent<ObstacleScript>().Shatter();
                            _rayTimer = 0;
                        }
                    }
                    else
                    {
                        _rayTimer = 0;
                    }
                    _previousHit = hitTransform.gameObject;
                }
                else
                {
                    _beam.SetActive(false);
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
}
