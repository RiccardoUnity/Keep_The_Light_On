using GWM = GameWorldManager;

public class PS_Rest : PlayerStat
{
    #region LikeSingleton
    private PS_Rest() : base() { }
    public static PS_Rest Instance(int key)
    {
        if (key == Key.GetKey())
        {
            return new PS_Rest();
        }
        return null;
    }
    #endregion

    private int _minutesGameTimeToCompleateRest = 600;
    private int _secondsRealTimeToCompleateRest;

    private PlayerController _playerController;
    private float _walkMoltiplier;
    private float _runMoltiplier;
    private float _jumpMoltiplier;

    protected override void OnStart()
    {
        _playerController = _playerManager.PlayerController;
        _secondsRealTimeToCompleateRest = (int)((_minutesGameTimeToCompleateRest * 60) / _timeManager.RealSecondToGameSecond);
        _increase = (float)_timeManager.SecondDelay / _secondsRealTimeToCompleateRest;
        _decrease = _increase / 3f * 2f;
    }

    protected override void CheckValue()
    {
        
    }
    
    protected override void SetValue()
    {
        if (_playerManager.IsWakeUp)
        {
            Value -= _decrease * _moltiplierDecrease;
        }
        else
        {
            Value += _increase * _moltiplierIncrease;
        }
    }
}