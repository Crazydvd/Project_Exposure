using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    private static System.Random _random = new System.Random();

    [Tooltip("How fast the animation should be")]
    public float Speed = 1;

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
        _animator.speed = Speed;

        if (_randomized)
        {
            _time = (float) _random.NextDouble();
        }

        AnimatorClipInfo[] _cInfo = _animator.GetCurrentAnimatorClipInfo(0);
        AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);

        if (_cInfo.Length > 0)
        {
            _animator.Play(state.fullPathHash, -1, _time);
        }
    }
}
