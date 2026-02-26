using UnityEngine;

[CreateAssetMenu(fileName = "SoundOnDeathWallHit_EffectSO", menuName = "EffectSO/SoundOnDeathWallHit")]
public class SoundOnDeathWallHit : EffectSO
{
    [SerializeField] private AudioClip audio;

    private void OnDeathWallHit(GameObject deathWall, Vector3 ballPosition)
    {
        BallRefs.Instance.Audio.clip = audio;
        BallRefs.Instance.Audio.pitch = 1f;
        BallRefs.Instance.Audio.Play();
    }

    public override void OnEnabled()
        => DeathWall.OnHit += OnDeathWallHit;

    public override void OnDisabled()
        => DeathWall.OnHit -= OnDeathWallHit;
}
