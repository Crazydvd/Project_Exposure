using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StarScript : MonoBehaviour
{
    [SerializeField] Sprite _filledStarSprite;
    [SerializeField] float _scoreForTwoStars = 10000f;
    [SerializeField] float _scoreForThreeStars = 20000f;

    GameObject[] _stars = new GameObject[3];
    Image[] _fillImages = new Image[3];

    int _currentStar = 1; // indexer
    int _fillGoal = 1; // amount of stars to fill
    bool _fillStars = false; // boolean to start

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

    public void CheckStarScore(float pScore){
        _fillStars = true;
        if(pScore > _scoreForThreeStars){
            _fillGoal = 3;
        }else if(pScore > _scoreForTwoStars){
            _fillGoal = 2;
        }
    }

    public int GetStarScore(){
        return _fillGoal;
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
