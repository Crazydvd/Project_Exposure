using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class VideoScript : MonoBehaviour
{
    [SerializeField] VideoPlayer _video;

    [SerializeField] VideoClip _dutch = null;
    [SerializeField] VideoClip _english = null;
    [SerializeField] VideoClip _german = null;

    [Space]
    [SerializeField] GameObject _text = null;

    void Start()
    {
        LoadingScreenScript.LevelReady = false;

        if (_video == null)
        {
            throw new System.Exception("Forgot to drag a video to this script");
        }

        setVideo();
        StartCoroutine(loadVideo());
    }

    void OnTriggerEnter(Collider other)
    {
        _video?.Play();
        _video.loopPointReached += setTextActive;
    }

    void setTextActive(VideoPlayer pPlayer)
    {
        _text.SetActive(true);
    }

    void OnDestroy()
    {
        if (gameObject && _video != null)
        {
            _video.Stop();
            _text.SetActive(false);
        }
    }

    IEnumerator loadVideo()
    {
        _video.Prepare();

        while (!_video.isPrepared)
        {
            yield return null;
        }

        LoadingScreenScript.LevelReady = true;
    }

    void setVideo()
    {
        Language language = LanguageSettings.Language;
        VideoClip clip = getClip(language);

        while (language > 0 && clip == null)
        {
            language--;
            clip = getClip(language);
        }

        if (clip == null)
        {
            return;
        }

        _video.clip = clip;
    }

    VideoClip getClip(Language pLanguage)
    {
        switch (pLanguage)
        {
            case Language.NL:
                return _dutch;
            case Language.EN:
                return _english;
            case Language.DE:
                return _german;
            default:
                return _dutch;
        }
    }
}
