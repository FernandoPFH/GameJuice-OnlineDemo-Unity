using UnityEngine;
using Unity.Cinemachine;

[CreateAssetMenu(fileName = "SlowMotionInLastBlock_EffectSO", menuName = "EffectSO/Finale/SlowMotionInLastBlock")]
public class SlowMotionInLastBlock : EffectSO
{
    [SerializeField] private AnimationCurve slowDownOverTime;

    private float frameWhenStarted = float.MinValue;

#if UNITY_EDITOR
    protected override void InitValues()
        => frameWhenStarted = float.MinValue;
#endif

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!isEnabled)
            return;       
            
        if (frameWhenStarted < 0)
        {
            Time.timeScale = 1f;
            return;
        }

        Time.timeScale = slowDownOverTime.Evaluate(Time.time - frameWhenStarted);
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
        if (Block.Count == 0)
        {
            Time.timeScale = 1f;
            frameWhenStarted = float.MinValue;
            return;
        }

        if (frameWhenStarted - Time.time  < 0 && IsTimeToZoom())
            frameWhenStarted = Time.time;
    }

    public override void OnEnabled()
    {
        Ball.OnHit += OnBallHit;

        if (IsTimeToZoom())
            frameWhenStarted = Time.time;
    }

    public override void OnDisabled()
    {
        Ball.OnHit -= OnBallHit;
        Time.timeScale = 1f;
    }
}
