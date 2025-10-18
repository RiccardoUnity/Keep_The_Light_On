using System;
using UnityEngine;
using GWM = GameWorldManager;

public class PS_Endurance : PlayerStat
{
    #region LikeSingleton
    private PS_Endurance() : base() { }
    public static Func<bool> Instance(int key, out PS_Endurance endurance, bool debug = false)
    {
        if (key == Key.GetKey())
        {
            endurance = new PS_Endurance();
            _debug = debug;
            return endurance.MyAwake;
        }
        endurance = null;
        return null;
    }
    #endregion

    private int  _minutesGameTimeOnSun = 120;
    private int _secondsRealTimeOnSun;
    private int _minutesGameTimeOffSun = 360;
    private int _secondsRealTimeOffSun;

    protected override void OnAwake()
    {
        _secondsRealTimeOnSun = (int)((_minutesGameTimeOnSun * 60) / _timeManager.RealSecondToGameSecond);
        _decrease = 1f / _secondsRealTimeOnSun;
        _secondsRealTimeOffSun = (int)((_minutesGameTimeOffSun * 60) / _timeManager.RealSecondToGameSecond);
        _increase = 1f / _secondsRealTimeOffSun;
    }

    protected override void CheckValue()
    {
        
    }

    protected override void SetValue(int secondsDelay)
    {
        if (_playerManager.IsUnderTheSun)
        {
            Value -= _decrease * _moltiplierDecrease * secondsDelay;
        }
        else
        {
            Value += _increase * _moltiplierIncrease * secondsDelay;
        }
    }
}
