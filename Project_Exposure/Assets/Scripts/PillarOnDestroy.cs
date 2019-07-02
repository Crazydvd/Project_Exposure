using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarOnDestroy : MonoBehaviour
{
    void OnDestroy()
    {
        if (LoadingScreenScript.Instance != null && LoadingScreenScript.Instance.IsLoading)
        {
            return;
        }

        GetComponentInParent<PillarBossScript>()?.RemoveObstacle();
    }
}
