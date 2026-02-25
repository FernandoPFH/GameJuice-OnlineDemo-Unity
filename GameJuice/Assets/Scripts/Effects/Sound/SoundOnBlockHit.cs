using UnityEngine;

[CreateAssetMenu(fileName = "SoundOnBlockHit_EffectSO", menuName = "EffectSO/SoundOnBlockHit")]
public class SoundOnBlockHit : EffectSO
{
    [SerializeField] private AudioClip audio;
    [SerializeField] private float minPitch = 1f;
    [SerializeField] private float maxPitch = 3f;

    private void OnBlockHit(GameObject block, int count, Vector3 ballVelocity)
    {
        BallRefs.Instance.Audio.clip = audio;
        BallRefs.Instance.Audio.pitch = Map(count, Block.TotalCount, 0f, minPitch, maxPitch);
        BallRefs.Instance.Audio.Play();
    }

    public override void OnEnabled()
        => Block.OnHit += OnBlockHit;

    public override void OnDisabled()
        => Block.OnHit -= OnBlockHit;

    public static float Map(float value, float fromSource, float toSource, float fromTarget, float toTarget)
    {
        return (value - fromSource) / (toSource - fromSource) * (toTarget - fromTarget) + fromTarget;
    }
}
