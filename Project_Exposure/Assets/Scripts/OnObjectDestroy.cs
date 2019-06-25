using UnityEngine;

public class OnObjectDestroy : MonoBehaviour
{
    enum Mode
    {
        ENABLE,
        DISABLE,
        TOGGLE,
    }

    [Header("Objects that are activated")]
    [SerializeField] GameObject[] _objects;

    [Header("What to do with the objects")]
    [SerializeField] Mode _mode = Mode.ENABLE;

    void OnDestroy()
    {
        foreach (GameObject @object in _objects)
        {
            @object.SetActive(true); 
        }
    }
}