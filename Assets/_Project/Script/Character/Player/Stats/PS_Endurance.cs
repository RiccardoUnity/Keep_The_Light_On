using System;

public class PS_Endurance : PlayerStat
{
    #region LikeSingleton
    private PS_Endurance() : base() { }
    public static PS_Endurance Instance(int key, out Func<bool, float, bool> myAwake, out Func<bool> myStart)
    {
        if (key == Key.GetKey())
        {
            PS_Endurance endurance = new PS_Endurance();
            myAwake = endurance.MyAwake;
            myStart = endurance.MyStart;
            return endurance;
        }
        myAwake = null;
        myStart = null;
        return null;
    }
    #endregion

    protected override void OnAwake()
    {
        Name = "Endurance";

        _minutesRealTimeToCompleteIncrease = 360;
        _minutesRealTimeToCompleteDecrease = 120;
    }

    protected override void OnStart() { }

    protected override void CheckValue(float timeDelay)
    {
        _isIncrease = _playerManager.IsUnderTheSun ? false : true;
    }
}
