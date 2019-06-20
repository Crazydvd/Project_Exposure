﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShake : MonoBehaviour
{
    Vector3 _oldPosition;
    bool _alreadyShaking;

    public IEnumerator Shake(float pDuration, float pMagnitude)
    {
        if (!_alreadyShaking)
        {
            _oldPosition = transform.localPosition;
            _alreadyShaking = true;
        }

        float _time = 0f;

        while (_time < pDuration)
        {
            float x = Random.Range(-1f, 1f) * pMagnitude;
            float y = Random.Range(-1f, 1f) * pMagnitude;

            transform.localPosition = new Vector3(_oldPosition.x + x, _oldPosition.y + y, _oldPosition.z);

            _time++;

            yield return null;
        }

        transform.localPosition = _oldPosition;
        _alreadyShaking = false;
    }

    public void StartShake(float pDuration, float pMagnitude)
    {
        StartCoroutine(Shake(pDuration, pMagnitude));
    }
}
