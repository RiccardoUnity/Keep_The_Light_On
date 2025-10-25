using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_AcceleratedTime : MonoBehaviour
{
    private bool _isMyAwake;

    [SerializeField] private Image _fill;
    private int _frameThatIHave = 300;
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

            _fillUnit = 1f / _frameThatIHave;
        }
    }

    public void StartAcceleration()
    {

    }

    private IEnumerator Acceleration()
    {
        _fill.fillAmount = 0f;
        int count = 0;
        while (_fill.fillAmount < 1f)
        {
            yield return null;
            ++count;
            _fill.fillAmount += _fillUnit;
        }
    }
}
