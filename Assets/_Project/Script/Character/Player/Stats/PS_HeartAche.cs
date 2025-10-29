using System;
using UnityEngine;
using GWM = GameWorldManager;

public class PS_HeartAche : PlayerStat
{
    #region LikeSingleton
    private PS_HeartAche() : base() { }
    public static PS_HeartAche Instance(int key, out Func<bool, float, bool> myAwake, out Func<bool> myStart)
    {
        if (key == Key.GetKey())
        {
            PS_HeartAche heartStroke = new PS_HeartAche();
            myAwake = heartStroke.MyAwake;
            myStart = heartStroke.MyStart;
            return heartStroke;
        }
        myAwake = null;
        myStart = null;
        return null;
    }
    #endregion



    protected override void OnAwake()
    {
        Name = "HeartAche";

        _minutesRealTimeToCompleteIncrease = 720;
        _minutesRealTimeToCompleteDecrease = 720;
    }

    protected override void OnStart() { }

    protected override void CheckValue(float timeDelay)
    {
        if (_modifier != 0)
        {
            _isIncrease = _modifier > 0 ? true : false;
        }
        else
        {
            _isIncrease = true;
            if (_playerManager.IsWakeUp)
            {
                _extra = 0;
            }
            else
            {
                _extra = -_decrease * timeDelay / 2f;
            }
        }
    }
}