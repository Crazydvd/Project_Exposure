using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlConveyorBelt : MonoBehaviour
{
    [Header("The parent(s) of the conveyorBelts that you want to control")]
    [SerializeField] List<GameObject> _conveyorBeltGroup = null;

    public void StopBelt()
    {
        SetSpeed(0);
    }

    public void StartBelt()
    {
        foreach (GameObject belt in _conveyorBeltGroup)
        {
            for (int i = 0; i < belt.transform.childCount; i++)
            {
                ConveyorScript script = belt.transform.GetChild(i).GetComponentInChildren<ConveyorScript>();
                if (script != null)
                {
                    script.Speed = script.GetOldSpeed();
                }
            }
        }
    }

    public void SetSpeed(float pSpeed)
    {
        foreach (GameObject belt in _conveyorBeltGroup)
        {
            for (int i = 0; i < belt.transform.childCount; i++)
            {
                ConveyorScript script = belt.transform.GetChild(i).GetComponentInChildren<ConveyorScript>();
                if (script != null)
                {
                    script.Speed = pSpeed;
                }
            }
        }
    }

    public GameObject AddConveyorBelt(GameObject pBelt)
    {
        if (!_conveyorBeltGroup.Contains(pBelt))
        {
            _conveyorBeltGroup.Add(pBelt);
        }

        return pBelt;
    }
}
