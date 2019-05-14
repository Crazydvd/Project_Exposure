using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleCountScript : MonoBehaviour
{

    [HideInInspector] public float TotalObstacleCount;

    // Start is called before the first frame update
    private void Start() => TotalObstacleCount = transform.childCount;

    public float GetCurrentObstacleCount() => transform.childCount;
}
