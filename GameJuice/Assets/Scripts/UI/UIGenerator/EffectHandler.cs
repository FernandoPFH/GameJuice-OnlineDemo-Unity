using UnityEngine;
using UnityEditor;
using TMPro;
using NaughtyAttributes;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class EffectHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    public EffectSO Effect;
    [SerializeField] private Transform contentHolder;
    [SerializeField] private SerializableDictionary<string, GameObject> prefabsPerType;

    [HideInInspector] public string Name;

    public static HashSet<string> Names = new();

    private void Awake()
        => Names.Add(Name);

    public void SetName(string name)
    {
        Name = name;
        title.text = AddSpacesBeforeUppercase(name);
    }

    public string AddSpacesBeforeUppercase(string input)
    {
        return Regex.Replace(input, "(?<!^)([A-Z])", " $1");
    }

#if UNITY_EDITOR
    private GameObject InstantiatePrefab(GameObject prefab, Transform parent)
    {
        GameObject gb = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        gb.transform.SetParent(parent);
        return gb;
    }

    [Button]
    private void GenerateMissingProperties()
        => GenerateMissingProperties(Effect);

    public void GenerateMissingProperties(EffectSO effect)
    {
        Effect = effect;

        if (prefabsPerType.TryGetValue("System.Boolean", out GameObject ePrefab))
        {
            PropertyHandler propertyHandler = InstantiatePrefab(ePrefab, contentHolder).GetComponent<PropertyHandler>();
            propertyHandler.SetEffectHandler(this);
            MethodInfo method = effect.GetType().GetMethod("OnIsEnabled");
            propertyHandler.SetupProperty(method, method.GetParameters().First());
        }

        foreach (MethodInfo method in effect.GetType().GetMethods().Where(x => x.Name.Contains("On") && x.Name.Contains("Changed")))
        {
            string methodName = method.Name;
            string parameterType = method.GetParameters().First().ParameterType.ToString();
            if (method.GetParameters().First().ParameterType.IsEnum)
                parameterType = "Enum";

            if (prefabsPerType.TryGetValue(parameterType, out GameObject prefab))
            {
                PropertyHandler propertyHandler = InstantiatePrefab(prefab, contentHolder).GetComponent<PropertyHandler>();
                propertyHandler.SetEffectHandler(this);
                propertyHandler.SetupProperty(method, method.GetParameters().First());
            }
        }
    }
#endif
}
