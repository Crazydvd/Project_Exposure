using UnityEngine;
using UnityEngine.Video;

public class VideoScript : MonoBehaviour
{
    [SerializeField] VideoPlayer _video;

    [SerializeField] VideoClip _dutch = null;
    [SerializeField] VideoClip _english = null;
    [SerializeField] VideoClip _german = null;

    void Start()
    {
        if (_video == null)
        {
            throw new System.Exception("Forgot to drag a video to this script");
        }

        setVideo();
    }

    void OnTriggerEnter(Collider other)
    {
        _video?.Play();
    }

    void OnTriggerExit(Collider other)
    {
        _video?.Stop();
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
