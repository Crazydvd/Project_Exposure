using UnityEngine;

public class OnObjectDestroy : MonoBehaviour
{
    [Header("Objects that are activated")]
    [SerializeField] GameObject[] _objects = null;

    [Header("What to do with the objects")]
    [SerializeField] Mode _mode = Mode.ENABLE;

    [Header("The Animator(s) which will play the animation")]
    [SerializeField] Animator[] _animators = null;

    [Header("The name of the animationState to play")]
    [SerializeField] string[] _animationStates = null;

    enum Mode
    {
        ENABLE,
        DISABLE,
        TOGGLE,
    }

    void enableObjects()
    {
        foreach (GameObject @object in _objects)
        {
            @object.SetActive(true);
        }
    }

    void disableObjects()
    {
        foreach (GameObject @object in _objects)
        {
            @object.SetActive(false);
        }
    }

    void toggleObjects()
    {
        foreach (GameObject @object in _objects)
        {
            @object.SetActive(!@object.activeSelf);
        }
    }

    void OnDestroy()
    {
        if (LoadingScreenScript.Instance.IsLoading)
        {
            return;
        }

        switch (_mode)
        {
            case Mode.ENABLE:
                enableObjects();
                break;
            case Mode.DISABLE:
                disableObjects();
                break;
            case Mode.TOGGLE:
                toggleObjects();
                break;
        }

        for (int i = 0; i < _animators.Length; i++)
        {
            if (!_animators[i])
            {
                return;
            }

            _animators[i].Play(_animationStates[i], 0);
        }
    }
}