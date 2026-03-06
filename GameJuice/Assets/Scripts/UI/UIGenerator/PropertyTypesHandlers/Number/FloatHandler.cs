using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;
using System.Reflection;
using TMPro;

public class FloatHandler : NumberHandler<float>
{
    protected override void HandleInput(string input)
    {
        if (!float.TryParse(input, out float parsedValue))
        {
            inputField.text = lastValue.ToString();
            return;
        }

        method.Invoke(effectHandler.Effect, new object[] { parsedValue });
        lastValue = parsedValue;
    }
}
