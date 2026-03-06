using UnityEngine;

[CreateAssetMenu(fileName = "SoundOnBarHit_EffectSO", menuName = "EffectSO/Sound/SoundOnBarHit")]
public class SoundOnBarHit : EffectSO
{
    [SerializeField] private AudioClip audio;

    private void OnBarHit()
    {
        BallRefs.Instance.Audio.clip = audio;
        BallRefs.Instance.Audio.pitch = 1f;
        BallRefs.Instance.Audio.Play();
    }

    public override void OnEnabled()
        => Bar.OnHit += OnBarHit;

    public override void OnDisabled()
        => Bar.OnHit -= OnBarHit;
}
