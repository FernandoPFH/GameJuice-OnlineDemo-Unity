using UnityEngine;

[CreateAssetMenu(fileName = "ChangeBallColorOnHit_EffectSO", menuName = "EffectSO/ChangeBallColorOnHit")]
public class ChangeBallColorOnHit : EffectSO
{
    [SerializeField] private Color endColor;
    [SerializeField] private float animationTime = 0.2f;
    [SerializeField] private LeanTweenType easingMode = LeanTweenType.easeInOutExpo;

    public void OnEndColorChanged(Color color)
        => endColor = color;

    public void OnAnimationTimeChanged(float time)
        => animationTime = time;

    public void OnEasingModeChanged(LeanTweenType LTT)
        => easingMode = LTT;

    private void UpdateBallColor(Color color)
        => BallRefs.Instance.Renderer.color = color;

    private void StartAnimation()
    {
        if (LeanTween.isTweening(BallRefs.Instance.Renderer.gameObject))
            CancelAnimation();

        LTSeq seq = LeanTween.sequence();

        Color initialColor = BallRefs.Instance.Renderer.color;

        seq.append(LeanTween.value(BallRefs.Instance.Renderer.gameObject, UpdateBallColor, initialColor, endColor, animationTime / 2f).setEase(easingMode));
        seq.append(LeanTween.value(BallRefs.Instance.Renderer.gameObject, UpdateBallColor, endColor, initialColor, animationTime / 2f).setEase(easingMode).setOnComplete(() => BallRefs.Instance.Renderer.color = initialColor));
    }

    private void CancelAnimation()
        => LeanTween.cancelAll(BallRefs.Instance.Renderer.gameObject);

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
