using System;
using UnityEngine;
using GWM = GameWorldManager;

public class PS_Thirst : PlayerStat
{
    #region LikeSingleton
    private PS_Thirst() : base() { }
    public static PS_Thirst Instance(int key, out Func<bool, float, bool> myAwake, out Func<bool> myStart)
    {
        if (key == Key.GetKey())
        {
            PS_Thirst thirst = new PS_Thirst();
            myAwake = thirst.MyAwake;
            myStart = thirst.MyStart;
            return thirst;
        }
        myAwake = null;
        myStart = null;
        return null;
    }
    #endregion

    protected override void OnAwake()
    {
        Name = "Thirst";

        _minutesRealTimeToCompleteIncrease = 5;
        _minutesRealTimeToCompleteDecrease = 480;
    }

    protected override void OnStart() { }

    protected override void CheckValue(float timeDelay)
    {
        if (_moltiplier != 0)
        {
            _isIncrease = _moltiplier > 0 ? true : false;
        }

        _extra = _decrease * timeDelay * _playerController.EnergyToProcess * -1;
    }
}