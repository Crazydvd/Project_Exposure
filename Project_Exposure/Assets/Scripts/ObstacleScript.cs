using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleScript : MonoBehaviour
{
    [SerializeField] private float _speed = 1f;
    [SerializeField] private float _shatterForce = 10f;

    [SerializeField] private Frequency _frequency = Frequency.MEDIUM;

    [SerializeField] private Material _lowFreqMaterial;
    [SerializeField] private Material _mediumFreqMaterial;
    [SerializeField] private Material _highFreqMaterial;
    [SerializeField] private List<GameObject> _tutorialZones;
    private Renderer _renderer;

    private void Start()
    {
        _renderer = GetComponent<Renderer>();

        switch (_frequency)
        {
            case Frequency.LOW:
                _renderer.material = _lowFreqMaterial;
                break;
            case Frequency.MEDIUM:
                _renderer.material = _mediumFreqMaterial;
                break;
            case Frequency.HIGH:
                _renderer.material = _highFreqMaterial;
                break;
            default:
                break;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.tag == "MainCamera")
        {
            Debug.Log("u dede");
            Shatter();
        }

        switch (_frequency)
        {
            case Frequency.LOW:
                if (other.transform.tag == "LowFreq")
                {
                    Shatter();
                }

                break;
            case Frequency.MEDIUM:
                if (other.transform.tag == "MediumFreq")
                {
                    Shatter();
                }

                break;
            case Frequency.HIGH:
                if (other.transform.tag == "HighFreq")
                {
                    Shatter();
                }

                break;
            default:
                break;
        }
    }

    public void Shatter()
    {
        //if a tutorial zone is linked to this object, resume gameplay on shatter
        if (_tutorialZones.Count > 0)
        {
            _tutorialZones[0].GetComponent<TutorialZoneScript>().ReenablePlayer();
        }

        //Shatter and destroy. Decimate and obliterate. Annihilate and eradicate. Erase from existence.
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).GetComponent<Rigidbody>() == null)
                Debug.Log("YOU FORGOT TO ADD KINEMATIC RIGIDBODY TO THE CHILD!!!");

            Rigidbody childRigid = transform.GetChild(i).GetComponent<Rigidbody>();
            Transform childTransform = transform.GetChild(i).GetComponent<Transform>();

            transform.GetChild(i).gameObject.SetActive(true);
            childRigid.isKinematic = false;

            Vector3 direction = (childTransform.position - transform.position).normalized;
            Vector3 randomizedDirection = new Vector3(direction.x * Random.Range(0.5f, 1.5f), direction.y * Random.Range(0.5f, 1.5f), direction.z * Random.Range(0.5f, 1.5f));

            childRigid.AddForce(randomizedDirection * _shatterForce, ForceMode.Impulse);
            childRigid.angularVelocity = new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f)) * 2;
        }

        transform.DetachChildren();
        Destroy(transform.parent.gameObject);
    }
}
