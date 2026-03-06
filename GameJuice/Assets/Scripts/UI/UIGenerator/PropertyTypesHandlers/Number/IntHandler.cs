using UnityEngine;
using UnityEngine.Events;
using UnityEditor;
using System;
using System.Reflection;
using TMPro;

public class IntHandler : NumberHandler<int>
{
    protected override void HandleInput(string input)
    {
        if (!int.TryParse(input, out int parsedValue))
        {
            inputField.text = lastValue.ToString();
            return;
        }

        method.Invoke(effectHandler.Effect, new object[] { parsedValue });
        lastValue = parsedValue;
    }
}
