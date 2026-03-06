using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpawnTrailBallFast_EffectSO", menuName = "EffectSO/Particle/SpawnTrailBallFast")]
public class SpawnTrailBallFast : EffectSO
{
    [SerializeField] private float ballVelocityThreshold = 10f;

    public float BallVelocityThreshold => ballVelocityThreshold;

    public void OnBallVelocityThresholdChanged(float threshould)
        => ballVelocityThreshold = threshould;

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!isEnabled)
            return;

        if (Ball.Velocity.sqrMagnitude < ballVelocityThreshold * ballVelocityThreshold)
        {
            BallRefs.Instance.FireTrail.SetActive(false);
            return;
        }

        float angle = -Vector2.SignedAngle(Ball.Velocity, -Vector3.up);

        Vector3 lerpAngle = Vector3.forward * Mathf.LerpAngle(BallRefs.Instance.FireTrail.transform.rotation.eulerAngles.z, angle, 0.5f);

        BallRefs.Instance.FireTrail.SetActive(true);
        BallRefs.Instance.FireTrail.transform.rotation = Quaternion.Euler(lerpAngle);
    }

    public override void OnDisabled()
        => BallRefs.Instance.FireTrail.SetActive(false);
}
