using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationScript : MonoBehaviour
{
    static System.Random _random = new System.Random();

    [Tooltip("How fast the animation should be")]
    public float Speed = 1;

    [SerializeField]
    [Tooltip("At what time (percentage) the animation should start")]
    [Range(0, 1)]
    float _time = 0;

    [SerializeField]
    bool _randomized = true;

    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();

        animator.speed = Speed;

        if (_randomized)
        {
            _time = (float) _random.NextDouble();
        }

        if (animator.GetCurrentAnimatorClipInfo(0).Length > 0)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);
            animator.Play(state.fullPathHash, -1, _time);
        }
    }
}
