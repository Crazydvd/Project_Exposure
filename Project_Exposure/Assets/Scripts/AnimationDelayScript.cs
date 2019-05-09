using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationDelayScript : MonoBehaviour
{
    private static System.Random _random = new System.Random();


    [SerializeField]
    [Tooltip("At what time (percentage) the animation should start")]
    [Range(0, 1)]
    private float _time = 0;

    [SerializeField]
    private bool _randomized = false;

    private Animator _animator;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        if (_randomized)
        {
            _time = (float) _random.NextDouble();
        }

        AnimatorClipInfo[] _cInfo = _animator.GetCurrentAnimatorClipInfo(0);

        if (_cInfo.Length > 0)
        {
            AnimationClip clip = _cInfo[0].clip;
            _animator.Play(clip.name, 0, clip.length * _time);
        }
    }
}
