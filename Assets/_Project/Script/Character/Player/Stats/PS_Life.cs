using System;
using UnityEngine;
using GWM = GameWorldManager;

public class PS_Life : PlayerStat
{
    #region LikeSingleton
    private PS_Life() : base() { }
    public static PS_Life Instance(int key, out Func<bool, float, bool> myAwake, out Func<bool> myStart)
    {
        if (key == Key.GetKey())
        {
            PS_Life life = new PS_Life();
            myAwake = life.MyAwake;
            myStart = life.MyStart;
            return life;
        }
        myAwake = null;
        myStart = null;
        return null;
    }
    #endregion

    private StatsRange[] _stats = new StatsRange[4];

    private StatsRange[] _illnesses = new StatsRange[3];

    protected override void OnAwake()
    {
        Name = "Life";

        _minutesRealTimeToCompleteIncrease = 720;
        _minutesRealTimeToCompleteDecrease = 720;
    }

    protected override void OnStart()
    {
        _playerManager.Endurance.onValueBecomesZero += EnduranceIsZero;
        _playerManager.Endurance.onValueIncreasesFromZero += EnduranceIncreasesFromZero;
        _playerManager.Endurance.onValueBecomesOne += EnduranceIncreasesFromZero;
        _playerManager.Rest.onValueBecomesZero += RestIsZero;
        _playerManager.Rest.onValueIncreasesFromZero += RestIncreasesFromZero;
        _playerManager.Rest.onValueBecomesOne += RestIncreasesFromZero;
        _playerManager.Hunger.onValueBecomesZero += HungerIsZero;
        _playerManager.Hunger.onValueIncreasesFromZero += HungerIncreasesFromZero;
        _playerManager.Hunger.onValueBecomesOne += HungerIncreasesFromZero;
        _playerManager.Thirst.onValueBecomesZero += ThirstIsZero;
        _playerManager.Thirst.onValueIncreasesFromZero += ThirstIncreasesFromZero;
        _playerManager.Thirst.onValueBecomesOne += ThirstIncreasesFromZero;

        _playerManager.SunStroke.onValueBecomesZero += SunStrokeIsZero;
        _playerManager.SunStroke.onValueIncreasesFromZero += SunStrokeDifferentFromZero;
        _playerManager.SunStroke.onValueDecreaseFromOne += SunStrokeDifferentFromZero;
        _playerManager.SunStroke.onValueBecomesOne += SunStrokeIsOne;
        _playerManager.StomachAche.onValueBecomesZero += StomacAcheIsZero;
        _playerManager.StomachAche.onValueIncreasesFromZero += StomacAcheDifferentFromZero;
        _playerManager.StomachAche.onValueDecreaseFromOne += StomacAcheDifferentFromZero;
        _playerManager.StomachAche.onValueBecomesOne += StomacAcheIsOne;
        _playerManager.HeartAche.onValueBecomesZero += HeartAcheIsZero;
        _playerManager.HeartAche.onValueIncreasesFromZero += HeartAcheDifferentFromZero;
        _playerManager.HeartAche.onValueDecreaseFromOne += HeartAcheDifferentFromZero;
        _playerManager.HeartAche.onValueBecomesOne += HeartAcheIsOne;
    }

    protected override void CheckValue(float timeDelay)
    {
        _isIncrease = _playerManager.IsWakeUp ? false : true;

        _extra = 0f;
        if (_modifiers.Count == 0)
        {
            _extra = timeDelay * (_isIncrease ? 0 : _decrease);
        }

        for (int i = 0; i < _stats.Length; ++i)
        {
            if (_stats[i] == StatsRange.Zero)
            {
                _extra -= _decrease * timeDelay;
            }
        }
        for (int i = 0; i < _illnesses.Length; ++i)
        {
            if (_illnesses[i] == StatsRange.Zero_One)
            {
                _extra -= _decrease * timeDelay / 2f;
            }
            if (_illnesses[i] == StatsRange.One)
            {
                _extra -= _decrease * timeDelay;
            }
        }

        if (!_playerManager.IsWakeUp)
        {
            _extra /= 2f;
        }
    }

    private void EnduranceIsZero() => _stats[0] = StatsRange.Zero;
    private void EnduranceIncreasesFromZero() => _stats[0] = StatsRange.Zero_One;

    private void RestIsZero() => _stats[1] = StatsRange.Zero;
    private void RestIncreasesFromZero() => _stats[1] = StatsRange.Zero_One;

    private void HungerIsZero() => _stats[2] = StatsRange.Zero;
    private void HungerIncreasesFromZero() => _stats[2] = StatsRange.Zero_One;

    private void ThirstIsZero() => _stats[3] = StatsRange.Zero;
    private void ThirstIncreasesFromZero() => _stats[3] = StatsRange.Zero_One;

    private void SunStrokeIsZero() => _illnesses[0] = StatsRange.Zero;
    private void SunStrokeDifferentFromZero() => _illnesses[0] = StatsRange.Zero_One;
    private void SunStrokeIsOne() => _illnesses[0] = StatsRange.One;

    private void StomacAcheIsZero() => _illnesses[1] = StatsRange.Zero;
    private void StomacAcheDifferentFromZero() => _illnesses[1] = StatsRange.Zero_One;
    private void StomacAcheIsOne() => _illnesses[1] = StatsRange.One;

    private void HeartAcheIsZero() => _illnesses[2] = StatsRange.Zero;
    private void HeartAcheDifferentFromZero() => _illnesses[2] = StatsRange.Zero_One;
    private void HeartAcheIsOne() => _illnesses[2] = StatsRange.One;
}