using UnityEngine;
using System.Collections.Generic;
using Unity.Cinemachine;

[CreateAssetMenu(fileName = "ShakeScreen_EffectSO", menuName = "EffectSO/Camera/ShakeScreen")]
public class ShakeScreen : EffectSO
{
    [SerializeField] private float baseAmplitude = 1f;
    [SerializeField] private float blockMultiplier = 1.5f;
    [SerializeField] private float animationTime = 0.5f;
    [SerializeField] private LeanTweenType easingMode = LeanTweenType.easeInOutExpo;

    public float BaseAmplitude => baseAmplitude;
    public float BlockMultiplier => blockMultiplier;
    public float AnimationTime => animationTime;
    public LeanTweenType EasingMode => easingMode;

    public void OnBaseAmplitudeChanged(float amp)
        => baseAmplitude = amp;

    public void OnBlockMultiplierChanged(float mult)
        => blockMultiplier = mult;

    public void OnAnimationTimeChanged(float time)
        => animationTime = time;

    public void OnEasingModeChanged(LeanTweenType LTT)
        => easingMode = LTT;

    private float currentAmplitude;

    private void UpdateShakeFrequency(float value)
        => CameraRefs.Cameras["BaseCamera"].GetComponent<CinemachineBasicMultiChannelPerlin>().FrequencyGain = value;

    private void StartAnimation(float amplitude)
    {
        CinemachineBasicMultiChannelPerlin noise =  CameraRefs.Cameras["BaseCamera"].GetComponent<CinemachineBasicMultiChannelPerlin>();
        noise.AmplitudeGain = amplitude;
        LeanTween.value(CameraRefs.Cameras["BaseCamera"].gameObject,UpdateShakeFrequency,1f,0f,animationTime).setEase(easingMode)
            .setOnComplete(_ =>
            {
                noise.AmplitudeGain = 0f;
                noise.FrequencyGain = 0f;
            });
    }

    private void CancelAnimation()
        => LeanTween.cancel(CameraRefs.Cameras["BaseCamera"].gameObject, true);

    private void OnBallHit(string otherTag, Vector2 point, Vector2 normal)
    {
        if (otherTag == "Block")
            currentAmplitude *= blockMultiplier;
        else
            currentAmplitude = baseAmplitude;

        StartAnimation(currentAmplitude);
    }

    public override void OnEnabled()
    { 
        currentAmplitude = baseAmplitude;
        Ball.OnHit += OnBallHit;
    }

    public override void OnDisabled()
    {
        Ball.OnHit -= OnBallHit;
        CancelAnimation();
    }
}
