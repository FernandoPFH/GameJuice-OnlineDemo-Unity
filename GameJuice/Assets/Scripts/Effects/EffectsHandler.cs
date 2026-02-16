using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class EffectsHandler : MonoBehaviour
{
    [SerializeField] private List<EffectSO> effects;

    private void Start()
    {
        foreach (EffectSO effect in effects)
            effect.OnStart();
    }

    private void Update()
    {
        foreach (EffectSO effect in effects)
            effect.OnUpdate();
    }

#if UNITY_EDITOR
    private void OnValidate()
        => effects = Resources.LoadAll<EffectSO>("Effects").ToList();
#endif
}
