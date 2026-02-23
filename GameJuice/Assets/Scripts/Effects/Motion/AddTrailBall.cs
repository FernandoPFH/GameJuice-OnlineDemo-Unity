using UnityEngine;

[CreateAssetMenu(fileName = "AddBallTrail_EffectSO", menuName = "EffectSO/AddBallTrail")]
public class AddTrailBall : EffectSO
{
    private void SetBallTrail()
    {
        BallRefs.Instance.Trail.enabled = true;

        BallRefs.Instance.Trail.startColor = BallRefs.Instance.Renderer.color;
        BallRefs.Instance.Trail.endColor = BallRefs.Instance.Renderer.color;
    }

    private void ResetBallTrail()
        => BallRefs.Instance.Trail.enabled = false;

    private void OnBallSpawn()
        => BallRefs.Instance.Trail.Clear();

    public override void OnEnabled()
    {
        Ball.OnSpawn += OnBallSpawn;

        SetBallTrail();
    }

    public override void OnDisabled()
    {
        Ball.OnSpawn -= OnBallSpawn;

        ResetBallTrail();
    }
}
