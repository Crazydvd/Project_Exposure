using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleShatterScript : MonoBehaviour
{
    [Header("Shatter Options")]
    [SerializeField] float _shatterForce = 10f;
    [SerializeField] float _shatterDelay = 0.5f;
    [SerializeField] float _shakeForce = 0.05f;

    [SerializeField] float _delayBeforeMenu = 2f;
    [SerializeField] GameObject _menu;
    [SerializeField] GameObject _keyBoard;
    [SerializeField] GameObject _starGameButton;
    [SerializeField] GameObject _highScore;

    float _timeBeforeShatter = 0.0f;
    bool _shaking;
    ScreenShake _screenShake;

    void Start()
    {
        _screenShake = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<ScreenShake>();
    }

    public void Shatter()
    {
        Transform shardsContainer = transform.GetChild(0).transform;

        //Shatter and destroy. Decimate and obliterate. Annihilate and eradicate. Erase from existence.
        for (int i = 0; i < shardsContainer.childCount; i++)
        {
            Transform child = shardsContainer.GetChild(i);
            Rigidbody childRigid = child.GetComponent<Rigidbody>();

            child.gameObject.SetActive(true);
            child.gameObject.layer = 11; //layer 11 == shards

            childRigid.isKinematic = false;
            //Set the material for the shards
            child.GetComponent<Renderer>().material = GetComponentInChildren<Renderer>().material;

            Vector3 direction = (child.position - transform.position).normalized;
            Vector3 randomizedDirection = new Vector3(direction.x * Random.Range(0.5f, 1.5f), direction.y * Random.Range(0.5f, 1.5f), direction.z * Random.Range(0.5f, 1.5f));

            childRigid.AddForce(randomizedDirection * _shatterForce, ForceMode.Impulse);
            childRigid.angularVelocity = new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f)) * 2;
        }

        _screenShake.StartShake(12f, 0.1f);
        shardsContainer.DetachChildren();
        //Play sounds
        FMODUnity.RuntimeManager.PlayOneShotAttached("event:/high_shattering", gameObject);

        Invoke("ContinueGame", _delayBeforeMenu);
        GetComponent<Renderer>().enabled = false;
        _starGameButton.SetActive(false);
        _highScore.SetActive(false);
    }

    void ContinueGame()
    {
        _menu.SetActive(false);
        _keyBoard.SetActive(true);
    }
}
