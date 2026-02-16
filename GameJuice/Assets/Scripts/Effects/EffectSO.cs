using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class EffectSO : ScriptableObject
{

    [SerializeField] protected bool isEnabled;

#if UNITY_EDITOR
    protected virtual List<(object, object, Action<object>)> valuesToCheck { get; set; }

    private bool lastIsEnabledStatus;
#endif

    public virtual void OnStart()
    {
#if UNITY_EDITOR
        InitValues();
#endif
    }

    public virtual void OnUpdate()
    {
#if UNITY_EDITOR
        if (lastIsEnabledStatus != isEnabled)
        {
            lastIsEnabledStatus = isEnabled;
            if (isEnabled)
                OnEnabled();
            else
                OnDisabled();
        }

        CheckValuesChanged();
#endif
    }

#if UNITY_EDITOR
    protected virtual void InitValue(object lastValue, object currentValue)
        => lastValue = currentValue;

    protected virtual void CheckValue(object lastValue, object currentValue, Action<object> onValueChanged)
    {
        if (lastValue == currentValue)
            return;

        lastValue = currentValue;
        onValueChanged(currentValue);
    }

    protected virtual void InitValues()
    {
        foreach ((object lastValue, object currentValue, Action<object> onValueChanged) in valuesToCheck)
            InitValue(lastValue, currentValue);
    }

    protected virtual void CheckValuesChanged()
    {
        foreach ((object lastValue, object currentValue, Action<object> onValueChanged) in valuesToCheck)
            CheckValue(lastValue, currentValue, onValueChanged);

    }
#endif

    public virtual void OnEnabled() { }
    public virtual void OnDisabled() { }
}
