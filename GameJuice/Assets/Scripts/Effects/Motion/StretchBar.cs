using UnityEngine;

[CreateAssetMenu(fileName = "StrechBar_EffectSO", menuName = "EffectSO/StrechBar")]
public class StretchBar : EffectSO
{
    [SerializeField] private float maxStretch = 2f;
    [SerializeField] private float maxShort = 0.9f;
    [SerializeField] private float maxVelocityRef = 2f;

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!isEnabled)
            return;

        float barVelocity = Mathf.Abs(BarRefs.Instance.Rigidbody.linearVelocityX);

        float stretch = Map(barVelocity, 0f, maxVelocityRef, 1f, maxStretch);
        float shorten = Map(barVelocity, 0f, maxVelocityRef, 1f, maxShort);

        Vector3 newScale = BarRefs.Instance.Renderer.transform.localScale;
        newScale.x = Mathf.Clamp(stretch, 1f, maxStretch);
        newScale.y = Mathf.Clamp(shorten, maxShort, 1f);

        BarRefs.Instance.Renderer.transform.localScale = newScale;
    }

    public override void OnDisabled()
        => BarRefs.Instance.Renderer.transform.localScale = Vector3.one;

    public static float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
}
