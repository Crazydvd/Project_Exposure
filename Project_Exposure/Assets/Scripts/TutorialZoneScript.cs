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
        }
    }

    public void ReenablePlayer()
    {
        if (_playerTrack != null)
        {
            _playerTrack.speed = 1;
        }

        if (_uiElement != null)
        {
            _uiElement.SetActive(false);
        }
        Destroy(gameObject);
    }
}
