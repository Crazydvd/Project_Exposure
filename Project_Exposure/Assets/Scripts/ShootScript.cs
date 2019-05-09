using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Frequency
{
    LOW = -1,
    MEDIUM = 0,
    HIGH = 1,
}

public class ShootScript : MonoBehaviour
{
    [SerializeField]
    GameObject _bulletType1;
    [SerializeField]
    GameObject _bulletType2;
    [SerializeField]
    GameObject _bulletType3;

    [SerializeField]
    float _speed = 5000f;

    Dictionary<Frequency, GameObject> _waves = new Dictionary<Frequency, GameObject>();

    Frequency _shootingFrequency = Frequency.MEDIUM;

    // Start is called before the first frame update
    void Start()
    {
        _waves.Add(Frequency.LOW, _bulletType1);
        _waves.Add(Frequency.MEDIUM, _bulletType2);
        _waves.Add(Frequency.HIGH, _bulletType3);
    }

    // Update is called once per frame
    void Update()
    {
        switchWave();

        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 5f);

        transform.LookAt(mouseWorldPosition);
        transform.Rotate(Vector3.right, 90f);

        if (Input.GetMouseButtonDown(0))
        {
            GameObject bullet = Instantiate(_waves[_shootingFrequency], transform.position, Quaternion.LookRotation(transform.up));
            bullet.GetComponent<Rigidbody>().AddForce(transform.up * _speed);
        }
    }

    private void switchWave()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            _shootingFrequency = Frequency.LOW;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            _shootingFrequency = Frequency.MEDIUM;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            _shootingFrequency = Frequency.HIGH;
            return;
        }
    }
}
