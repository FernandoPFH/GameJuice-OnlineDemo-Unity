using UnityEngine;

[CreateAssetMenu(fileName = "ScaleBallOnHit_EffectSO", menuName = "EffectSO/Hit/ScaleBallOnHit")]
public class ScaleBallOnHit : EffectSO
{
    [SerializeField] private float scaleMultiplier = 1.2f;
    [SerializeField] private float animationTime = 0.2f;
    [SerializeField] private LeanTweenType easingMode = LeanTweenType.easeInOutExpo;

    public float ScaleMultiplier => scaleMultiplier;
    public float AnimationTime => animationTime;
    public LeanTweenType EasingMode => easingMode;

    private void OnScaleMultiplierChanged(float scale)
        => scaleMultiplier = scale;

    private void OnAnimationTimeChanged(float time)
        => animationTime = time;

    private void OnEasingModeChanged(LeanTweenType LTT)
        => easingMode = LTT;

    private void StartAnimation()
    {
        LTSeq seq = LeanTween.sequence();
        seq.append(BallRefs.Instance.Renderer.transform.LeanScale(Vector3.one * scaleMultiplier, animationTime / 2f).setEase(easingMode));
        seq.append(BallRefs.Instance.Renderer.transform.LeanScale(Vector3.one, animationTime / 2f).setEase(easingMode));
    }

    private void CancelAnimation()
    {
        LeanTween.cancel(BallRefs.Instance.Renderer.gameObject,true);
        BallRefs.Instance.Renderer.transform.localScale = Vector3.one;
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
