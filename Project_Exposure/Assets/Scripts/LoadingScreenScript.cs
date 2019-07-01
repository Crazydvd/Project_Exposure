using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreenScript : MonoBehaviour
{
    public static LoadingScreenScript Instance { get; private set; }

    //Ref to the current loading operation in the background
    AsyncOperation _currentLoadingOperation;

    public bool IsLoading { get; private set; }

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

    [Header("Hide when done loading")]
    [SerializeField] bool _hideLoadingText = false;
    [SerializeField] bool _hidePercentageWhenDone = false;

    [Header("Loading screen fade in/out")]
    [SerializeField] bool _fadeIn = false;
    [SerializeField] bool _fadeOut = true;

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

    GameObject _loadingText = null;

    Transform _extra = null;
    Text[] _extraTexts;
    Image[] _extraImages;
    RawImage[] _extraRawImages;

    void Awake()
    {
        initialize();
    }

    void Update()
    {
        if (IsLoading)
        {
            if (_fadeIn || _fadeOut)
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
                if (LevelReady)
                {
                    if (_hideLoadingText)
                    {
                        _loadingText?.SetActive(false);
                    }

                    if (_hidePercentageWhenDone)
                    {
                        _percentLoadedText.gameObject.SetActive(false);
                    }

                    //TO DO:
                    /*
                     * Loop through a list of GameObjects here and set them active
                     * so that people can set a list of Objects that have to be shown when done loading
                     */

                    if (!_requireInput || (_requireInput && checkInput()))
                    {
                        if (_fadeOut)
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

        _loadingText = transform.Find("LoadingText").gameObject;

        _extra = transform.Find("Extras");
        _extraTexts = _extra.GetComponentsInChildren<Text>();
        _extraImages = _extra.GetComponentsInChildren<Image>();
        _extraRawImages = _extra.GetComponentsInChildren<RawImage>();

        _animator = GetComponent<Animator>();

        _barFill.gameObject.SetActive(!_hideProgressBar);
        _percentLoadedText.gameObject.SetActive(!_hidePercentage);

        Hide();
    }

    //Updates the UI
    void setProgress(float pProgress)
    {
        _barFill.fillAmount = _loadOnly ? Mathf.Clamp01(pProgress / 0.9f) : pProgress;

        _percentLoadedText.text = _loadOnly ? Mathf.Clamp(Mathf.CeilToInt(pProgress * 100 / 0.9f), 0, 100) + "%"
                                            : Mathf.CeilToInt(pProgress * 100) + "%";
    }

    public void Show(int pBuildIndex)
    {
        gameObject.SetActive(true);

        Time.timeScale = 0;

        //Start loading
        StartCoroutine(load(pBuildIndex));

        //Settings stuff up
        setProgress(0.0f);

        _timeElapsed = 0;

        if (_fadeIn)
        {
            setExtraAlpha(0);

            //Trigger animation
            _animator.SetTrigger("Show");
        }
        else
        {
            _animator.Play("Show", 0, 1);
        }

        _whenInput?.gameObject.SetActive(false);

        _didFadeOut = false;

        IsLoading = true;
    }

    public void Hide()
    {
        Time.timeScale = 1;

        gameObject.SetActive(false);

        _currentLoadingOperation = null;

        IsLoading = false;
    }

    public static void Load(int pBuildIndex)
    {
        Instance?.Show(pBuildIndex);
    }

    System.Collections.IEnumerator load(int pBuildIndex)
    {
        _currentLoadingOperation = SceneManager.LoadSceneAsync(pBuildIndex);

        _currentLoadingOperation.allowSceneActivation = false;

        while (!_currentLoadingOperation.isDone)
        {
            setProgress(_currentLoadingOperation.progress);
            _timeElapsed += Time.unscaledDeltaTime;

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

        foreach (RawImage image in _extraRawImages)
        {
            colour = image.color;
            colour.a = alpha;
            image.color = colour;
        }
    }
}
