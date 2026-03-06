using UnityEngine;

[CreateAssetMenu(fileName = "StrechBall_EffectSO", menuName = "EffectSO/Motion/StrechBall")]
public class StretchBall : EffectSO
{
    [SerializeField] private float maxStretch = 2f;
    [SerializeField] private float maxShort = 0.9f;
    [SerializeField] private float maxVelocityRef = 2f;

    public float MaxStretch => maxStretch;
    public float MaxShort => maxShort;
    public float MaxVelocityRef => maxVelocityRef;

    public void OnMaxStretchChanged(float stretch)
        => maxStretch = stretch;

    public void OnMaxShortChanged(float mShort)
        => maxShort = mShort;

    public void OnMaxVelocityRefChanged(float velocity)
        => maxVelocityRef = velocity;

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!isEnabled)
            return;

        float angle = -Vector2.SignedAngle(Ball.Velocity.normalized, Vector2.right);

        float ballVelocity = Mathf.Abs(Ball.Velocity.magnitude);

        float stretch = Map(ballVelocity, 0f, maxVelocityRef, 1f, maxStretch);
        float shorten = Map(ballVelocity, 0f, maxVelocityRef, 1f, maxShort);

        BallRefs.Instance.Renderer.transform.localRotation = Quaternion.Euler(Vector3.forward * angle);

        Vector3 newScale = BallRefs.Instance.Renderer.transform.localScale;
        newScale.x = Mathf.Clamp(stretch, 1f, maxStretch);
        newScale.y = Mathf.Clamp(shorten, maxShort, 1f);

        BallRefs.Instance.Renderer.transform.localScale = newScale;
    }

    public override void OnDisabled()
    {
        BallRefs.Instance.Renderer.transform.localScale = Vector3.one;
        BallRefs.Instance.Renderer.transform.localRotation = Quaternion.Euler(Vector3.zero);
    }

    public static float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
}
