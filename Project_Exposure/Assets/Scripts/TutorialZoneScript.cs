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
        if (other.tag.ToUpper() == "MAINCAMERA" && !_player)
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

            _playerTrack = _player.transform.parent.GetComponent<Animator>();
            //_initialSpeed = _playerTrack.speed;
            _playerTrack.speed = 0;
            _uiElement.SetActive(true);
        }
    }

    public void ReenablePlayer()
    {
        if (_playerTrack != null)
        {
<<<<<<< HEAD
            _playerTrack.speed = 1;
            ConveyorScript.Speed = 1;
=======
            _playerTrack.speed = _initialSpeed;
>>>>>>> 7f90e6cbaae1f792dbfc2a56b9a360b394f95a25
        }

        if (_uiElement != null)
        {
            _uiElement.SetActive(false);
        }
        Destroy(gameObject);
    }
}
