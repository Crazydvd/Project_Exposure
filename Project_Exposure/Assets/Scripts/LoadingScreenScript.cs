using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenScript : MonoBehaviour
{
    public static LoadingScreenScript Instance { get; private set; }

    //Ref to the current loading operation in the background
    AsyncOperation _currentLoadingOperation;

    bool _isLoading;

    [Header("The filled part of the loading bar")]
    [SerializeField] RectTransform _barFill;

    Vector3 _barFillLocalScale;

    [Header("The %")]
    [SerializeField] Text _percentLoadedText;

    [Header("Minimum screen time (seconds)")]
    [SerializeField] float _minTime = 0;

    float _timeElapsed = 0;

    [SerializeField] bool _hideProgressBar = false;
    [SerializeField] bool _hidePercentage = false;

    [Header("Loading screen fade in/out")]
    [SerializeField] bool _fade = true;

    [Header("Loading progress only")]
    [SerializeField] bool _loadOnly = false;

    Animator _animator;
    bool _didFadeOut;

    void Awake()
    {
        initialize();
    }

    void Update()
    {
        if (_isLoading)
        {
            setProgress(_currentLoadingOperation.progress);

            if (_currentLoadingOperation.isDone && !_didFadeOut)
            {
                if (_fade)
                {
                    _animator.SetTrigger("Hide");
                    _didFadeOut = true;
                }
                else
                {
                    Hide();
                }
            }
            else
            {
                if (_timeElapsed >= _minTime)
                {
                    _currentLoadingOperation.allowSceneActivation = true;
                }
            }
        }
    }

    void initialize()
    {
        if (Instance == null)
        {
            Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);

            return;
        }

        _barFillLocalScale = _barFill.localScale;
        _animator = GetComponent<Animator>();

        _barFill.parent.gameObject.SetActive(!_hideProgressBar);
        _percentLoadedText.gameObject.SetActive(!_hidePercentage);

        Hide();
    }

    //Updates the UI
    void setProgress(float pProgress)
    {
        _barFillLocalScale.x = pProgress;

        _barFill.localScale = _barFillLocalScale;

        _percentLoadedText.text = _loadOnly ? Mathf.CeilToInt(pProgress * 100) / 0.9 + "%" 
                                            : Mathf.CeilToInt(pProgress * 100) + "%";
    }

    public void Show(int pBuildIndex)
    {
        gameObject.SetActive(true);

        //Start loading
        StartCoroutine(load(pBuildIndex));

        //Settings stuff up
        setProgress(0.0f);

        _timeElapsed = 0;

        _currentLoadingOperation.allowSceneActivation = false;

        if (_fade)
        {
            //Trigger animation
            _animator.SetTrigger("Show");
        }

        _didFadeOut = false;

        _isLoading = true;
    }

    public void Hide()
    {
        gameObject.SetActive(false);

        _currentLoadingOperation = null;

        _isLoading = false;
    }

    public static void Load(int pBuildIndex)
    {
        Instance.Show(pBuildIndex);
    }

    System.Collections.IEnumerator load(int pBuildIndex)
    {
        _currentLoadingOperation = SceneManager.LoadSceneAsync(pBuildIndex);

        while (!_currentLoadingOperation.isDone)
        {
            setProgress(_currentLoadingOperation.progress);
            _timeElapsed += Time.deltaTime;

            yield return null;
        }
    }
}
