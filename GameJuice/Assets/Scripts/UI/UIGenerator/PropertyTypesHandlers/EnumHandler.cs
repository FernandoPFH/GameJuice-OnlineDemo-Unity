using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using TMPro;

public class EnumHandler : PropertyHandler<int>
{
    [SerializeField] protected TMP_Dropdown dropdownField;

    protected override void Start()
    {
        base.Start();
        dropdownField.value = lastValue;
    }

    public void OnValueChanged(int input)
    {
        method.Invoke(effectHandler.Effect, new object[] { input });
        lastValue = input;
    }

#if UNITY_EDITOR
    public override void SetupProperty(MethodInfo method, ParameterInfo parameter)
    {
        base.SetupProperty(method, parameter);
        dropdownField.ClearOptions();
        List<string> options = new();
        foreach (var x in Enum.GetValues(parameter.ParameterType))
            options.Add(x.ToString());
        dropdownField.AddOptions(options);
    }
#endif
}
