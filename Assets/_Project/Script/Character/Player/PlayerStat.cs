using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class PlayerStat
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
            GameWorldManager.Instance.timeGame.onSecondDayChange += UpdateDuration;
        }

        private void UpdateDuration()
        {
            _duration -= GameWorldManager.Instance.timeGame.SecondDelay;
            if (_duration <= 0)
            {
                _onDestroy?.Invoke(this);
            }
        }
    }

    private float _value = 1f;
    public float value { get => _value; protected set => _value = Mathf.Clamp01(value); }

    private bool _hasAlreadyAwaked;
    protected List<Modifier> _modifiers = new List<Modifier>(2);

    public event Action onValueZero;
    public event Action onValueOne;

    protected static int GenerateKey() => 11;

    public virtual bool MyStart()
    {
        if (_hasAlreadyAwaked)
        {
            return false;
        }
        else
        {
            _hasAlreadyAwaked = true;
            GameWorldManager.Instance.timeGame.onSecondDayChange += UpdateValue;
            OnAwake();
            return true;
        }
    }

    protected abstract void OnAwake();

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
