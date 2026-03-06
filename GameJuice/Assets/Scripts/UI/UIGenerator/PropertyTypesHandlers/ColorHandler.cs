using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System.Reflection;

public class ColorHandler : PropertyHandler<Color>
{
    [SerializeField] protected ColorPickerUI colorPickerField;

    protected override void Start()
    {
        base.Start();
        colorPickerField.ToggleHDRLabel(false);
        colorPickerField.SetColor(lastValue);
    }

    public void OnValueChanged(ColorData input)
    {
        method.Invoke(effectHandler.Effect, new object[] { input.ColorWithoutHDR });
        lastValue = input.ColorWithoutHDR;
    }
}
