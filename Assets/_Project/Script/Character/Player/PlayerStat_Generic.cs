using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GTM = S_TimeManager;

[Serializable]
public abstract class PlayerStat_Generic
{
    public class Modifier
    {
        private int _duration;
        public float Moltiplier { get; private set; }
        private Action<Modifier> _onDestroy;

        public Modifier(int duration, float moltiplier, Action<Modifier> onDestroy)
        {
            _duration = duration;
            Moltiplier = moltiplier;
            _onDestroy = onDestroy;
            GTM.Instance.onSecondDayChange += UpdateDuration;
        }

        private void UpdateDuration()
        {
            _duration -= GTM.Instance.SecondDelay;
            if (_duration <= 0)
            {
                _onDestroy?.Invoke(this);
            }
        }
    }

    private float _value = 1f;
    public float value { get => _value; protected set => _value = Mathf.Clamp01(value); }

    private bool _isAlreadySubscribe;
    protected List<Modifier> _modifiers = new List<Modifier>(2);

    public event Action onValueZero;
    public event Action onValueOne;

    public void Subscribe()
    {
        if (!_isAlreadySubscribe)
        {
            GTM.Instance.onSecondDayChange += UpdateValue;
            _isAlreadySubscribe = true;
        }
    }

    public void UnSubscribe()
    {
        if (_isAlreadySubscribe)
        {
            GTM.Instance.onSecondDayChange -= UpdateValue;
            _isAlreadySubscribe = false;
        }
    }

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

    public void RemoveModifier(Modifier modifier)
    {
        _modifiers.Remove(modifier);
    }
}
