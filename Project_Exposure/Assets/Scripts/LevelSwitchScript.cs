using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelSwitchScript : MonoBehaviour
{
    [SerializeField] Sprite[] _unselectedSprites;
    [SerializeField] Sprite[] _selectedSprites;
    List<Image> _images = new List<Image>();

    void Start()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            _images.Add(transform.GetChild(i).GetComponent<Image>());
        }

        if(transform.childCount == 2 && LanguageSettings.Language == Language.EN){
            SetSelected(1);
        }
    }

    void deselectEverything(){
        for(int i = 0; i < _images.Count; i++){
            _images[i].sprite = _unselectedSprites[i];
        }
    }

    public void SetSelected(int pIndex){
        deselectEverything();
        _images[pIndex].sprite = _selectedSprites[pIndex];
    }
}
