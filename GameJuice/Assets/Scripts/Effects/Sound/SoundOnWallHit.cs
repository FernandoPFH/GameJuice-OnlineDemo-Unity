using UnityEngine;

[CreateAssetMenu(fileName = "SoundOnWallHit_EffectSO", menuName = "EffectSO/SoundOnWallHit")]
public class SoundOnWallHit : EffectSO
{
    [SerializeField] private AudioClip audio;

    private void OnWallHit(GameObject wall)
    {
        BallRefs.Instance.Audio.clip = audio;
        BallRefs.Instance.Audio.pitch = 1f;
        BallRefs.Instance.Audio.Play();
    }

    public override void OnEnabled()
        => Wall.OnHit += OnWallHit;

    public override void OnDisabled()
        => Wall.OnHit -= OnWallHit;
}
