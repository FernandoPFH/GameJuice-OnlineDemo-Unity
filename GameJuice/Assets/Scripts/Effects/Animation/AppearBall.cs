using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AppearBall_EffectSO", menuName = "EffectSO/AppearBall")]
public class AppearBall : EffectSO
{
    [SerializeField] private float timeToAppear = 2f;
    [SerializeField] private LeanTweenType easingMode = LeanTweenType.easeInOutExpo;

    [SerializeField] private Vector3 scaleMultiplier;

    [SerializeField] private bool shouldHaveDelay;
    [SerializeField] private float minDelay = 0f;
    [SerializeField] private float maxDelay = 0.2f;

    private bool hasAnimationStarted;
    private int numOfAnimationsToFinish;

    private float Delay => !shouldHaveDelay ? 0f : Random.Range(minDelay, maxDelay);

#if UNITY_EDITOR
    private float lastTimeToAppear;
    private LeanTweenType lastEasingMode;

    private Vector3 lastScaleMultiplier;

    private bool lastShouldHaveDelay;
    private float lastMinDelay;
    private float lastMaxDelay;

    protected override void InitValues()
    {
        InitValue(ref lastTimeToAppear, timeToAppear);
        InitValue(ref lastEasingMode, easingMode);
        InitValue(ref lastScaleMultiplier, scaleMultiplier);
        InitValue(ref lastShouldHaveDelay, shouldHaveDelay);
        InitValue(ref lastMinDelay, minDelay);
        InitValue(ref lastMaxDelay, maxDelay);

        hasAnimationStarted = false;
        numOfAnimationsToFinish = 0;
    }

    protected override void CheckValuesChanged()
    {
        CheckValueChanged(ref lastTimeToAppear, timeToAppear, OnTimeToAppearChanged);
        CheckValueChanged(ref lastEasingMode, easingMode, OnEasingModeChanged);
        CheckValueChanged(ref lastScaleMultiplier, scaleMultiplier, OnScaleOffsetChanged);
        CheckValueChanged(ref lastShouldHaveDelay, shouldHaveDelay, OnShouldHaveDelayChanged);
        CheckValueChanged(ref lastMinDelay, minDelay, OnMinDelayChanged);
        CheckValueChanged(ref lastMaxDelay, maxDelay, OnMaxDelayChanged);
    }
#endif

    public void OnTimeToAppearChanged(float time)
    {
        timeToAppear = time;
        if (isEnabled)
            GameStateManager.Instance.ResetGame();
    }

    public void OnEasingModeChanged(LeanTweenType type)
    {
        easingMode = type;
        if (isEnabled)
            GameStateManager.Instance.ResetGame();
    }

    public void OnScaleOffsetChanged(Vector3 scale)
    {
        scaleMultiplier = scale;
        if (isEnabled)
            GameStateManager.Instance.ResetGame();
    }

    public void OnShouldHaveDelayChanged(bool should)
    {
        shouldHaveDelay = should;
        if (isEnabled)
            GameStateManager.Instance.ResetGame();
    }

    public void OnMinDelayChanged(float delay)
    {
        minDelay = delay;
        if (isEnabled)
            GameStateManager.Instance.ResetGame();
    }

    public void OnMaxDelayChanged(float delay)
    {
        maxDelay = delay;
        if (isEnabled)
            GameStateManager.Instance.ResetGame();
    }

    private void StartAnimation()
    {
        numOfAnimationsToFinish = 0;
        hasAnimationStarted = true;

        numOfAnimationsToFinish++;

        Transform obj = Ball.Instance.transform;

        Vector3 finalScale = obj.localScale;
        obj.localScale = obj.localScale.Multiply(scaleMultiplier);
        obj.LeanScale(finalScale, timeToAppear).setDelay(Delay).setEase(easingMode).setOnComplete(() => { numOfAnimationsToFinish--; obj.localScale = finalScale; });
    }

    private void CancelAnimation()
            => LeanTween.cancelAll(Ball.Instance.transform);

    private bool HasAppearBallAnimationFinished()
    {
        if (!isEnabled)
            return true;

        if (!hasAnimationStarted)
            return false;

        if (numOfAnimationsToFinish > 0)
            return false;

        return true;
    }

    private void OnGameStateChange(GameState state)
    {
        if (state is not GameState.BallAnimation)
            return;

        StartAnimation();
    }

    private void OnGameReset()
    {
        CancelAnimation();
        hasAnimationStarted = false;
    }

    public override void OnEnabled()
    {
        GameStateManager.Instance.RegisterWait(GameState.BallAnimation, HasAppearBallAnimationFinished);
        GameStateManager.OnGameStateChange += OnGameStateChange;
        GameStateManager.OnGameReset += OnGameReset;

        GameStateManager.Instance.ResetGame();
    }

    public override void OnDisabled()
    {
        GameStateManager.Instance.UnregisterWait(GameState.BallAnimation, HasAppearBallAnimationFinished);
        GameStateManager.OnGameStateChange -= OnGameStateChange;

        CancelAnimation();
    }
}
