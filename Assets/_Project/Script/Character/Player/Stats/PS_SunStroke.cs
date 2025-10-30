using System;

public class PS_SunStroke : PlayerStat
{
    #region LikeSingleton
    private PS_SunStroke() : base() { }
    public static PS_SunStroke Instance(int key, out Func<bool, float, bool> myAwake, out Func<bool> myStart)
    {
        if (key == Key.GetKey())
        {
            PS_SunStroke sunStroke = new PS_SunStroke();
            myAwake = sunStroke.MyAwake;
            myStart = sunStroke.MyStart;
            return sunStroke;
        }
        myAwake = null;
        myStart = null;
        return null;
    }
    #endregion

    private bool _isEnduranceZero;

    protected override void OnAwake()
    {
        Name = "SunStroke";

        _minutesRealTimeToCompleteIncrease = 60;
        _minutesRealTimeToCompleteDecrease = 360;
    }
    protected override void OnStart()
    {
        _playerManager.Endurance.onValueBecomesZero += EnduranceIsZero;
        _playerManager.Endurance.onValueIncreasesFromZero += EnduranceIncreasesFromZero;
    }

    protected override void CheckValue(float timeDelay)
    {
        _isIncrease = _isEnduranceZero ? true : false;
    }

    private void EnduranceIsZero() => _isEnduranceZero = true;
    private void EnduranceIncreasesFromZero() => _isEnduranceZero = false;
}