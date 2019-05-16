using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShatterObject : MonoBehaviour
{
    [SerializeField]
    float _speed = 10f;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Shatter();
        }
    }

    public void Shatter()
    {
        //TEMPORARY:
        //for (int i = 0; i < transform.childCount; i++)
        //{
        //    Transform child = transform.GetChild(i);
        //    Rigidbody childRigid = child.GetComponent<Rigidbody>();

        //    if (childRigid == null)
        //    {
        //        Debug.Log("YOU FORGOT TO ADD KINEMATIC RIGIDBODY TO THE CHILD!!!");
        //    }

        //    Transform childTransform = child.GetComponent<Transform>();
        //    child.gameObject.SetActive(true);
        //    childRigid.isKinematic = false;

        //    Vector3 direction = (childTransform.position - transform.position).normalized;
        //    Vector3 randomizedDirection = new Vector3(direction.x * Random.Range(0.5f, 1.5f), direction.y * Random.Range(0.5f, 1.5f), direction.z * Random.Range(0.5f, 1.5f));

        //    childRigid.AddForce(randomizedDirection * _shatterForce, ForceMode.Impulse);
        //    childRigid.angularVelocity = new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f)) * 2;
        //}

        //transform.DetachChildren();
        //Destroy(gameObject);


        Transform shardsContainer = transform.GetChild(0).transform;

        //Shatter and destroy. Decimate and obliterate. Annihilate and eradicate. Erase from existence.
        for (int i = 0; i < shardsContainer.childCount; i++)
        {
            Transform child = shardsContainer.GetChild(i);
            Rigidbody childRigid = child.GetComponent<Rigidbody>();

            if (childRigid == null)
                Debug.Log("YOU FORGOT TO ADD KINEMATIC RIGIDBODY TO THE CHILD!!!");

            child.gameObject.SetActive(true);
            childRigid.isKinematic = false;
            child.GetComponent<Renderer>().material = shardsContainer.GetComponent<Renderer>().material;

            Vector3 direction = (child.position - shardsContainer.position).normalized;
            Vector3 randomizedDirection = new Vector3(direction.x * Random.Range(0.5f, 1.5f), direction.y * Random.Range(0.5f, 1.5f), direction.z * Random.Range(0.5f, 1.5f));

            childRigid.AddForce(randomizedDirection * _speed, ForceMode.Impulse);
            childRigid.angularVelocity = new Vector3(Random.Range(0f, 10f), Random.Range(0f, 10f), Random.Range(0f, 10f)) * 2;
        }

        shardsContainer.DetachChildren();
        Destroy(gameObject);
    }
}
