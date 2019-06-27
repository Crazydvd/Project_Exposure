using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarOnDestroy : MonoBehaviour
{
    void OnDestroy()
    {
        GetComponentInParent<PillarBossScript>().RemoveObstacle();
    }
}
