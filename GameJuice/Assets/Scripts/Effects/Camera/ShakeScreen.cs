using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "ShakeScreen_EffectSO", menuName = "EffectSO/ShakeScreen")]
public class ShakeScreen : EffectSO
{
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private float blockMultiplier = 1.5f;
    [SerializeField] private float animationTime = 0.5f;
    [SerializeField] private LeanTweenType easingMode = LeanTweenType.easeInOutExpo;

    public void OnPositionOffsetChanged(Vector3 pos)
        => positionOffset = pos;

    public void OnBlockMultiplierChanged(float mult)
        => blockMultiplier = mult;

    public void OnAnimationTimeChanged(float time)
        => animationTime = time;

    public void OnEasingModeChanged(LeanTweenType LTT)
        => easingMode = LTT;

    private void StartAnimation(Vector3 pos)
    {
        LTSeq seq = LeanTween.sequence();
        seq.append(Camera.main.transform.LeanMoveLocal(pos, animationTime / 4f).setEase(easingMode));
        seq.append(Camera.main.transform.LeanMoveLocal(-pos, animationTime / 2f).setEase(easingMode));
        seq.append(Camera.main.transform.LeanMoveLocal(Vector3.zero, animationTime / 4f).setEase(easingMode).setOnComplete(() => { Camera.main.transform.localPosition = Vector3.zero; }));
    }

    private void CancelAnimation()
        => LeanTween.cancelAll(Camera.main.transform);

    private void OnBallHit(string otherTag, Vector2 point, Vector2 normal)
    {
        Vector3 pos = positionOffset;

        if (otherTag == "Block")
            pos *= blockMultiplier;

        StartAnimation(pos);
    }

    public override void OnEnabled()
        => Ball.OnHit += OnBallHit;

    public override void OnDisabled()
    {
        Ball.OnHit -= OnBallHit;
        CancelAnimation();
    }
}
