using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarBarScript : MonoBehaviour
{
    [SerializeField] float _oneStarScore;
    [SerializeField] float _twoStarScore;
    [SerializeField] float _threeStarScore;

    [SerializeField] ScoreScript _scoreScript;
    [SerializeField] Image[] _progressBar;

    Transform _currentAnimationObject;
    float[] _barProgressionGoals = new float[] { 0, 0, 0 };
    bool _starAnimationUpscale = false;
    bool _starAnimationDownscale = false;
    float _nextStar = 1;

    //see when to start star animation
    bool _fillChecking = false;
    int _fillCheckerIndex = -1;

    void Update()
    {
        progressAnimation();
        progressBar();
        checkStar();
        float score = _scoreScript.GetScore();

        if(score < _oneStarScore){
            _nextStar = 1;
            if(_nextStar == 2) // went down
            {
                transform.GetChild(0).gameObject.SetActive(false);
            }
        }
        else if(score >= _oneStarScore && score < _twoStarScore){
            if(_nextStar == 1) // went up
            {
                _fillChecking = true;
                _fillCheckerIndex = 0;
            }
            else if (_nextStar == 3) // went down
            {
                transform.GetChild(1).gameObject.SetActive(false);
                _nextStar = 2;
            }
        }
        else if(score >= _twoStarScore && score < _threeStarScore){
            if (_nextStar == 2) // went up
            {
                _fillChecking = true;
                _fillCheckerIndex = 1;
            }
            else if(_nextStar == 4) // went down
            {
                transform.GetChild(2).gameObject.SetActive(false);
                _nextStar = 3;
            }
        }
        else if(score >= _threeStarScore){
            if (_nextStar == 3) // went up
            {
                _fillChecking = true;
                _fillCheckerIndex = 2;
            }
        }

        switch (_nextStar) {
            case 1:
                _barProgressionGoals[0] = (1f / _oneStarScore * score);
                break;
            case 2:
                _barProgressionGoals[1] = (1f / (_twoStarScore - _oneStarScore)) * (score - _oneStarScore);
                break;
            case 3:
                _barProgressionGoals[2] = (1f / (_threeStarScore - _twoStarScore)) * (score - _twoStarScore);
                break;
        }
    }

    void progressBar(){
        switch (_nextStar)
        {
            case 1:
                _progressBar[0].fillAmount = Mathf.Lerp(_progressBar[0].fillAmount,_barProgressionGoals[0], 0.3f);
                break;
            case 2:
                if(Mathf.Approximately(_progressBar[0].fillAmount, 1f))
                    _progressBar[1].fillAmount = Mathf.Lerp(_progressBar[1].fillAmount, _barProgressionGoals[1], 0.1f);
                break;
            case 3:
                if(Mathf.Approximately(_progressBar[1].fillAmount, 1f))
                    _progressBar[2].fillAmount = Mathf.Lerp(_progressBar[2].fillAmount, _barProgressionGoals[2], 0.1f);
                break;
        }
    }

    void checkStar(){
        if(_fillChecking){
            if(Mathf.Approximately(_progressBar[_fillCheckerIndex].fillAmount, 1f)){
                _nextStar = _fillCheckerIndex + 2; // 0 + 2
                _currentAnimationObject = transform.GetChild(_fillCheckerIndex);
                _currentAnimationObject.gameObject.SetActive(true);
                _starAnimationUpscale = true;
                _fillChecking = false;
            }
        }
    }

    void progressAnimation()
    {
        if (_starAnimationUpscale)
        {
            if (_currentAnimationObject.localScale.x < 1.4f)
            {
                _currentAnimationObject.localScale += new Vector3(0.01f, 0.01f, 0.01f);
            }
            else
            {
                _starAnimationDownscale = true;
                _starAnimationUpscale = false;
                _currentAnimationObject.localScale = new Vector3(1.4f, 1.4f, 1.4f);
            }
        }
        if (_starAnimationDownscale)
        {
            if (_currentAnimationObject.localScale.x > 1f)
            {
                _currentAnimationObject.localScale -= new Vector3(0.01f, 0.01f, 0.01f);
            }
            else
            {
                _starAnimationDownscale = false;
                _currentAnimationObject.localScale = new Vector3(1, 1, 1);
            }
        }
    }
}
