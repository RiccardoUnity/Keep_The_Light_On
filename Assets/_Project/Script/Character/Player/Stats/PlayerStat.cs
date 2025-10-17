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
            GWM.Instance.TimeManager.onNormalPriority += UpdateDuration;
        }

        private void UpdateDuration()
        {
            _duration -= GWM.Instance.TimeManager.SecondDelay;
            if (_duration <= 0)
            {
                _onDestroy?.Invoke(this);
            }
        }
    }

    public static Key Key = new Key();
    
    public float Value { get => _value; protected set => _value = Mathf.Clamp01(value); }
    protected float _value = 1f;

    protected float _increase;
    protected float _decrease;

    private bool _isMyStart;
    protected List<Modifier> _modifiers = new List<Modifier>(2);
    protected float _moltiplierIncrease;
    protected float _moltiplierDecrease;

    public event Action onValueZero;
    public event Action onValueOne;

    protected TimeManager _timeManager;
    protected PlayerManager _playerManager;

    public virtual bool MyStart()
    {
        if (_isMyStart)
        {
            return false;
        }
        else
        {
            _isMyStart = true;
            _timeManager = GWM.Instance.TimeManager;
            _timeManager.onNormalPriority += UpdateValue;
            _playerManager = GWM.Instance.PlayerManager;
            OnStart();
            return true;
        }
    }

    protected abstract void OnStart();

    public void UpdateValue()
    {
        SetMoltiplier();
        CheckValue();
        SetValue();
        CallEvent();
    }

    protected abstract void CheckValue();
    protected abstract void SetValue();

    protected virtual void CallEvent()
    {
        if (Value <= 0f)
        {
            Value = 0f;
            onValueZero?.Invoke();
        }
        else if (Value >= 1f)
        {
            Value = 1f;
            onValueOne?.Invoke();
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
