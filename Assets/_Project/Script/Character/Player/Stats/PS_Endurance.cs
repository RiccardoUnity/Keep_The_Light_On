using UnityEngine;
using GWM = GameWorldManager;

public class PS_Endurance : PlayerStat
{
    #region LikeSingleton
    private PS_Endurance() : base() { }
    public static PS_Endurance Instance(int key)
    {
        if (key == Key.GetKey())
        {
            return new PS_Endurance();
        }
        return null;
    }
    #endregion

    private float _distanceRay = 15f;
    public bool HasOnSun { get => _hasOnSun; }
    private bool _hasOnSun;

    private int  _minutesGameTimeOnSun = 120;
    private int _secondsRealTimeOnSun;
    private int _minutesGameTimeOffSun = 360;
    private int _secondsRealTimeOffSun;

    protected override void OnStart()
    {
        _secondsRealTimeOnSun = (int)((_minutesGameTimeOnSun * 60) / _timeManager.RealSecondToGameSecond);
        _decrease = (float)_timeManager.SecondDelay / _secondsRealTimeOnSun;
        _secondsRealTimeOffSun = (int)((_minutesGameTimeOffSun * 60) / _timeManager.RealSecondToGameSecond);
        _increase = (float)_timeManager.SecondDelay / _secondsRealTimeOffSun;
    }

    protected override void CheckValue()
    {
        if (_timeManager.DayTime == DayTime.Day)
        {
            Ray ray = new Ray(_playerManager.Head.position, GWM.Instance.mainLight.rotation * Vector3.forward);
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
        if (_hasOnSun)
        {
            Value -= _decrease * _moltiplierDecrease;
        }
        else
        {
            Value += _increase * _moltiplierIncrease;
        }
    }
}
