using UnityEngine;
using TMPro;
using NaughtyAttributes;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Events;
#endif

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

        BooleanHandler effectOnEnableHandler = InstantiatePrefab(prefabsPerType["System.Boolean"], contentHolder).GetComponent<BooleanHandler>();
        effectOnEnableHandler.SetEffectHandler(this);
        MethodInfo onEnabledMethod = effect.GetType().GetMethod("OnIsEnabled");
        effectOnEnableHandler.SetupProperty(onEnabledMethod, onEnabledMethod.GetParameters().First());

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

                propertyHandler.gameObject.SetActive(false);
                UnityEventTools.AddPersistentListener(effectOnEnableHandler.toggleField.onValueChanged, propertyHandler.gameObject.SetActive);
            }
        }
    }
#endif
}
