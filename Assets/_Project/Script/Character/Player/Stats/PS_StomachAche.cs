using System;
using UnityEngine;
using GWM = GameWorldManager;

public class PS_StomachAche : PlayerStat
{
    #region LikeSingleton
    private PS_StomachAche() : base() { }
    public static PS_StomachAche Instance(int key, out Func<bool, float, bool> myAwake, out Func<bool> myStart)
    {
        if (key == Key.GetKey())
        {
            PS_StomachAche stomacStroke = new PS_StomachAche();
            myAwake = stomacStroke.MyAwake;
            myStart = stomacStroke.MyStart;
            return stomacStroke;
        }
        myAwake = null;
        myStart = null;
        return null;
    }
    #endregion

    private bool _isStomahAche;

    protected override void OnAwake()
    {
        Name = "StomacAche";

        _minutesRealTimeToCompleteIncrease = 60;
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
            if (_isStomahAche)
            {
                _isIncrease = true;
                if (_playerManager.IsWakeUp)
                {
                    _extra = _decrease * timeDelay;
                }
                else
                {
                    _extra = _decrease * timeDelay / 2f;
                }
            }
            else
            {
                _isIncrease = false;
            }
        }
    }

    public void SetStomachAcheTrue() => _isStomahAche = true;
}