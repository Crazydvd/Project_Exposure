using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarScript : MonoBehaviour
{
    [SerializeField] Sprite _filledStarSprite;
    [SerializeField] float _scoreForOneStar = 5000f;
    [SerializeField] float _scoreForTwoStars = 10000f;
    [SerializeField] float _scoreForThreeStars = 20000f;

    GameObject[] _stars = new GameObject[3];
    Image[] _fillImages = new Image[3];

    int _currentStar = 1; // indexer
    int _fillGoal = 0; // amount of stars to fill
    bool _fillStars = false; // boolean to start
    int _points = 0;

    bool _starAnimationUpscale = false;
    bool _starAnimationDownscale = false;
    RectTransform _currentAnimationTransform;
    int _starAnimationIndex = 0;


    void Start()
    {
        for (int i = 0; i < 3; i++)
        {
            _stars[i] = transform.GetChild(i).gameObject;
            _fillImages[i] = _stars[i].transform.GetChild(0).GetComponent<Image>();
        }
    }

    public void CheckStarScore(float pScore, int pLevel){
        Debug.Log(pScore + " " + pLevel);
        setPointStats(pScore, pLevel);
        if(pScore > _scoreForThreeStars){
            _fillGoal = 3;
        }else if(pScore > _scoreForTwoStars){
            _fillGoal = 2;
        }else if(pScore > _scoreForOneStar){
            _fillGoal = 1;
        }

        if(_fillGoal > 0){
            _fillStars = true;
        }
    }

    public int GetStarScore(){
        return _fillGoal;
    }

    void setPointStats(float pScore, int pLevel){
        int points = 0;

        // calculate the points
        if(pScore >= _scoreForThreeStars){
            points = 50;
        }else if(pScore >= _scoreForTwoStars){
            points = 30 + (int) ((20 / (_scoreForThreeStars - _scoreForTwoStars) * (pScore - _scoreForTwoStars)));
        }else if(pScore >= _scoreForOneStar){
            points = 10 + (int) ((20 / (_scoreForTwoStars - _scoreForOneStar) * (pScore - _scoreForOneStar)));
        }else{
            points = (int)(10 / _scoreForOneStar * pScore);
        }
        _points = points;
        Debug.Log(points);
        switch(pLevel){
            case 1:
                StatsTrackerScript.PointsLevel1 = points;
                break;
            case 2:
                StatsTrackerScript.PointsLevel2 = points;
                break;
            case 3:
                StatsTrackerScript.PointsLevel3 = points;
                break;
        }
    }


    // Update is called once per frame
    void Update()
    {
        if(_fillStars){
            if (_fillImages[_currentStar - 1].fillAmount < 1f) {
                _fillImages[_currentStar - 1].fillAmount += 0.01f;
            }else{
                _stars[_currentStar - 1].GetComponent<Image>().sprite = _filledStarSprite;
                _currentAnimationTransform = _stars[_currentStar - 1].GetComponent<RectTransform>();
                _starAnimationIndex = _currentStar;
                _starAnimationUpscale = true;

                //play the sound
                FMODUnity.RuntimeManager.PlayOneShot("event:/Star");
                if(_currentStar < _fillGoal){
                    _currentStar++;
                }
                else{
                    _fillStars = false;
                }
            }
        }

        if(_starAnimationUpscale){
            if (_currentAnimationTransform.localScale.x < 1.2f)
            {
                _currentAnimationTransform.localScale += new Vector3(0.01f, 0.01f, 0.01f);
            }else{
                _starAnimationDownscale = true;
                _starAnimationUpscale = false;
                _currentAnimationTransform.localScale = new Vector3(1.2f, 1.2f, 1.2f);
            }
        }
        if (_starAnimationDownscale)
        {
            if (_currentAnimationTransform.localScale.x > 1f)
            {
                _currentAnimationTransform.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
            }
            else
            {
                _starAnimationDownscale = false;
                _currentAnimationTransform.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
