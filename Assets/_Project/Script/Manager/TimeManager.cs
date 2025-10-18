using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWM = GameWorldManager;

public class TimeManager
{
    #region LikeSingleton
    private TimeManager()
    {
        if (GWM.Instance.MainDebug)
        {
            Debug.Log("TimeManager Initialized");
        }
    }

    public static TimeManager Instance(int key)
    {
        if (key == Key.GetKey())
        {
            return new TimeManager();
        }
        return null;
    }
    
    public static Key Key = new Key();
    #endregion

    private int _gameDayInRealSeconds = -1;
    public float RealSecondToGameSecond {  get; private set; }
    public int CurrentSecondDay { get; private set; }
    public int CurrentDay { get; private set; }
    private const int _secondDelay = 1;
    private bool _isPauseCalled;
    private int _accelerationInGameSecond;
    private int _accelerationMoltiplier;
    private bool _isStartTimeSetted;
    private bool _isStarted;
    private bool _isTimeGameOn = true;
    public GameTimeType GameTimeType { get; private set; }
    private bool _gameplayNotPriority;
    private IEnumerator _gameTime;
    private WaitForSeconds _delay;

    private Transform _mainLight;
    private Vector3 _angleSecondLightV3;

    public DayTime DayTime { get; private set; }
    private int[] _daylyBand = new int[4];

    public event Action onPause;
    public event Action<int> onNormalPriority;
    public event Action<int> onNormalNotPriority1;
    public event Action<int> onNormalNotPriority2;
    public event Action onDayChange;
    public event Action onDawn;
    public event Action onDay;
    public event Action onDusk;
    public event Action onNight;

    //In Loading
    public bool StartTime(int currentSecondDay, int currentDay)
    {
        if (_isStartTimeSetted)
        {
            Debug.LogError("Time has already setted");
            return false;
        }
        else
        {
            _isStartTimeSetted = true;
            CurrentSecondDay = currentSecondDay;
            CurrentDay = currentDay;
            return true;
        }
    }

    //In Start of GameSceneManager
    public bool MyStart(Light mainLight, out IEnumerator time)
    {
        if (_isStarted)
        {
            Debug.LogError("Time has already started");
            time = null;
            return false;
        }
        else
        {
            _isStarted = true;

            _gameDayInRealSeconds = GWM.Instance.GameDayInRealMinutes * 60;
            RealSecondToGameSecond = (24 * 60 * 60) / _gameDayInRealSeconds;

            if (!S_SaveSystem.HasALoading)
            {
                int startHours = GWM.Instance.StartHours;
                int startMinutes = GWM.Instance.StartMinutes;
                CurrentSecondDay = _gameDayInRealSeconds * ((startHours * 60 * 60) + startMinutes * 60) / (24 * 60 * 60);
            }

            _mainLight = mainLight.transform;
            SetRotationMainLight();

            GameTimeType = GameTimeType.Normal;
            SetDailyBand();
            _delay = new WaitForSeconds(_secondDelay);
            time = GameTime();
            
            return true;
        }
    }

    //00.00 => 270°; 06.00 => 180°; 12.00 => 90°; 18.00 => 0°;
    public void SetRotationMainLight()
    {
        float angleSecondLight = 360f / _gameDayInRealSeconds;
        float angleXLight = (angleSecondLight * _gameDayInRealSeconds * 0.75f) - (angleSecondLight * CurrentSecondDay);
        _mainLight.eulerAngles = new Vector3(angleXLight, _mainLight.eulerAngles.y, _mainLight.eulerAngles.z);
        _angleSecondLightV3 = new Vector3(angleSecondLight, 0f, 0f);
    }

    private void SetDailyBand()
    {
        int secondHour = _gameDayInRealSeconds / 24;
        _daylyBand[0] = secondHour * 5;
        _daylyBand[1] = secondHour * 7;
        _daylyBand[2] = secondHour * 17;
        _daylyBand[3] = secondHour * 19;

        if (CurrentSecondDay < _daylyBand[0])
        {
            DayTime = DayTime.Night;
        }
        else if (CurrentDay < _daylyBand[1])
        {
            DayTime = DayTime.Dawn;
        }
        else if (CurrentDay < _daylyBand[2])
        {
            DayTime = DayTime.Day;
        }
        else if (CurrentDay < _daylyBand[3])
        {
            DayTime = DayTime.Dusk;
        }
        else
        {
            DayTime = DayTime.Night;
        }
    }

    private void SetIsGameTimeOn(bool value) => _isTimeGameOn = value;

    private IEnumerator GameTime()
    {
        while (_isTimeGameOn)
        {
            if (GWM.Instance.IsGamePause)
            {
                GameTimeType = GameTimeType.Pause;
            }
            else
            {
                GameTimeType = GameTimeType.Normal;
                _isPauseCalled = false;
            }

            switch (GameTimeType)
            {
                case GameTimeType.Pause:
                    if (!_isPauseCalled)
                    {
                        _isPauseCalled = true;
                        onPause?.Invoke();
                    }
                    yield return null;
                    break;
                case GameTimeType.Normal:
                    GamePlay(1);
                    yield return _delay;
                    break;
                case GameTimeType.Accelerate:
                    if (_accelerationInGameSecond > 0)
                    {
                        GamePlay(_accelerationMoltiplier);
                        _accelerationInGameSecond -= _accelerationMoltiplier;
                    }
                    else
                    {
                        GameTimeType = GameTimeType.Normal;
                    }
                    yield return null;
                    break;
            }
        }
    }

    private void GamePlay(int moltiplier)
    {
        _gameplayNotPriority = !_gameplayNotPriority;
        if (_gameplayNotPriority)
        {
            onNormalNotPriority1?.Invoke(_secondDelay * moltiplier);
        }
        else
        {
            onNormalNotPriority2?.Invoke(_secondDelay * moltiplier);
        }

        CurrentSecondDay += _secondDelay * moltiplier;
        onNormalPriority?.Invoke(_secondDelay * moltiplier);
        //A day is passed
        if (CurrentSecondDay >= _gameDayInRealSeconds)
        {
            ++CurrentDay;
            CurrentSecondDay -= _gameDayInRealSeconds;
            onDayChange?.Invoke();
        }

        //Main Light change direction
        _mainLight.eulerAngles += _angleSecondLightV3 * _secondDelay * moltiplier;
        SetDayTime();
    }

    private void SetDayTime()
    {
        if (DayTime == DayTime.Night && CurrentSecondDay > _daylyBand[0])
        {
            DayTime = DayTime.Dawn;
            onDawn?.Invoke();
        }
        else if (DayTime == DayTime.Dawn && CurrentSecondDay > _daylyBand[1])
        {
            DayTime = DayTime.Day;
            onDay?.Invoke();
        }
        else if (DayTime == DayTime.Day && CurrentSecondDay > _daylyBand[2])
        {
            DayTime = DayTime.Dusk;
            onDusk?.Invoke();
        }
        else if (DayTime == DayTime.Dusk && CurrentSecondDay > _daylyBand[3])
        {
            DayTime = DayTime.Night;
            onNight?.Invoke();
        }
    }

    public void SetGamePlayAccelerate(int realSecondAccelerate)
    {
        GameTimeType = GameTimeType.Accelerate;
        _accelerationInGameSecond = (int)(realSecondAccelerate / RealSecondToGameSecond);
        _accelerationMoltiplier = _accelerationInGameSecond / 300 + 1;

        //UI con animazione
    }
}
