using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GTM = GameTimeManager;
using PM = PlayerManager;

[Serializable]
public class PlayerStat_Endurance : PlayerStat_Generic
{
    [SerializeField] private LayerMask _blockLightLayerMask;
    private QueryTriggerInteraction _qti = QueryTriggerInteraction.Ignore;

    private bool _hasOnSun;
    private const int  _minutesGameTimeOnSun = 60;
    private int _secondsGameTimeOnSun;
    private const int _minutesGameTimeOffSun = 180;
    private int _secondsGameTimeOffSun;

    //public PlayerStat_Endurance() : base()
    //{
    //    _secondsGameTimeOnSun = (int)(_minutesGameTimeOnSun * 60 * GTM.Instance.realSecondToGameSecond);
    //}

    protected override void CheckValue()
    {
        if (GTM.Instance.DayTime == DayTime.Day)
        {
            if(Physics.Linecast(PM.Instance.head.position, GTM.Instance.mainLight.position, _blockLightLayerMask, _qti))
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
