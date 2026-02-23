using UnityEngine;

[CreateAssetMenu(fileName = "StrechBall_EffectSO", menuName = "EffectSO/StrechBall")]
public class StretchBall : EffectSO
{
    [SerializeField] private float maxStretch = 2f;
    [SerializeField] private float maxShort = 0.9f;
    [SerializeField] private float maxVelocityRef = 2f;

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!isEnabled)
            return;

        float angle = -Vector3.SignedAngle(BallRefs.Instance.Rigidbody.linearVelocity.normalized, Vector3.right, Vector3.forward);

        float ballVelocity = Mathf.Abs(BallRefs.Instance.Rigidbody.linearVelocity.magnitude);

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
