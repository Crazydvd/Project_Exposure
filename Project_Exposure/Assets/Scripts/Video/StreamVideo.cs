using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using System.Collections;

[RequireComponent(typeof(RawImage))]
public class StreamVideo : MonoBehaviour
{
    //The videoplayers that will stream to the image
    [SerializeField] VideoPlayer _blueWavePlayer;
    [SerializeField] VideoPlayer _greenWavePlayer;
    [SerializeField] VideoPlayer _redWavePlayer;
    
    //The image to stream onto
    RawImage _image;

    [Header("The video clips to use")]
    [SerializeField] VideoClip _blueWave;
    [SerializeField] VideoClip _greenWave;
    [SerializeField] VideoClip _redWave;

    void Start()
    {
        _image = GetComponent<RawImage>();

        _blueWavePlayer.clip = _blueWave;
        _greenWavePlayer.clip = _greenWave;
        _redWavePlayer.clip = _redWave;

        StartCoroutine(playVideo());
    }

    IEnumerator playVideo()
    {
        _redWavePlayer.Prepare();
        _greenWavePlayer.Prepare();
        _blueWavePlayer.Prepare();

        _image.color = new Color(1, 1, 1, 0);

        while (!_redWavePlayer.isPrepared
               && !_greenWavePlayer.isPrepared
               && !_blueWavePlayer.isPrepared)
        {
            yield return null;
        }

        _image.color = Color.white;
        _image.texture = _greenWavePlayer.texture;
        _redWavePlayer.Play();
        _greenWavePlayer.Play();
        _blueWavePlayer.Play();

        Play(Frequency.MEDIUM);
    }

    public void Play(Frequency pFrequency)
    {
        switch (pFrequency)
        {
            case Frequency.LOW:
                //_redWavePlayer.enabled = true;
                //_greenWavePlayer.enabled = false;
                //_blueWavePlayer.enabled = false;

                _image.texture = _redWavePlayer.texture;
                break;
            case Frequency.MEDIUM:
                //_redWavePlayer.enabled = false;
                //_greenWavePlayer.enabled = true;
                //_blueWavePlayer.enabled = false;

                _image.texture = _greenWavePlayer.texture;
                break;
            case Frequency.HIGH:
                //_redWavePlayer.enabled = false;
                //_greenWavePlayer.enabled = false;
                //_blueWavePlayer.enabled = true;

                _image.texture = _blueWavePlayer.texture;
                break;
        }
    }
}
