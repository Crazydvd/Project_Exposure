using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectObstacleCol : MonoBehaviour
{
    ObstacleScript _obstacle = null;
    ScreenShake _screenShake = null;

    Rigidbody _rigidbody = null;

    bool _shatterOnFall = true;

    ScoreScript _scoreScript = null;

    public float ScoreLoss { get; set; }

    void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _obstacle = GetComponentInParent<ObstacleScript>();
        _screenShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScreenShake>();
        _scoreScript = GameObject.Find("Score").GetComponent<ScoreScript>(); //Shite, but it works.
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.transform.tag == "MainCamera")
        {
            _obstacle.Shatter(true);
            _screenShake.StartShake(24f, 0.2f);
            _scoreScript.GetComponent<UnityEngine.UI.Text>().text = _scoreScript.GetScore() + "";
            _scoreScript._negativeScoreScript.EnableText();
        }
        else if (other.transform.tag.ToUpper() == _obstacle.GetFreq() + "FREQ")
        {
            _obstacle.EnableShake(true);
        }
        else if (other.gameObject.layer == 13) //layer 13 == Projectiles (all bullets are on this layer)
        {
            _obstacle.EnableShake(false);
            ShootScript.Multiplier = 0;
            _scoreScript.DecreaseScore(ScoreLoss);
            _scoreScript._negativeScoreScript.EnableText();
            _scoreScript._negativeScoreScript.SetFollowObject(transform);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (GetComponent<Rigidbody>() == null)
        {
            return;
        }

        if (_shatterOnFall && GetComponent<Rigidbody>()?.velocity.magnitude > 2)
        {
            fallOnFloor();
        }
    }

    public void FallImmune()
    {
        _shatterOnFall = false;
        Invoke("resetFallImmunity", 1.0f);
    }

    void resetFallImmunity()
    {
        _shatterOnFall = true;
    }

    void fallOnFloor()
    {
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/bounceglass", gameObject);
        GetComponentInParent<ObstacleScript>().Shatter();
    }
}
