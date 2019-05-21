using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    [SerializeField] GameObject _lookAt;

    public IEnumerator Shake(float pDuration, float pMagnitude)
    {
        Vector3 _oldPosition = _lookAt.transform.position;

        float _time = 0f;

        while (_time < pDuration)
        {
            float x = Random.Range(-1f, 1f) * pMagnitude;
            float y = Random.Range(-1f, 1f) * pMagnitude;

            _lookAt.transform.position = new Vector3(_oldPosition.x + x, _oldPosition.y + y, _oldPosition.z);

            _time += Time.deltaTime;

            yield return null;
        }

        _lookAt.transform.position = _oldPosition;
    }

    public void StartShake(float pDuration, float pMagnitude)
    {
        StartCoroutine(Shake(pDuration, pMagnitude));
    }
}
