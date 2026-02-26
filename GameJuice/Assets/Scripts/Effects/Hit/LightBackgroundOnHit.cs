using UnityEngine;

[CreateAssetMenu(fileName = "LightBackgroundOnHit_EffectSO", menuName = "EffectSO/LightBackgroundOnHit")]
public class LightBackgroundOnHit : EffectSO
{
    [SerializeField] private float colorMultiplier = 1.2f;
    [SerializeField] private float animationTime = 0.2f;
    [SerializeField] private LeanTweenType easingMode = LeanTweenType.easeInOutExpo;

    private void OnColorMultiplierChanged(float scale)
        => colorMultiplier = scale;

    private void OnAnimationTimeChanged(float time)
        => animationTime = time;

    private void OnEasingModeChanged(LeanTweenType LTT)
        => easingMode = LTT;

    private void UpdateBackgroundColor(Color color)
        => Background.Instance.SetColor(color);

    private void StartAnimation()
    {
        if (LeanTween.isTweening(Background.Instance.gameObject))
            LeanTween.cancelAll(Background.Instance.gameObject);

        LTSeq seq = LeanTween.sequence();

        Color baseColor = Background.Instance.GetColor();

        seq.append(LeanTween.value(Background.Instance.gameObject, UpdateBackgroundColor, baseColor, baseColor * colorMultiplier, animationTime / 2f));
        seq.append(LeanTween.value(Background.Instance.gameObject, UpdateBackgroundColor, baseColor * colorMultiplier, baseColor, animationTime / 2f).setOnComplete(() => { UpdateBackgroundColor(baseColor); }));
    }

    private void CancelAnimation()
    {
        LeanTween.cancelAll(Background.Instance.transform);
    }

    private void OnBlockHit(GameObject block, int count, Vector3 ballVelocity)
        => StartAnimation();

    public override void OnEnabled()
        => Block.OnHit += OnBlockHit;

    public override void OnDisabled()
    {
        Block.OnHit -= OnBlockHit;

        CancelAnimation();
    }
}
