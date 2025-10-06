using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PM = PlayerManager;
using GWM = GameWorldManager;

[Serializable]
public class PlayerStat_Endurance : PlayerStat
{
    #region LikeSingleton
    private PlayerStat_Endurance() : base() { }
    public static PlayerStat_Endurance Instance(int key)
    {
        if (key == Key.GetKey())
        {
            return new PlayerStat_Endurance();
        }
        return null;
    }
    #endregion

    private bool _hasOnSun;
    private int  _minutesGameTimeOnSun = 60;
    private int _secondsGameTimeOnSun;
    private int _minutesGameTimeOffSun = 180;
    private int _secondsGameTimeOffSun;
    private float _distanceRay = 15f;

    protected override void OnStart()
    {
        _secondsGameTimeOnSun = (int)(_minutesGameTimeOnSun * 60 * GWM.Instance.timeGame.realSecondToGameSecond);
        _secondsGameTimeOffSun = (int)(_minutesGameTimeOffSun * 60 * GWM.Instance.timeGame.realSecondToGameSecond);
    }

    protected override void CheckValue()
    {
        if (GWM.Instance.timeGame.DayTime == DayTime.Day)
        {
            Ray ray = new Ray(PM.Instance.head.position, GWM.Instance.mainLight.rotation * Vector3.forward);
            if(Physics.Raycast(ray, _distanceRay, GWM.Instance.blockMainLight, GWM.Instance.qti))
            {
                _hasOnSun = true;
            }
            else
            {
                _hasOnSun = false;
            }
        }
        else
        {
            _hasOnSun = false;
        }
    }

    protected override void SetValue()
    {
        
    }
}
