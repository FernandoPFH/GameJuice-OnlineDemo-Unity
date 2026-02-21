using UnityEngine;
using System;
using System.Collections.Generic;

public abstract class EffectSO : ScriptableObject
{
    [SerializeField] protected bool isEnabled;

#if UNITY_EDITOR
    private bool lastIsEnabledStatus;
#endif

    public virtual void OnStart()
    {
#if UNITY_EDITOR
        lastIsEnabledStatus = false;

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

    public virtual void OnDestroy()
    {
        if (isEnabled)
            OnDisabled();
    }

#if UNITY_EDITOR
    public virtual void OnGizmosDraw() { }

    protected virtual void InitValue<T>(ref T lastValue, T currentValue)
    => lastValue = currentValue;

    protected virtual void InitValues() { }

    protected virtual void CheckValueChanged<T>(ref T lastValue, T currentValue, Action<T> onValueChanged)
    {
        if (lastValue.Equals(currentValue))
            return;

        lastValue = currentValue;
        onValueChanged((T)currentValue);
    }

    protected virtual void CheckValuesChanged() { }
#endif

    public virtual void OnEnabled() { }
    public virtual void OnDisabled() { }
}
