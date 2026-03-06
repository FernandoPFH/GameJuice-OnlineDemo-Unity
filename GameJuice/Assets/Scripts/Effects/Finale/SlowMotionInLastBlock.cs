using UnityEngine;
using Unity.Cinemachine;

[CreateAssetMenu(fileName = "SlowMotionInLastBlock_EffectSO", menuName = "EffectSO/Finale/SlowMotionInLastBlock")]
public class SlowMotionInLastBlock : EffectSO
{
    [SerializeField] private AnimationCurve slowDownOverTime;

    private float timeElapsed = float.MinValue;

#if UNITY_EDITOR
    protected override void InitValues()
        => timeElapsed = float.MinValue;
#endif

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!isEnabled)
            return;

        float framesSinceStart = Time.time - timeElapsed;

        if (framesSinceStart < 0 || framesSinceStart > slowDownOverTime.keys[slowDownOverTime.length - 1].time)
        {
            Time.timeScale = 1f;
            return;
        }

        Time.timeScale = slowDownOverTime.Evaluate(framesSinceStart);

    }

    private bool IsTimeToZoom()
    {
        if (!Ball.Instance.BallTrajectoryPredic.NextContacts.TryPeek(out NextContact nContact))
            return false;

        if (Block.Count == 1 && nContact.type == "Block")
            return true;

        return false;
    }

    private void OnBallHit(string otherTag, Vector2 point, Vector2 normal)
    {
        if (Time.time - timeElapsed < 0 && IsTimeToZoom())
            timeElapsed = Time.time;
    }

    public override void OnEnabled()
    {
        Ball.OnHit += OnBallHit;

        if (IsTimeToZoom())
            timeElapsed = Time.time;
    }

    public override void OnDisabled()
    {
        Ball.OnHit -= OnBallHit;
        Time.timeScale = 1f;
    }
}
