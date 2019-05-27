using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Timeline;
using UnityEngine.UI;

public class TutorialZoneScript : MonoBehaviour
{
    [SerializeField] GameObject _uiElement;
    GameObject _player;
    Animator _playerTrack;
    float _initialSpeed;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToUpper() == "MAINCAMERA")
        {
            _player = other.gameObject;

            //_player.GetComponent<CinemachineBrain>().ActiveVirtualCamera.VirtualCameraGameObject;
            //_initialSpeed = _playerTrack.speed;
            //_playerTrack.speed = 0;
            //_player.GetComponent<CinemachineTrack>().GetChildTracks(
            if (_uiElement != null)
            {
                _uiElement.SetActive(true);
            }

            _playerTrack = _player.GetComponent<Animator>();
            _initialSpeed = _playerTrack.speed;
            _playerTrack.speed = 0;
            ConveyorScript.Speed = 0;
            _uiElement.SetActive(true);
        }
    }

    public void ReenablePlayer()
    {
        if (_playerTrack != null)
        {
            _playerTrack.speed = _initialSpeed;
            ConveyorScript.Speed = 1;
        }

        if (_uiElement != null)
        {
            _uiElement.SetActive(false);
        }
        Destroy(gameObject);
    }
}
