using System;
using GWM = GameWorldManager;

public class PS_Life : PlayerStat
{
    #region LikeSingleton
    private PS_Life() : base() { }
    public static Func<bool> Instance(int key, out PS_Life life, bool debug = false)
    {
        if (key == Key.GetKey())
        {
            life = new PS_Life();
            _debug = debug;
            return life.MyAwake;
        }
        life = null;
        return null;
    }
    #endregion



    protected override void OnAwake()
    {
        _playerManager.Endurance.onValueBecomesZero += EnduranceIsZero;
        _playerManager.Endurance.onValueIncreasesFromZero += EnduranceIncreasesFromZero;
        _playerManager.Rest.onValueBecomesZero += RestIsZero;
        _playerManager.Rest.onValueIncreasesFromZero += RestIncreasesFromZero;
        _playerManager.Hunger.onValueBecomesZero += HungerIsZero;
        _playerManager.Hunger.onValueIncreasesFromZero += HungerIncreasesFromZero;
        _playerManager.Thirst.onValueBecomesZero += ThirstIsZero;
        _playerManager.Thirst.onValueIncreasesFromZero += ThirstIncreasesFromZero;
        _playerManager.SunStroke.onValueBecomesZero += SunStrokeIsZero;
        _playerManager.SunStroke.onValueIncreasesFromZero += SunStrokeIncreasesFromZero;
        _playerManager.StomacStroke.onValueBecomesZero += StomacStrokeIsZero;
        _playerManager.StomacStroke.onValueIncreasesFromZero += StomacStrokeIncreasesFromZero;
        _playerManager.HeartStroke.onValueBecomesZero += HeartStrokeIsZero;
        _playerManager.HeartStroke.onValueIncreasesFromZero += HeartStrokeIncreasesFromZero;
    }

    protected override void CheckValue()
    {

    }

    protected override void SetValue(int secondsDelay)
    {

    }

    private void EnduranceIsZero()
    {

    }
    private void EnduranceIncreasesFromZero()
    {

    }

    private void RestIsZero()
    {

    }
    private void RestIncreasesFromZero()
    {

    }

    private void HungerIsZero()
    {

    }
    private void HungerIncreasesFromZero()
    {

    }

    private void ThirstIsZero()
    {

    }
    private void ThirstIncreasesFromZero()
    {

    }

    private void SunStrokeIsZero()
    {

    }
    private void SunStrokeIncreasesFromZero()
    {

    }

    private void StomacStrokeIsZero()
    {

    }
    private void StomacStrokeIncreasesFromZero()
    {

    }

    private void HeartStrokeIsZero()
    {

    }
    private void HeartStrokeIncreasesFromZero()
    {

    }
}