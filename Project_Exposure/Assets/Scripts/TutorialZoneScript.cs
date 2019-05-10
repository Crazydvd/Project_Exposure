using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialZoneScript : MonoBehaviour
{
	[SerializeField] GameObject _uiElement;

	GameObject _player;
	Animator _playerTrack;
	float _initialSpeed;

	private void OnTriggerEnter(Collider other)
	{
		if(other.tag.ToUpper() == "MAINCAMERA"){
			_player = other.gameObject;
			_playerTrack = _player.GetComponent<Animator>();
			_initialSpeed = _playerTrack.speed;
			_playerTrack.speed = 0;
			_uiElement.SetActive(true);
		}
	}

	public void ReenablePlayer(){
		_playerTrack.speed = _initialSpeed;
		_uiElement.SetActive(false);
		Destroy(gameObject);
	}
}
