using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWM = GameWorldManager;

public class UI_Bed : MonoBehaviour
{
    private bool _isMyAwake;
    private int _keyForPlayerManager;

    [SerializeField] private UI_OtherButton _chooseHours;
    [SerializeField] private UI_Button _startRest;

    private string[] _hours = new string[12];

    public void MyAwake()
    {
        if (_isMyAwake)
        {
            Debug.Log("MyAwake has already setted", gameObject);
        }
        else
        {
            _isMyAwake = true;

            for (int i = 1; i < 13; ++i)
            {
                _hours[i -1] = i.ToString();
            }
            _chooseHours.SetUp(_hours, 0);

            gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        if (_isMyAwake)
        {
            _chooseHours.SetIndex(0);
        }
    }

    public bool SetKeyForPlayerManager(int key)
    {
        if (_keyForPlayerManager == 0 || _keyForPlayerManager == key)
        {
            _keyForPlayerManager = key;
            return true;
        }
        return false;
    }

    public void StartRest()
    {
        GWM.Instance.TimeManager.onEndAceleration += WakeUpPlayer;
        GWM.Instance.PlayerManager.SetIsWakeUp(_keyForPlayerManager, false);
        int realSecond = (_chooseHours.Index + 1) * 60 * 60;
        GWM.Instance.TimeManager.SetGamePlayAccelerate(realSecond, 5f);
    }

    public void Exit() => GWM.Instance.UIInventory.CloseUIBed();

    private void WakeUpPlayer()
    {
        GWM.Instance.TimeManager.onEndAceleration -= WakeUpPlayer;
        GWM.Instance.PlayerManager.SetIsWakeUp(_keyForPlayerManager, true);
    }
}
