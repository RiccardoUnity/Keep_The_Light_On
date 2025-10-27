using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AcceleratedTime : MonoBehaviour
{
    private bool _isMyAwake;

    [SerializeField] private Image _fill;
    private float _fillUnit;

    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;
        }
    }

    public void StartAcceleration(float realSecondAwait)
    {
        gameObject.SetActive(true);
        StartCoroutine(Acceleration(realSecondAwait));
    }

    private IEnumerator Acceleration(float realSecondAwait)
    {
        _fill.fillAmount = 0f;
        float time = 0;
        while (time < realSecondAwait)
        {
            yield return null;
            time += Time.deltaTime;
            _fill.fillAmount = time / realSecondAwait;
        }
        gameObject.SetActive(false);
    }
}
