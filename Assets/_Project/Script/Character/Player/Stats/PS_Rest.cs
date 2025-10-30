using System;

public class PS_Rest : PlayerStat
{
    #region LikeSingleton
    private PS_Rest() : base() { }
    public static PS_Rest Instance(int key, out Func<bool, float, bool> myAwake, out Func<bool> myStart)
    {
        if (key == Key.GetKey())
        {
            PS_Rest rest = new PS_Rest();
            myAwake = rest.MyAwake;
            myStart = rest.MyStart;
            return rest;
        }
        myAwake = null;
        myStart = null;
        return null;
    }
    #endregion

    protected override void OnAwake()
    {
        Name = "Rest";

        _minutesRealTimeToCompleteIncrease = 600;
        _minutesRealTimeToCompleteDecrease = 840;
    }

    protected override void OnStart() { }

    protected override void CheckValue(float timeDelay)
    {
        _isIncrease = _playerManager.IsWakeUp ? false : true;

        if (_isIncrease)
        {
            _extra = 0f;
        }
        else
        {
            _extra = _decrease * timeDelay * _playerController.EnergyToProcess * -1;
        }
    }
}