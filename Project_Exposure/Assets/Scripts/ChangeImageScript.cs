using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ChangeImageScript : MonoBehaviour
{
    [SerializeField] Sprite[] _dutch;
    [SerializeField] Sprite[] _english;
    [SerializeField] Sprite[] _german;

    Sprite[] _images = null;

    Image _image;

    int _index = 0;

    // Start is called before the first frame update
    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        _image = GetComponent<Image>();
        _index = 0;

        switch (LanguageSettings.Language)
        {
            case Language.NL:
                _images = _dutch;
                break;
            case Language.EN:
                _images = _english;
                break;
            case Language.DE:
                _images = _german;
                break;
        }

        _image.sprite = getImage();
    }

    public void NextSprite()
    {
        _index++;

        _image.sprite = getImage();
    }

    Sprite getImage()
    {
        if (_images.Length == 0)
        {
            return null;
        }

        _index %= _images.Length;

        return _images[_index];
    }
}
