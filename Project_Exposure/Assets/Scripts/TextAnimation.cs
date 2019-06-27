using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class TextAnimation : MonoBehaviour
{
    /// <summary>
    /// whether or not to play the animation
    /// </summary>
    public bool Play { get; set; } = true;

    [Header("Time (seconds) between letters")]
    [SerializeField] float _time = 1;

    [Header("Which letters to animate (starting at 1)")]
    [SerializeField] int _lettersFromBeginning = 0;
    [SerializeField] int _lettersFromEnd = 0;

    [Header("Should it not be affected by TimeScale")]
    [SerializeField] bool _useUnscaledTime = true;

    Text _text;

    string _begin;
    string _end;
    string _remainder;

    string _currentBegin;
    string _currentEnd;

    int _beginLength = 0;
    int _endLength = 0;

    float _elapsedTime = 0;

    /* Have amount of letters from beginning
     * Turn that into substring
     * 
     * have amount of letters from end
     * turn that into substring
     * 
     * then take the remaining stuff
     * turn that into substring
     * 
     * take a substring (1 for both begin and end)
     * keep adding another letter to it per interval
     * then add begin and end to the remainder substring.
     */

    void Start()
    {
        initialize();

        print($"{_begin} {_remainder} {_end}");
    }

    void initialize()
    {
        _text = GetComponent<Text>();
        string originalText = _text.text;
        _begin = (_lettersFromBeginning > 0) ? originalText.Substring(0, _lettersFromBeginning) : "";
        _end = (_lettersFromEnd > 0) ? originalText.Substring(originalText.Length - _lettersFromEnd) : "";

        int total = _lettersFromBeginning + _lettersFromEnd;

        _remainder = total > originalText.Length ? "" : originalText.Substring(_lettersFromBeginning, originalText.Length - total);
    }

    void Update()
    {
        if (!Play)
        {
            return;
        }

        _elapsedTime += _useUnscaledTime ? Time.unscaledDeltaTime : Time.deltaTime;

        if (_elapsedTime >= _time)
        {
            _elapsedTime = 0;

            if (_begin.Length != 0)
            {
                increaseBegin();
            }

            if (_end.Length != 0)
            {
                increaseEnd();
            }

            _text.text = _currentBegin + _remainder + _currentEnd;
        }
    }

    void increaseBegin()
    {
        _beginLength++;
        _beginLength %= _begin.Length + 1;
        _currentBegin = _begin.Substring(0, _beginLength);
    }

    void increaseEnd()
    {
        _endLength++;
        _endLength %= _end.Length + 1;
        _currentEnd = _end.Substring(0, _endLength);
    }
}
