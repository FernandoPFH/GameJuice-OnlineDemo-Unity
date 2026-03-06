using UnityEngine;
using UnityEditor;
using TMPro;
using System.Reflection;
using System.Linq;
using System.Text.RegularExpressions;

public abstract class PropertyHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] protected EffectHandler effectHandler;
    [SerializeField] private string methodName;
    [SerializeField] private string propertyName;

    protected MethodInfo method;
    protected ParameterInfo parameter;
    protected PropertyInfo property;

    protected virtual void Start()
    {
        method = effectHandler.Effect.GetType().GetMethod(methodName);
        parameter = method.GetParameters().First();
        property = effectHandler.Effect.GetType().GetProperty(propertyName);
    }

    public void SetName(string name)
        => title.text = AddSpacesBeforeUppercase(name);

    public void SetEffectHandler(EffectHandler handler)
        => effectHandler = handler;

    public string AddSpacesBeforeUppercase(string input)
    {
        return Regex.Replace(input, "(?<!^)([A-Z])", " $1");
    }

#if UNITY_EDITOR
    public virtual void SetupProperty(MethodInfo method, ParameterInfo parameter)
    {
        methodName = method.Name;
        propertyName = methodName.Replace("On", "").Replace("Changed", "");
        SetName(propertyName);
    }
#endif
}

public abstract class PropertyHandler<T> : PropertyHandler
{
    protected T lastValue;

    protected override void Start()
    {
        base.Start();
        lastValue = (T)property.GetValue(effectHandler.Effect);
    }
}
