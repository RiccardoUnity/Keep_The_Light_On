using System;

public class PS_Stamina : PlayerStat
{
    #region LikeSingleton
    private PS_Stamina() : base() { }
    public static PS_Stamina Instance(int key, out Func<bool, float, bool> myAwake, out Func<bool> myStart)
    {
        if (key == Key.GetKey())
        {
            PS_Stamina stamina = new PS_Stamina();
            myAwake = stamina.MyAwake;
            myStart = stamina.MyStart;
            return stamina;
        }
        myAwake = null;
        myStart = null;
        return null;
    }
    #endregion

    public float DecreaseJump {  get; private set; }
    private bool _isJumping;

    protected override void OnAwake()
    {
        Name = "Stamina";

        _minutesRealTimeToCompleteIncrease = 10;
        _minutesRealTimeToCompleteDecrease = 30;

        _playerController.onJump += OnJump;
    }

    protected override void OnStart()
    {
        DecreaseJump = _decrease * 2;
    }

    protected override void CheckValue(float timeDelay)
    {
        _isIncrease = _playerController.EnergyToProcess > 0 ? false : true;

        _extra = 0f;
        if (!_isIncrease)
        {
            if (_playerController.IsRun)
            {
                _extra += _decrease * timeDelay * -1;
            }
            else
            {
                _extra += _decrease * timeDelay;
            }
            if (_isJumping)
            {
                _isJumping = false;
                _extra += -DecreaseJump;
            }
        }
    }

    private void OnJump() => _isJumping = true;
}