using UnityEngine;
using UnityEditor;
using System.Reflection;
using TMPro;

public abstract class NumberHandler<T> : PropertyHandler<T> where T : struct
{
    [SerializeField] protected TMP_InputField inputField;

    protected override void Start()
    {
        base.Start();
        inputField.text = lastValue.ToString();
    }

    protected abstract void HandleInput(string input);

    public void OnValueChanged(string input)
        => HandleInput(input.Replace(".", ","));
}
