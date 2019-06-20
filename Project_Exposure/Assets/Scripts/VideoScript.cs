using UnityEngine;
using UnityEngine.Video;

public class VideoScript : MonoBehaviour
{
    [SerializeField] VideoPlayer _video;

    void OnTriggerEnter(Collider other)
    {
        _video?.Play();
    }

    void OnTriggerExit(Collider other)
    {
        _video?.Stop();
    }
}
