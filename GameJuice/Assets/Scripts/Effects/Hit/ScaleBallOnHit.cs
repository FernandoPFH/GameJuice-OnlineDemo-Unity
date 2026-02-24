using UnityEngine;

[CreateAssetMenu(fileName = "ScaleBallOnHit_EffectSO", menuName = "EffectSO/ScaleBallOnHit")]
public class ScaleBallOnHit : EffectSO
{
    [SerializeField] private float scaleMultiplier = 1.2f;
    [SerializeField] private float animationTime = 0.2f;
    [SerializeField] private LeanTweenType easingMode = LeanTweenType.easeInOutExpo;

#if UNITY_EDITOR
    private float lastScaleMultiplier;
    private float lastAnimationTime;
    private LeanTweenType lastEasingMode;

    protected override void InitValues()
    {
        InitValue(ref lastScaleMultiplier, scaleMultiplier);
        InitValue(ref lastAnimationTime, animationTime);
        InitValue(ref lastEasingMode, easingMode);
    }

    protected override void CheckValuesChanged()
    {
        CheckValueChanged(ref lastScaleMultiplier, scaleMultiplier, OnScaleMultiplierChanged);
        CheckValueChanged(ref lastAnimationTime, animationTime, OnAnimationTimeChanged);
        CheckValueChanged(ref lastEasingMode, easingMode, OnEasingModeChanged);
    }
#endif

    private void OnScaleMultiplierChanged(float scale)
        => scaleMultiplier = scale;

    private void OnAnimationTimeChanged(float time)
        => animationTime = time;

    private void OnEasingModeChanged(LeanTweenType LTT)
        => easingMode = LTT;

    private void StartAnimation()
    {
        LTSeq seq = LeanTween.sequence();
        seq.append(BallRefs.Instance.Renderer.transform.LeanScale(Vector3.one * scaleMultiplier, animationTime / 2f));
        seq.append(BallRefs.Instance.Renderer.transform.LeanScale(Vector3.one, animationTime / 2f));
    }

    private void CancelAnimation()
    {
        LeanTween.cancelAll(BallRefs.Instance.Renderer.transform);
        BallRefs.Instance.Renderer.transform.localScale = Vector3.one;
    }

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
