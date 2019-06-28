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
    [SerializeField] Image _barFill;

    [Header("The %")]
    [SerializeField] Text _percentLoadedText;

    [Header("Minimum screen time (seconds)")]
    [SerializeField] float _minTime = 0;

    float _timeElapsed = 0;

    [Header("Don't show")]
    [SerializeField] bool _hideProgressBar = false;
    [SerializeField] bool _hidePercentage = false;

    [Header("Loading screen fade in/out")]
    [SerializeField] bool _fade = true;

    [Header("Show loading progress only")]
    [SerializeField] bool _loadOnly = false;

    [Header("Require input to continue")]
    [SerializeField] bool _requireInput = false;

    [Header("Optionally: Show when input needed")]
    [SerializeField] GameObject _whenInput = null;

    [Header("Size 0 == any button")]
    [SerializeField] string[] _inputButtons;

    /// <summary>
    /// Can be used by external scripts to prevent level from starting (e.g have some time to preload videos)
    /// </summary>
    public static bool LevelReady { get; set; } = true;


    Animator _animator;
    bool _didFadeOut;

    Image _mainImage = null;

    Transform _extra = null;
    Text[] _extraTexts;
    Image[] _extraImages;


    void Awake()
    {
        initialize();
    }

    void Update()
    {
        if (_isLoading)
        {
            if (_fade)
            {
                setExtraAlpha(_mainImage.color.a);
            }

            setProgress(_currentLoadingOperation.progress);

            if (_didFadeOut)
            {
                return;
            }

            if (_currentLoadingOperation.isDone)
            {
                Time.timeScale = 0;

                if (LevelReady)
                {
                    if (!_requireInput || (_requireInput && checkInput()))
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
        //Only one loading screen can exist
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

        _mainImage = GetComponent<Image>();

        _extra = transform.Find("Extras");
        _extraTexts = _extra.GetComponentsInChildren<Text>();
        _extraImages = _extra.GetComponentsInChildren<Image>();

        _animator = GetComponent<Animator>();

        _barFill.transform.parent.gameObject.SetActive(!_hideProgressBar);
        _percentLoadedText.gameObject.SetActive(!_hidePercentage);

        Hide();
    }

    //Updates the UI
    void setProgress(float pProgress)
    {
        //_barFillLocalScale.x = _loadOnly ? Mathf.Clamp01(pProgress / 0.9f) : pProgress;

        _barFill.fillAmount = pProgress;//_barFillLocalScale;

        _percentLoadedText.text = _loadOnly ? Mathf.Clamp(Mathf.CeilToInt(pProgress * 100 / 0.9f), 0, 100) + "%"
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

        _whenInput?.gameObject.SetActive(false);

        setExtraAlpha(0);

        _didFadeOut = false;

        _isLoading = true;
    }

    public void Hide()
    {
        Time.timeScale = 1;

        gameObject.SetActive(false);

        _currentLoadingOperation = null;

        _isLoading = false;
    }

    public static void Load(int pBuildIndex)
    {
        Instance?.Show(pBuildIndex);
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

    bool checkInput()
    {
        if (_whenInput && !_whenInput.gameObject.activeInHierarchy)
        {
            _whenInput.SetActive(true);
        }

        if (_inputButtons.Length == 0 && Input.anyKey)
        {
            return true;
        }

        foreach (string button in _inputButtons)
        {
            if (Input.GetButton(button))
            {
                return true;
            }
        }

        return false;
    }

    void setExtraAlpha(float pAlpha)
    {
        Color colour;
        float alpha = pAlpha;

        foreach (Text text in _extraTexts)
        {
            colour = text.color;
            colour.a = alpha;
            text.color = colour;
        }

        foreach (Image image in _extraImages)
        {
            colour = image.color;
            colour.a = alpha;
            image.color = colour;
        }
    }
}
