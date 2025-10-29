using System;
using System.Collections.Generic;
using UnityEngine;
using GWM = GameWorldManager;

public abstract class PlayerStat
{
    protected class Modifier
    {
        public float Value { get; private set; }
        public bool IsIncrease { get; private set; }
        private float _duration;
        private Action<Modifier> _onDestroy;

        public Modifier(float value, bool isIncrease, float duration, Action<Modifier> onDestroy)
        {
            Value = value;
            IsIncrease = isIncrease;
            _duration = duration;
            _onDestroy = onDestroy;
            GWM.Instance.TimeManager.onPriority += UpdateNormalPriority;
        }

        private void UpdateNormalPriority(float timeDelay)
        {
            _duration -= timeDelay;
            if (_duration <= 0)
            {
                GWM.Instance.TimeManager.onPriority -= UpdateNormalPriority;
                _onDestroy?.Invoke(this);
            }
        }
    }

    public static Key Key = new Key();
    protected bool _debug;
    public string Name { get; protected set; }

    public float Value { get => _value; protected set => _value = Mathf.Clamp01(value); }
    protected float _value = 1f;
    private bool _isValueZero;
    private bool _isValueOne;

    protected float _increase;
    protected float _decrease;
    protected bool _isIncrease;
    protected float _modifier;
    protected List<Modifier> _modifiers = new List<Modifier>(2);
    protected float _extra;

    protected int _minutesRealTimeToCompleteIncrease;
    protected float _minutesRealTimeToCompleteDecrease;

    private bool _isMyAwake;
    private bool _isMyStart;

    public event Action onValueBecomesZero;
    public event Action onValueIncreasesFromZero;
    public event Action onValueBecomesOne;
    public event Action onValueDecreaseFromOne;
    protected bool _forceMiddleCall = true;

    protected TimeManager _timeManager;
    protected PlayerManager _playerManager;
    protected PlayerController _playerController;

    protected virtual bool MyAwake(bool debug, float startValue)
    {
        if (_isMyAwake)
        {
            return false;
        }
        else
        {
            _isMyAwake = true;
            _debug = debug;
            _value = Mathf.Clamp01(startValue);
            if (_value == 0f)
            {
                _forceMiddleCall = false;
            }
            else if (_value == 1f)
            {
                _forceMiddleCall = false;
            }
            _timeManager = GWM.Instance.TimeManager;
            _timeManager.onPriority += UpdateNormalPriority;
            _playerManager = GWM.Instance.PlayerManager;
            _playerController = _playerManager.PlayerController;

            OnAwake();

            float secondsGameTime;
            if (_minutesRealTimeToCompleteIncrease != 0)
            {
                secondsGameTime = ((_minutesRealTimeToCompleteIncrease * 60) / _timeManager.RealSecondToGameSecond);
                _increase = 1f / secondsGameTime;
            }
            if (_minutesRealTimeToCompleteDecrease != 0)
            {
                secondsGameTime = ((_minutesRealTimeToCompleteDecrease * 60) / _timeManager.RealSecondToGameSecond);
                _decrease = 1f / secondsGameTime;
            }

            if (_debug)
            {
                Debug.Log($"My Awake of {Name} is complete - Decrease: {_decrease} - Increase: {_increase}");
            }
            return true;
        }
    }

    protected abstract void OnAwake();

    protected virtual bool MyStart()
    {
        if (_isMyStart)
        {
            return false;
        }
        else
        {
            _isMyStart = true;
            OnStart();
            return true;
        }
    }
    protected abstract void OnStart();

    private void UpdateNormalPriority(float timeDelay)
    {
        SetModifier(timeDelay);
        CheckValue(timeDelay);
        SetValue(timeDelay);
        CallEvent();

        if (_debug)
        {
            Debug.Log($"{Name} value: {_value} - IsIncrease: {_isIncrease} - Extra: {_extra}");
        }
    }

    protected abstract void CheckValue(float timeDelay);

    protected void SetValue(float timeDelay)
    {
        if (_isIncrease)
        {
            Value += _increase * timeDelay + _modifier + _extra;
        }
        else
        {
            Value += -_decrease * timeDelay + _modifier + _extra;
        }
    }

    public void CallEvent()
    {
        if (Value <= 0f && !_isValueZero)
        {
            Value = 0f;
            _isValueZero = true;
            onValueBecomesZero?.Invoke();
        }
        else if (Value > 0f && _isValueZero || _forceMiddleCall)
        {
            _isValueZero = false;
            _forceMiddleCall = false;
            onValueIncreasesFromZero?.Invoke();
        }
        else if (Value >= 1f && !_isValueOne)
        {
            Value = 1f;
            _isValueOne = true;
            onValueBecomesOne?.Invoke();
        }
        else if (Value < 1f && _isValueOne)
        {
            _isValueOne = false;
            onValueDecreaseFromOne?.Invoke();
        }
    }

    public void AddModifier(float moltiplier, bool isIncrease, float duration)
    {
        Modifier modifier = new Modifier(moltiplier, isIncrease, duration, RemoveModifier);
        _modifiers.Add(modifier);
    }

    private void RemoveModifier(Modifier modifier)
    {
        _modifiers.Remove(modifier);
        if (_debug)
        {
            Debug.Log($"{Name} - Modificatore distrutto, rimanenti " + _modifiers.Count);
        }
    }

    private void SetModifier(float timeDelay)
    {
        _modifier = 0f;
        if (_modifiers.Count > 0)
        {
            foreach (Modifier modifier in _modifiers)
            {
                _modifier += modifier.Value * timeDelay * (modifier.IsIncrease ? 1 : -1);
            }
        }
    }

}
