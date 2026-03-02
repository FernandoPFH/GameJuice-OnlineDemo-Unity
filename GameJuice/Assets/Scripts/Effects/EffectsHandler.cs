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

    private void OnDestroy()
    {
        foreach (EffectSO effect in effects)
            effect.OnDestroy();
    }

#if UNITY_EDITOR
    private void OnValidate()
        => effects = Resources.LoadAll<EffectSO>("Effects").ToList();

    private void OnDrawGizmosSelected()
    {
        foreach (EffectSO effect in effects)
            effect.OnGizmosDraw();
    }
#endif
}
