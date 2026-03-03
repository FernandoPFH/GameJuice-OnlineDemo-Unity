using UnityEngine;

[CreateAssetMenu(fileName = "SoundOnBackground_EffectSO", menuName = "EffectSO/SoundOnBackground")]
public class SoundOnBackground : EffectSO
{
    [SerializeField] private float fadeInTime = 2f;
    [SerializeField] private LeanTweenType easingMode = LeanTweenType.easeInOutExpo;

    private void UpdateVolume(float volume)
        => Background.Instance.AudioSource.volume = volume;

    private void StartFadeIn()
    {
        float finalVolume = Background.Instance.AudioSource.volume;
        LeanTween.value(Background.Instance.gameObject, UpdateVolume, 0f, finalVolume, fadeInTime).setEase(easingMode).setOnComplete(() => UpdateVolume(finalVolume));
        Background.Instance.AudioSource.Play();
    }

    private void CancelFadeIn()
    {
        LeanTween.cancelAll(Background.Instance.gameObject);
        Background.Instance.AudioSource.Stop();
    }

    public override void OnEnabled()
        => StartFadeIn();

    public override void OnDisabled()
        => CancelFadeIn();
}
