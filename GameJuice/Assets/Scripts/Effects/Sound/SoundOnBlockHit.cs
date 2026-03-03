using UnityEngine;

[CreateAssetMenu(fileName = "SoundOnBlockHit_EffectSO", menuName = "EffectSO/SoundOnBlockHit")]
public class SoundOnBlockHit : EffectSO
{
    [SerializeField] private AudioClip audio;
    [SerializeField] private float minPitch = 1f;
    [SerializeField] private float maxPitch = 2f;
    [SerializeField] private int maxCombo = 10;
    [SerializeField] private float timeToResetCombo = 1f;

    private int currentCombo = 0;
    private float timeSinceLastHit = float.MinValue;

#if UNITY_EDITOR
    protected override void InitValues()
        => currentCombo = 0;
#endif

    public void OnMinPitchChanged(float pitch)
        => minPitch = pitch;

    public void OnMaxPitchChanged(float pitch)
        => maxPitch = pitch;

    public void OnMaxComboChanged(int combo)
        => maxCombo = combo;

    public void OnTimeToResetCombo(float time)
        => timeToResetCombo = time;

    private void OnBlockHit(GameObject block, int count, Vector3 ballVelocity)
    {
        if (Time.time - timeSinceLastHit <= timeToResetCombo)
            currentCombo++;
        else
            currentCombo = 0;

        timeSinceLastHit = Time.time;

        float step = (maxPitch - minPitch) / maxCombo;

        BallRefs.Instance.Audio.clip = audio;
        BallRefs.Instance.Audio.pitch = minPitch + step * currentCombo;
        BallRefs.Instance.Audio.Play();
    }

    public override void OnEnabled()
        => Block.OnHit += OnBlockHit;

    public override void OnDisabled()
        => Block.OnHit -= OnBlockHit;
}
