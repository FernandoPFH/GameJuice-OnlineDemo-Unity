using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SpawnTrailBallFast_EffectSO", menuName = "EffectSO/SpawnTrailBallFast")]
public class SpawnTrailBallFast : EffectSO
{
    [SerializeField] private float ballVelocityThreshold = 10f;

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!isEnabled)
            return;

        if (BallRefs.Instance.Rigidbody.linearVelocity.magnitude < ballVelocityThreshold)
        {
            BallRefs.Instance.FireTrail.SetActive(false);
            return;
        }

        Vector2 linearVelocity = BallRefs.Instance.Rigidbody.linearVelocity;
        linearVelocity.x *= -1f;

        float angle = Vector2.SignedAngle(linearVelocity, -Vector3.up);

        Vector3 lerpAngle = Vector3.forward * Mathf.LerpAngle(BallRefs.Instance.FireTrail.transform.rotation.eulerAngles.z, angle, 0.5f);

        BallRefs.Instance.FireTrail.SetActive(true);
        BallRefs.Instance.FireTrail.transform.rotation = Quaternion.Euler(lerpAngle);
    }

    public override void OnDisabled()
        => BallRefs.Instance.FireTrail.SetActive(false);
}
