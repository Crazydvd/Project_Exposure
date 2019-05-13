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
	[SerializeField] bool _rayMode = false;

	[SerializeField] GameObject _bulletSpawnPoint;
    [SerializeField] GameObject _bulletType1;
    [SerializeField] GameObject _bulletType2;
    [SerializeField] GameObject _bulletType3;

    [SerializeField] float _speed = 5000f;

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
			else{
				if (Input.GetMouseButton(0))
				{
					RaycastHit hit;
					if(Physics.Raycast(transform.position, transform.up, out hit, Mathf.Infinity)){
						Debug.Log(hit.transform.name);
					}
				}
			}
		}
    }

    public void SwitchWave(int mode = 0)
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) || mode == 1)
        {
            _shootingFrequency = Frequency.LOW;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) || mode == 2)
        {
            _shootingFrequency = Frequency.MEDIUM;
            return;
        }

        if (Input.GetKeyDown(KeyCode.Alpha3) || mode == 3)
        {
            _shootingFrequency = Frequency.HIGH;
            return;
        }
    }
}
