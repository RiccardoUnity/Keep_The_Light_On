using System;
using UnityEngine;
using GWM = GameWorldManager;

public class PS_Hunger : PlayerStat
{
    #region LikeSingleton
    private PS_Hunger() : base() { }
    public static PS_Hunger Instance(int key, out Func<bool, float, bool> myAwake, out Func<bool> myStart)
    {
        if (key == Key.GetKey())
        {
            PS_Hunger hunger = new PS_Hunger();
            myAwake = hunger.MyAwake;
            myStart = hunger.MyStart;
            return hunger;
        }
        myAwake = null;
        myStart = null;
        return null;
    }
    #endregion

    protected override void OnAwake()
    {
        Name = "Hunger";

        _minutesRealTimeToCompleteIncrease = 0;
        _minutesRealTimeToCompleteDecrease = 960;
    }

    protected override void OnStart() { }

    protected override void CheckValue(float timeDelay)
    {
        if (_modifier == 0)
        {
            _isIncrease = false;
        }
        else
        {
            _isIncrease = _modifier > 0 ? true : false;
        }

        _extra = _decrease * timeDelay * _playerController.EnergyToProcess * -1;
    }
}