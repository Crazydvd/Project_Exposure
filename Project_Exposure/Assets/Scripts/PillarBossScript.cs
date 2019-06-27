using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarBossScript : MonoBehaviour
{
    [SerializeField] GameObject _platformShoot;
    [SerializeField] float _animationSpeed = 1f;
    [SerializeField] float _hurlSpeed = 10f;

    [Header("Projectiles")]
    [SerializeField] GameObject _object1;
    [SerializeField] GameObject _object2;
    [SerializeField] GameObject _object3;

    [Header("Legs")]
    [SerializeField] GameObject[] _obstacles;


    Animator _animator;
    GameObject _mainCamera;
    GameObject _projectile;
    int _obstacleCount;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _mainCamera = Camera.main.gameObject;

        setAnimationSpeed(_animationSpeed);

        foreach (GameObject obstacle in _obstacles)
        {
            
            _obstacleCount++;
        }
    }

    void Update()
    {
        _platformShoot.transform.LookAt(_mainCamera.transform);
        _platformShoot.transform.rotation = Quaternion.Euler(0, _platformShoot.transform.rotation.eulerAngles.y, 0);
    }

    void setAnimationSpeed(float pValue)
    {
        _animator.speed = pValue;
    }

    void spawnProjectile()
    {
        GameObject randomProjectile = _object1; // default object if things go wrong
        int rnd = Random.Range(0, 3);
        switch (rnd)
        {
            case 0:
                randomProjectile = _object1;
                break;
            case 1:
                randomProjectile = _object2;
                break;
            case 2:
                randomProjectile = _object3;
                break;
            default:
                break;
        }

        _projectile = Instantiate(randomProjectile, _platformShoot.transform.GetChild(0).transform.position + new Vector3(0, 0.5f, 0), Quaternion.identity);
        _projectile.transform.SetParent(_platformShoot.transform.GetChild(0), true);
    }

    void launchProjectile()
    {
        //get the vector                        predicted camera postition                            projectile position         little adjustment                   amplify            
        Vector3 force = ((_mainCamera.transform.position + _mainCamera.transform.forward * 2) - _projectile.transform.position + new Vector3(0, 0.5f, 0)).normalized * _hurlSpeed;

        //launch the shit
        _projectile.transform.parent = null;
        Rigidbody rigidbody = _projectile.GetComponentInChildren<Rigidbody>();
        rigidbody.useGravity = false;
        rigidbody.isKinematic = false;
        rigidbody.AddForce(force, ForceMode.Impulse);
    }

    public void RemoveObstacle()
    {
        _obstacleCount--;
        if (_obstacleCount < 1)
        {
            _animator.SetBool("dead", true);
            _platformShoot.SetActive(false);
        }
    }    
}
