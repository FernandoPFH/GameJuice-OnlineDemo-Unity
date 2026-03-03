using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DisappearEnd_EffectSO", menuName = "EffectSO/DisappearEnd")]
public class DisappearEnd : EffectSO
{
    [SerializeField] private Material material;
    [SerializeField] private float timeToDisappear = 2f;
    [SerializeField] private LeanTweenType easingMode = LeanTweenType.easeInOutExpo;

    [SerializeField] private float maxPosition = 1.5f;

    private bool hasAnimationStarted;
    private int numOfAnimationsToFinish;

#if UNITY_EDITOR
    protected override void InitValues()
    {
        hasAnimationStarted = false;
        numOfAnimationsToFinish = 0;
    }
#endif

    public void OnTimeToDisappearChanged(float time)
        => timeToDisappear = time;

    public void OnEasingModeChanged(LeanTweenType type)
        => easingMode = type;

    public void OnMaxPositionChanged(float pos)
        => maxPosition = pos;

    private void UpdatePostProcessing(float pos)
        => material.SetFloat("_PosicaoTransicao", pos);

    private void StartAnimation()
    {
        numOfAnimationsToFinish = 0;
        hasAnimationStarted = true;

        numOfAnimationsToFinish++;

        LeanTween.value(Background.Instance.gameObject, UpdatePostProcessing, 0f, maxPosition, timeToDisappear).setEase(easingMode).setOnComplete(() => { numOfAnimationsToFinish--; UpdatePostProcessing(maxPosition); });
    }

    private void CancelAnimation()
            => LeanTween.cancelAll(Background.Instance.gameObject);

    private bool HasDisappearEndAnimationFinished()
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
        if (state is not GameState.EndAnimation)
            return;

        StartAnimation();
    }

    private void OnGameReset()
    {
        CancelAnimation();
        hasAnimationStarted = false;
        UpdatePostProcessing(0f);
    }

    public override void OnEnabled()
    {
        GameStateManager.Instance.RegisterWait(GameState.EndAnimation, HasDisappearEndAnimationFinished);
        GameStateManager.OnGameStateChange += OnGameStateChange;
        GameStateManager.OnGameReset += OnGameReset;

        GameStateManager.Instance.ResetGame();
    }

    public override void OnDisabled()
    {
        GameStateManager.Instance.UnregisterWait(GameState.EndAnimation, HasDisappearEndAnimationFinished);
        GameStateManager.OnGameStateChange -= OnGameStateChange;
        GameStateManager.OnGameReset -= OnGameReset;

        CancelAnimation();
    }
}
