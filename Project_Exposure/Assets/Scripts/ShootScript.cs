using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{
	[SerializeField]
	GameObject _bullet;
    [SerializeField]
    float _speed = 5000f;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
		Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition + Vector3.forward * 5f);

		transform.LookAt(mouseWorldPosition);
		transform.Rotate(Vector3.right, 90f);

		if(Input.GetMouseButtonDown(0)){
			GameObject bullet = Instantiate(_bullet, transform.position, Quaternion.LookRotation(transform.up));
			bullet.GetComponent<Rigidbody>().AddForce(transform.up * _speed);
		}
	}
}
