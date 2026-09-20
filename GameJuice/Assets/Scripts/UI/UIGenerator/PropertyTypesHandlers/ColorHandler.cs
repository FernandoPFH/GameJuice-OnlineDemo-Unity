using UnityEngine;

using FernandoPFH_Essentials_Runtime;

public class ColorHandler : PropertyHandler<Color>
{
    [SerializeField] protected ColorPickerUI colorPickerField;

    protected override void Start()
    {
        base.Start();
        colorPickerField.ToggleHDRLabel(false);
        colorPickerField.SetColor(lastValue);
    }

    public void OnValueChanged(Color input)
    {
        method.Invoke(effectHandler.Effect, new object[] { input });
        lastValue = input;
    }
}
