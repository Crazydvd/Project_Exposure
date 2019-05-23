using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScript : MonoBehaviour
{
    Text _textUI;
    float _score;

    void Start()
    {
        _textUI = GetComponent<Text>();
        _textUI.text = JsonText.GetText("SCORE") + ": 0";
    }

    public void IncreaseScore(float pIncrease)
    {
        _score += pIncrease;
        _textUI.text = JsonText.GetText("SCORE") + ": " + _score;
    }
}
