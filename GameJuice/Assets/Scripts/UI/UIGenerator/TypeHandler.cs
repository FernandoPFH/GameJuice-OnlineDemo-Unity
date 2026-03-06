using UnityEngine;
using UnityEditor;
using System.Linq;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using NaughtyAttributes;

public class TypeHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Transform contentHolder;
    [SerializeField] private GameObject effectPrefab;

    [HideInInspector] public string Type;

    public void SetType(string type)
    {
        Type = type;
        title.text = AddSpacesBeforeUppercase(type);
    }

    private string ProcessName(string name)
        => name.Replace("_EffectSO", "");

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
    private void GenerateMissingEffects()
        => GenerateMissingEffects(UIGenerator.EffectsPerType[Type]);

    public void GenerateMissingEffects(List<EffectSO> effects)
    {
        List<EffectHandler> effectHandlersAlreadyCreated = contentHolder.GetComponentsInChildren<EffectHandler>().ToList();

        foreach (EffectSO effect in effects)
        {
            string name = ProcessName(effect.name);

            EffectHandler effectHandler;
            if (effectHandlersAlreadyCreated.Any(x => x.Name == name))
                effectHandler = effectHandlersAlreadyCreated.First(x => x.Name == name);
            else
            {
                effectHandler = InstantiatePrefab(effectPrefab, contentHolder).GetComponent<EffectHandler>();
                effectHandler.name = name;
                effectHandler.SetName(name);
            }

            effectHandler.GenerateMissingProperties(effect);
        }
    }
#endif
}
