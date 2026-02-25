using UnityEngine;
using System.Linq;

[CreateAssetMenu(fileName = "TriggerHitStop_EffectSO", menuName = "EffectSO/TriggerHitStop")]
public class TriggerHitStop : EffectSO
{
    [SerializeField] private AnimationCurve timeScaleOverTime;

    private int frameLastHit = int.MinValue;

#if UNITY_EDITOR
    protected override void InitValues()
        => frameLastHit = int.MinValue;
#endif

    public override void OnUpdate()
    {
        base.OnUpdate();

        if (!isEnabled)
            return;

        int framesSinceHit = Time.frameCount - frameLastHit;

        if (framesSinceHit < 0 || framesSinceHit > timeScaleOverTime.keys[timeScaleOverTime.length - 1].time)
        {
            Time.timeScale = 1f;
            return;
        }

        float scale = timeScaleOverTime.Evaluate(framesSinceHit);

        Time.timeScale = scale;
    }

    private void OnBlockHit(GameObject block, int count, Vector3 ballVelocity)
        => frameLastHit = Time.frameCount;

    public override void OnEnabled()
        => Block.OnHit += OnBlockHit;

    public override void OnDisabled()
    {
        Block.OnHit -= OnBlockHit;
        Time.timeScale = 1f;
    }
}
