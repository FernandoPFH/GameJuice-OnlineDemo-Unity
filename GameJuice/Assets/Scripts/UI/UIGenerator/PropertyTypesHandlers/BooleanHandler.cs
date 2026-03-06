using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Reflection;
using System.Collections.Generic;

public class BooleanHandler : PropertyHandler<bool>
{
    public Toggle toggleField;

    public static HashSet<BooleanHandler> Enablers = new();

    protected override void Start()
    {
        base.Start();
        toggleField.isOn = lastValue;

        if (method.Name == "OnIsEnabled")
            Enablers.Add(this);
    }

    public void OnValueChanged(bool input)
    {
        method.Invoke(effectHandler.Effect, new object[] { input });
        lastValue = input;
    }
}
