using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TutorialZoneScript : MonoBehaviour
{
    [SerializeField] GameObject _uiElement;
    [SerializeField] bool _stopBelt = false;
    [SerializeField] bool _enableGun = false;
    [SerializeField] GameObject _conveyorBelt;
    [SerializeField] ObstacleScript[] _obstacles;
    [SerializeField] PowerupPickupScript[] _pickups;

    bool _rotateGun;
    int _obstacleCount;
    GameObject _player;
    Animator _playerTrack;

    ControlConveyorBelt _control = null;

    void Start()
    {
        if (_obstacles.Length > 0)
        {
            foreach (ObstacleScript obstacle in _obstacles)
            {
                obstacle.SetTutorialZone(this);
                _obstacleCount++;
            }
        }
        if(_pickups.Length > 0)
        {
            foreach (PowerupPickupScript powerup in _pickups)
            {
                powerup.SetTutorialZone(this);
                _obstacleCount++;
            }
        }

        if (_stopBelt)
        {
            (_control = GetComponent<ControlConveyorBelt>() ?? gameObject.AddComponent<ControlConveyorBelt>()).AddConveyorBelt(_conveyorBelt);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToUpper() == "MAINCAMERA" && !_player)
        {
            _player = other.gameObject;

            if (_uiElement != null)
            {
                _uiElement.SetActive(true);
            }

            _playerTrack = _player.transform.parent.GetComponent<Animator>();
            _playerTrack.speed = 0;
            _player.GetComponentInChildren<ShootScript>().RotateGunBack();
            //_uiElement.SetActive(true);

            if (_enableGun)
            { // enable the gun, duh
                _player.GetComponentInChildren<ShootScript>().EnableGun();
            }

            if (!_stopBelt)
            {
                return;
            }

            _control.StopBelt();
        }
    }

    // signal that one of the obstacles has been destroyed
    public void RemoveObstacle()
    {
        _obstacleCount--;
        if (_obstacleCount < 1)
        {
            ReenablePlayer();
        }
    }

    public void ReenablePlayer()
    {
        if (_playerTrack)
        {
            _playerTrack.speed = 1;
        }

        if (_uiElement != null)
        {
            _uiElement?.SetActive(false);
        }

        if (_stopBelt)
        {
            _control.StartBelt();
        }

        Destroy(gameObject);
    }
}
