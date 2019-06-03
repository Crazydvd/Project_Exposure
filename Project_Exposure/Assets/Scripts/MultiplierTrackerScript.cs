using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MultiplierTrackerScript : MonoBehaviour
{
    Text _text;
    // Start is called before the first frame update
    void Start()
    {
        _text = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ShootScript.Multiplier < 2)
        {
            _text.text = "";
        }else{
            _text.text = "X " + ShootScript.Multiplier;
        }
    }
}
