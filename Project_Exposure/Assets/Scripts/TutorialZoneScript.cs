using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TutorialZoneScript : MonoBehaviour
{
    [SerializeField] GameObject _uiElement;
    [SerializeField] bool _stopBelt = false;
    [SerializeField] GameObject _conveyorBelt;
    [SerializeField] ObstacleScript[] _obstacles;

    int _obstacleCount;
    GameObject _player;
    Animator _playerTrack;

    ControlConveyorBelt _control = null;

    void Start()
    {
        foreach (ObstacleScript obstacle in _obstacles)
        {
            obstacle.SetTutorialZone(this);
            _obstacleCount++;
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
            _uiElement.SetActive(true);

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

        _uiElement?.SetActive(false);

        if ( _stopBelt)
        {
            _control.StartBelt();
        }

        Destroy(gameObject);
    }
}
