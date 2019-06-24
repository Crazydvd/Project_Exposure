using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VideoPlayerScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<UnityEngine.Video.VideoPlayer>().Prepare();
    }
}
