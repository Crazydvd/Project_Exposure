using System.Collections;
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

    void OnTriggerEnter(Collider other)
    {
        if (other.tag.ToUpper() == "MAINCAMERA")
        {
            float score = _scoreScript.GetScore();
            other.transform.parent.GetComponent<Animator>().speed = 0;
            _endScoreText.text = "Score: " + score;
            other.gameObject.GetComponentInChildren<ShootScript>().DisableGun();
            _gameUI.SetActive(false);
            _endMenu.SetActive(true);

            _starScript.CheckStarScore(score);
            _highscoreScript.AddEntry("Naam", score, _level);
        }
    }
}
