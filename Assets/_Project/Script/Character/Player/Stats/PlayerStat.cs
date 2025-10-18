using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GWM = GameWorldManager;

public abstract class PlayerStat
{
    protected class Modifier
    {
        public float Moltiplier { get; private set; }
        public bool IsIncrease { get; private set; }
        private int _duration;
        private Action<Modifier> _onDestroy;

        public Modifier(float moltiplier, bool isIncrease, int duration, Action<Modifier> onDestroy)
        {
            Moltiplier = moltiplier;
            IsIncrease = isIncrease;
            _duration = duration;
            _onDestroy = onDestroy;
            GWM.Instance.TimeManager.onNormalPriority += UpdateNormalPriority;
        }

        private void UpdateNormalPriority(int secondsDelay)
        {
            _duration -= secondsDelay;
            if (_duration <= 0)
            {
                _onDestroy?.Invoke(this);
            }
        }
    }

    public static Key Key = new Key();

    protected static bool _debug;

    public float Value { get => _value; protected set => _value = Mathf.Clamp01(value); }
    protected float _value = 1f;
    private bool _isValueZero;

    protected float _increase;
    protected float _decrease;

    private bool _isMyAwake;
    protected List<Modifier> _modifiers = new List<Modifier>(2);
    protected float _moltiplierIncrease;
    protected float _moltiplierDecrease;

    public event Action onValueBecomesZero;
    public event Action onValueIncreasesFromZero;

    protected TimeManager _timeManager;
    protected PlayerManager _playerManager;

    protected virtual bool MyAwake()
    {
        if (_isMyAwake)
        {
            return false;
        }
        else
        {
            _isMyAwake = true;
            _timeManager = GWM.Instance.TimeManager;
            _timeManager.onNormalPriority += UpdateNormalPriority;
            _playerManager = GWM.Instance.PlayerManager;
            OnAwake();
            return true;
        }
    }

    protected abstract void OnAwake();

    private void UpdateNormalPriority(int secondsDelay)
    {
        SetMoltiplier();
        CheckValue();
        SetValue(secondsDelay);
        CallEvent();
    }

    protected abstract void CheckValue();
    protected abstract void SetValue(int secondsDelay);

    protected virtual void CallEvent()
    {
        if (Value <= 0f && !_isValueZero)
        {
            Value = 0f;
            _isValueZero = true;
            onValueBecomesZero?.Invoke();
        }
        else if (Value >= 0f && _isValueZero)
        {
            _isValueZero = false;
            onValueIncreasesFromZero?.Invoke();
        }
    }

    public void AddModifier(float moltiplier, bool isIncrease, int duration)
    {
        Modifier modifier = new Modifier(moltiplier, isIncrease, duration, RemoveModifier);
        _modifiers.Add(modifier);
    }

    private void RemoveModifier(Modifier modifier)
    {
        _modifiers.Remove(modifier);
    }

    private void SetMoltiplier()
    {
        _moltiplierIncrease = 1f;
        _moltiplierDecrease = 1f;
        foreach (Modifier modifier in _modifiers)
        {
            if (modifier.IsIncrease)
            {
                _moltiplierIncrease *= modifier.Moltiplier;
            }
            else
            {
                _moltiplierDecrease *= modifier.Moltiplier;
            }
        }
    }
}
