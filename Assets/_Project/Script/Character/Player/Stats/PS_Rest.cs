using System;
using Unity.Burst.CompilerServices;
using GWM = GameWorldManager;

public class PS_Rest : PlayerStat
{
    #region LikeSingleton
    private PS_Rest() : base() { }
    public static Func<bool> Instance(int key, out PS_Rest rest, bool debug = false)
    {
        if (key == Key.GetKey())
        {
            rest = new PS_Rest();
            _debug = debug;
            return rest.MyAwake;
        }
        rest = null;
        return null;
    }
    #endregion

    private int _minutesGameTimeToCompleateRest = 600;
    private int _secondsRealTimeToCompleateRest;

    private PlayerController _playerController;
    private float _walkMoltiplier;
    private float _runMoltiplier;
    private float _jumpMoltiplier;

    protected override void OnAwake()
    {
        _playerController = _playerManager.PlayerController;
        _secondsRealTimeToCompleateRest = (int)((_minutesGameTimeToCompleateRest * 60) / _timeManager.RealSecondToGameSecond);
        _increase = 1f / _secondsRealTimeToCompleateRest;
        _decrease = _increase / 3f * 2f;
    }

    protected override void CheckValue()
    {
        _walkMoltiplier = _playerController.ToProcess(true) * _decrease;
        _runMoltiplier = _playerController.ToProcess(false) * _decrease * 2;
        _jumpMoltiplier = _playerController.ToProcess() * _decrease * 4;
    }
    
    protected override void SetValue(int secondsDelay)
    {
        if (_playerManager.IsWakeUp)
        {
            Value -= _decrease * _moltiplierDecrease * secondsDelay - _walkMoltiplier - _runMoltiplier - _jumpMoltiplier;
        }
        else
        {
            Value += _increase * _moltiplierIncrease * secondsDelay;
        }
    }
}