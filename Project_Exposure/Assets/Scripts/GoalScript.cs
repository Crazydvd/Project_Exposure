﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoalScript : MonoBehaviour
{
    [SerializeField] GameObject _endMenu;
    [SerializeField] GameObject _gameUI;
    [SerializeField] Text _endScoreText;
    [SerializeField] ScoreScript _scoreScript;
    [SerializeField] StarScript _starScript;
    [SerializeField] ObstacleCountScript _obstacleCountScript;
    [SerializeField] HighscoreScript _highscoreScript;
    [SerializeField] int _level = 1;

    bool _startScoreAnimation = false;
    float _score;
    float _currentAnimationScore;

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToUpper() == "MAINCAMERA")
        {
            Time.timeScale = 0f;
            _score = _scoreScript.GetScore();
            _currentAnimationScore = 0f;
            _startScoreAnimation = true;

            other.transform.parent.GetComponent<Animator>().speed = 0;
            other.gameObject.GetComponentInChildren<ShootScript>().DisableGun();
            _gameUI.SetActive(false);
            _endMenu.SetActive(true);

            _starScript.CheckStarScore(_score);
            _highscoreScript.SetScore(_score);
            _highscoreScript.SetLevel(_level);
        }
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.F9)){
            PlayerPrefs.DeleteAll();
        }

        if(_startScoreAnimation){
            if (_currentAnimationScore < _score)
            {
                _currentAnimationScore++;
                _endScoreText.text = "Score: " + _currentAnimationScore;
            }
            else{
                int place = _highscoreScript.CheckDailySpot(_score);
                if (place < 11)
                {
                    _endScoreText.color = Color.yellow;
                }
            }
        }
    }
}
