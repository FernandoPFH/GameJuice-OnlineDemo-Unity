using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DisappearEnd_EffectSO", menuName = "EffectSO/Finale/DisappearEnd")]
public class DisappearEnd : EffectSO
{
    [SerializeField] private Material material;
    [SerializeField] private string propertyName = "_PosicaoTransicao";
    [SerializeField] private float maxPosition = 1.5f;

    [SerializeField] private float timeToDisappear = 2f;
    [SerializeField] private LeanTweenType easingMode = LeanTweenType.easeInOutExpo;

    public float TimeToDisappear => timeToDisappear;
    public LeanTweenType EasingMode => easingMode;

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

    private void UpdatePostProcessing(float pos)
        => material.SetFloat(propertyName, pos);

    private void StartAnimation(float inicialValue, float finalValue)
    {
        numOfAnimationsToFinish = 0;
        hasAnimationStarted = true;

        numOfAnimationsToFinish++;

        LeanTween.value(Background.Instance.gameObject, UpdatePostProcessing, inicialValue, finalValue, timeToDisappear).setEase(easingMode).setOnComplete(() => { numOfAnimationsToFinish--; UpdatePostProcessing(finalValue); });
    }

    private void CancelAnimation()
            => LeanTween.cancel(Background.Instance.gameObject, true);

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

    private bool HasAppearStartAnimationFinished()
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
        switch (state)
        {
            case GameState.GameSpaceAnimation:
                StartAnimation(maxPosition, 0f);
                break;
            case GameState.EndAnimation:
                StartAnimation(0f, maxPosition);
                break;
            default:
                OnGameReset();
                break;
        }
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
        GameStateManager.Instance.RegisterWait(GameState.GameSpaceAnimation, HasAppearStartAnimationFinished);
        GameStateManager.OnGameStateChange += OnGameStateChange;
        GameStateManager.OnGameReset += OnGameReset;

        GameStateManager.Instance.ResetGame();
    }

    public override void OnDisabled()
    {
        GameStateManager.Instance.UnregisterWait(GameState.EndAnimation, HasDisappearEndAnimationFinished);
        GameStateManager.Instance.UnregisterWait(GameState.GameSpaceAnimation, HasAppearStartAnimationFinished);
        GameStateManager.OnGameStateChange -= OnGameStateChange;
        GameStateManager.OnGameReset -= OnGameReset;

        CancelAnimation();
    }
}
