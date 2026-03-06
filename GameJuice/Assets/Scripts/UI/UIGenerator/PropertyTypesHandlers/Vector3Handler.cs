using UnityEngine;
using UnityEditor;
using System;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using TMPro;

public class Vector3Handler : PropertyHandler<Vector3>
{
    [SerializeField] protected TMP_InputField inputField_X;
    [SerializeField] protected TMP_InputField inputField_Y;
    [SerializeField] protected TMP_InputField inputField_Z;

    protected override void Start()
    {
        base.Start();
        inputField_X.text = lastValue.x.ToString();
        inputField_Y.text = lastValue.y.ToString();
        inputField_Z.text = lastValue.z.ToString();
    }

    public void OnValueChangedX(string input)
    {
        if (!float.TryParse(input.Replace(".", ","), out float parsedValue))
        {
            inputField_X.text = lastValue.x.ToString();
            return;
        }

        lastValue.x = parsedValue;
        method.Invoke(effectHandler.Effect, new object[] { lastValue });
    }

    public void OnValueChangedY(string input)
    {
        if (!float.TryParse(input.Replace(".", ","), out float parsedValue))
        {
            inputField_Y.text = lastValue.y.ToString();
            return;
        }

        lastValue.y = parsedValue;
        method.Invoke(effectHandler.Effect, new object[] { lastValue });
    }

    public void OnValueChangedZ(string input)
    {
        if (!float.TryParse(input.Replace(".", ","), out float parsedValue))
        {
            inputField_Z.text = lastValue.z.ToString();
            return;
        }

        lastValue.z = parsedValue;
        method.Invoke(effectHandler.Effect, new object[] { lastValue });
    }
}
