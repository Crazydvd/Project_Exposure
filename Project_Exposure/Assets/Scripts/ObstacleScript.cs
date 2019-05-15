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

    private void Start()
    {
        Renderer renderer = GetComponentInChildren<Renderer>();

        switch (_frequency)
        {
            case Frequency.LOW:
                renderer.material = _lowFreqMaterial;
                break;
            case Frequency.MEDIUM:
                renderer.material = _mediumFreqMaterial;
                break;
            case Frequency.HIGH:
                renderer.material = _highFreqMaterial;
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

        if (other.transform.tag.ToUpper() == _frequency + "FREQ")
        {
            Shatter();
        }
    }

    public void Shatter()
    {
        //if a tutorial zone is linked to this object, resume gameplay on shatter
        if (_tutorialZones.Count > 0)
        {
            _tutorialZones[0].GetComponent<TutorialZoneScript>().ReenablePlayer();
        }

        Transform shardsContainer = transform.GetChild(0).transform;

        //Shatter and destroy. Decimate and obliterate. Annihilate and eradicate. Erase from existence.
        for (int i = 0; i < shardsContainer.childCount; i++)
        {
            Transform child = shardsContainer.GetChild(i);
            Rigidbody childRigid = child.GetComponent<Rigidbody>();

            //NOTE: Earlier you used brackets for single-line if statements, pick one. Consistency is key!
            if (childRigid == null)
                Debug.Log("YOU FORGOT TO ADD KINEMATIC RIGIDBODY TO THE CHILD!!!");

            child.gameObject.SetActive(true);
            childRigid.isKinematic = false;

            Vector3 direction = (child.position - shardsContainer.position).normalized;
            Vector3 randomizedDirection = new Vector3(direction.x * Random.Range(0.5f, 1.5f), direction.y * Random.Range(0.5f, 1.5f), direction.z * Random.Range(0.5f, 1.5f));

            childRigid.AddForce(randomizedDirection * _shatterForce, ForceMode.Impulse);
            childRigid.angularVelocity = new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f)) * 2;
        }

        shardsContainer.DetachChildren();
        Destroy(gameObject);
    }

    public Frequency GetFreq()
    {
        return _frequency;
    } 
}
