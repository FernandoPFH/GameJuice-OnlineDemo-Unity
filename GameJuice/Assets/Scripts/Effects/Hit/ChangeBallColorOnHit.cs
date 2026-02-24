using UnityEngine;

[CreateAssetMenu(fileName = "ChangeBallColorOnHit_EffectSO", menuName = "EffectSO/ChangeBallColorOnHit")]
public class ChangeBallColorOnHit : EffectSO
{
    [SerializeField] private Color endColor;
    [SerializeField] private float animationTime = 0.2f;
    [SerializeField] private LeanTweenType easingMode = LeanTweenType.easeInOutExpo;

#if UNITY_EDITOR
    private Color lastEndColor;
    private float lastAnimationTime;
    private LeanTweenType lastEasingMode;

    protected override void InitValues()
    {
        InitValue(ref lastEndColor, endColor);
        InitValue(ref lastAnimationTime, animationTime);
        InitValue(ref lastEasingMode, easingMode);
    }

    protected override void CheckValuesChanged()
    {
        CheckValueChanged(ref lastEndColor, endColor, OnEndColorChanged);
        CheckValueChanged(ref lastAnimationTime, animationTime, OnAnimationTimeChanged);
        CheckValueChanged(ref lastEasingMode, easingMode, OnEasingModeChanged);
    }
#endif

    private void OnEndColorChanged(Color color)
        => endColor = color;

    private void OnAnimationTimeChanged(float time)
        => animationTime = time;

    private void OnEasingModeChanged(LeanTweenType LTT)
        => easingMode = LTT;

    private void UpdateBallColor(Color color)
        => BallRefs.Instance.Renderer.color = color;

    private void StartAnimation()
    {
        if (LeanTween.isTweening(BallRefs.Instance.Renderer.gameObject))
            CancelAnimation();

        LTSeq seq = LeanTween.sequence();

        Color initialColor = BallRefs.Instance.Renderer.color;

        seq.append(LeanTween.value(BallRefs.Instance.Renderer.gameObject, UpdateBallColor, initialColor, endColor, animationTime / 2f));
        seq.append(LeanTween.value(BallRefs.Instance.Renderer.gameObject, UpdateBallColor, endColor, initialColor, animationTime / 2f).setOnComplete(() => BallRefs.Instance.Renderer.color = initialColor));
    }

    private void CancelAnimation()
        => LeanTween.cancelAll(BallRefs.Instance.Renderer.gameObject);

    private void OnBlockHit(GameObject block, int count)
        => StartAnimation();

    public override void OnEnabled()
        => Block.OnHit += OnBlockHit;

    public override void OnDisabled()
    {
        Block.OnHit -= OnBlockHit;

        CancelAnimation();
    }
}
