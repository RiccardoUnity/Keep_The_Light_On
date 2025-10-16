using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class PlayerStat
{
    protected class Modifier
    {
        private int _duration;
        public float Moltiplier { get; private set; }
        private Action<Modifier> _onDestroy;

        public Modifier(int duration, float moltiplier, Action<Modifier> onDestroy)
        {
            _duration = duration;
            Moltiplier = moltiplier;
            _onDestroy = onDestroy;
            GameWorldManager.Instance.TimeManager.onSecondDayChange += UpdateDuration;
        }

        private void UpdateDuration()
        {
            _duration -= GameWorldManager.Instance.TimeManager.SecondDelay;
            if (_duration <= 0)
            {
                _onDestroy?.Invoke(this);
            }
        }
    }

    public static Key Key = new Key();

    private float _value = 1f;
    public float value { get => _value; protected set => _value = Mathf.Clamp01(value); }

    private bool _hasAlreadyStarted;
    protected List<Modifier> _modifiers = new List<Modifier>(2);

    public event Action onValueZero;
    public event Action onValueOne;

    protected TimeManager _timeManager;

    public virtual bool MyStart()
    {
        if (_hasAlreadyStarted)
        {
            return false;
        }
        else
        {
            _hasAlreadyStarted = true;
            _timeManager = GameWorldManager.Instance.TimeManager;
            _timeManager.onSecondDayChange += UpdateValue;
            OnStart();
            return true;
        }
    }

    protected abstract void OnStart();

    public void UpdateValue()
    {
        CheckValue();
        SetValue();
        CallEvent();
    }

    protected abstract void CheckValue();
    protected abstract void SetValue();

    protected virtual void CallEvent()
    {
        if (value <= 0f)
        {
            value = 0f;
            onValueZero?.Invoke();
        }
        else if (value >= 1f)
        {
            value = 1f;
            onValueOne?.Invoke();
        }
    }

    public void AddModifier(int duration, float moltiplier)
    {
        Modifier modifier = new Modifier(duration, moltiplier, RemoveModifier);
        _modifiers.Add(modifier);
    }

    private void RemoveModifier(Modifier modifier)
    {
        _modifiers.Remove(modifier);
    }
}
