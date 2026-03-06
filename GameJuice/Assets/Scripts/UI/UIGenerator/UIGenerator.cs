using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using UnityEditor;
using NaughtyAttributes;

public class UIGenerator : Singleton<UIGenerator>
{
    [SerializeField] private SerializableDictionary<string, List<EffectSO>> effectsPerType = new();
    [SerializeField] private GameObject typePrefab;

    public static SerializableDictionary<string, List<EffectSO>> EffectsPerType => Instance.effectsPerType;

#if UNITY_EDITOR
    private void OnValidate()
    {
        EffectSO[] allEffects = Resources.LoadAll<EffectSO>("Effects");
        foreach (EffectSO effect in allEffects)
        {
            string effectType = AssetDatabase.GetAssetPath(effect).Reverse().Split("/")[1].Split("/")[0].Reverse();

            if (!effectsPerType.TryGetValue(effectType, out List<EffectSO> effects))
                effects = new();

            if (!effects.Contains(effect))
                effects.Add(effect);

            effectsPerType[effectType] = effects;
        }
    }

    private GameObject InstantiatePrefab(GameObject prefab, Transform parent)
    {
        GameObject gb = PrefabUtility.InstantiatePrefab(prefab) as GameObject;
        gb.transform.SetParent(parent);
        return gb;
    }

    [Button]
    private void GenerateMissingUI()
    {
        List<TypeHandler> typeHandlersAlreadyCreated = GetComponentsInChildren<TypeHandler>().ToList();

        foreach (string type in effectsPerType.Keys)
        {
            TypeHandler typeHandler;
            if (typeHandlersAlreadyCreated.Any(x => x.Type == type))
                typeHandler = typeHandlersAlreadyCreated.First(x => x.Type == type);
            else
            {
                typeHandler = InstantiatePrefab(typePrefab, transform).GetComponent<TypeHandler>();
                typeHandler.name = type;
                typeHandler.SetType(type);
            }

            typeHandler.GenerateMissingEffects(effectsPerType[type]);
        }
    }
#endif
}
