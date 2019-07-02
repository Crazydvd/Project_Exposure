using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpdateSliderImagesScript : MonoBehaviour
{
    [SerializeField] GameObject _imagesContainer;
    Slider _slider;

    void Start()
    {
        _slider = GetComponent<Slider>();
    }

    public void UpdateSlider(){
        disableAllImages();
        _imagesContainer.transform.GetChild((int) _slider.value - 1).gameObject.SetActive(true);
    }

    private void disableAllImages(){
        for (int i = 0; i < _imagesContainer.transform.childCount; i++)
        {
            _imagesContainer.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
}
