using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AppearGameSpace_EffectSO", menuName = "EffectSO/Animation/AppearGameSpace")]
public class AppearGameSpace : EffectSO
{
    [SerializeField] private float timeToAppear = 2f;
    [SerializeField] private LeanTweenType easingMode = LeanTweenType.easeInOutExpo;

    [SerializeField] private bool affectPosition;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private bool affectRotation;
    [SerializeField] private Vector3 rotationOffset;
    [SerializeField] private bool affectScale;
    [SerializeField] private Vector3 scaleMultiplier;

    [SerializeField] private bool shouldHaveDelay;
    [SerializeField] private float minDelay = 0f;
    [SerializeField] private float maxDelay = 0.2f;

    public float TimeToAppear => timeToAppear;
    public LeanTweenType EasingMode => easingMode;

    public bool AffectPosition => affectPosition;
    public Vector3 PositionOffset => positionOffset;
    public bool AffectRotation => affectRotation;
    public Vector3 RotationOffset => rotationOffset;
    public bool AffectScale => affectScale;
    public Vector3 ScaleMultiplier => scaleMultiplier;

    public bool ShouldHaveDelay => shouldHaveDelay;
    public float MinDelay => minDelay;
    public float MaxDelay => maxDelay;

    private bool hasAnimationStarted;
    private int numOfAnimationsToFinish;

    private float Delay => !shouldHaveDelay ? 0f : Random.Range(minDelay, maxDelay);

#if UNITY_EDITOR
    private float lastTimeToAppear;
    private LeanTweenType lastEasingMode;

    private bool lastAffectPosition;
    private Vector3 lastPositionOffset;
    private bool lastAffectRotation;
    private Vector3 lastRotationOffset;
    private bool lastAffectScale;
    private Vector3 lastScaleMultiplier;

    private bool lastShouldHaveDelay;
    private float lastMinDelay;
    private float lastMaxDelay;

    protected override void InitValues()
    {
        InitValue(ref lastTimeToAppear, timeToAppear);
        InitValue(ref lastEasingMode, easingMode);
        InitValue(ref lastAffectPosition, affectPosition);
        InitValue(ref lastPositionOffset, positionOffset);
        InitValue(ref lastAffectRotation, affectRotation);
        InitValue(ref lastRotationOffset, rotationOffset);
        InitValue(ref lastAffectScale, affectScale);
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
        CheckValueChanged(ref lastAffectPosition, affectPosition, OnAffectPositionChanged);
        CheckValueChanged(ref lastPositionOffset, positionOffset, OnPositionOffsetChanged);
        CheckValueChanged(ref lastAffectRotation, affectRotation, OnAffectRotationChanged);
        CheckValueChanged(ref lastRotationOffset, rotationOffset, OnRotationOffsetChanged);
        CheckValueChanged(ref lastAffectScale, affectScale, OnAffectScaleChanged);
        CheckValueChanged(ref lastScaleMultiplier, scaleMultiplier, OnScaleMultiplierChanged);
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

    public void OnAffectPositionChanged(bool affect)
    {
        affectPosition = affect;
        if (isEnabled)
            GameStateManager.Instance.ResetGame();
    }

    public void OnPositionOffsetChanged(Vector3 pos)
    {
        positionOffset = pos;
        if (isEnabled)
            GameStateManager.Instance.ResetGame();
    }

    public void OnAffectRotationChanged(bool affect)
    {
        affectRotation = affect;
        if (isEnabled)
            GameStateManager.Instance.ResetGame();
    }

    public void OnRotationOffsetChanged(Vector3 rot)
    {
        rotationOffset = rot;
        if (isEnabled)
            GameStateManager.Instance.ResetGame();
    }

    public void OnAffectScaleChanged(bool affect)
    {
        affectScale = affect;
        if (isEnabled)
            GameStateManager.Instance.ResetGame();
    }

    public void OnScaleMultiplierChanged(Vector3 scale)
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

        List<Transform> objectsToAnimate = new()
        {
            Bar.Instance.transform,
            DeathWall.Instance.transform
        };

        objectsToAnimate.AddRange(Wall.Instances.Select(x => x.transform));
        objectsToAnimate.AddRange(Block.Instances.Select(x => x.transform));

        if (affectPosition)
        {
            foreach (Transform obj in objectsToAnimate)
            {
                numOfAnimationsToFinish++;
                Vector3 finalPos = obj.localPosition;
                obj.localPosition = obj.localPosition + positionOffset;
                obj.LeanMoveLocal(finalPos, timeToAppear).setDelay(Delay).setEase(easingMode).setOnComplete(() => { numOfAnimationsToFinish--; obj.localPosition = finalPos; });
            }
        }

        if (affectRotation)
        {
            foreach (Transform obj in objectsToAnimate)
            {
                numOfAnimationsToFinish++;
                Vector3 finalRot = obj.localRotation.eulerAngles;
                obj.localRotation = Quaternion.Euler(obj.localRotation.eulerAngles + rotationOffset);
                obj.LeanRotate(finalRot, timeToAppear).setDelay(Delay).setEase(easingMode).setOnComplete(() => { numOfAnimationsToFinish--; obj.localRotation = Quaternion.Euler(finalRot); });
            }
        }

        if (affectScale)
        {
            foreach (Transform obj in objectsToAnimate)
            {
                numOfAnimationsToFinish++;
                Vector3 finalScale = obj.localScale;
                obj.localScale = obj.localScale.Multiply(scaleMultiplier);
                obj.LeanScale(finalScale, timeToAppear).setDelay(Delay).setEase(easingMode).setOnComplete(() => { numOfAnimationsToFinish--; obj.localScale = finalScale; });
            }
        }
    }

    private void CancelAnimation()
    {
        List<GameObject> objectsToAnimate = new()
        {
            Bar.Instance.gameObject,
            DeathWall.Instance.gameObject
        };

        objectsToAnimate.AddRange(Wall.Instances.Select(x => x.gameObject));
        objectsToAnimate.AddRange(Block.Instances.Select(x => x.gameObject));

        foreach (GameObject obj in objectsToAnimate)
            LeanTween.cancel(obj,true);
    }

    private bool HasAppearGameSpaceAnimationFinished()
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
        if (state is not GameState.GameSpaceAnimation)
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
        GameStateManager.Instance.RegisterWait(GameState.GameSpaceAnimation, HasAppearGameSpaceAnimationFinished);
        GameStateManager.OnGameStateChange += OnGameStateChange;
        GameStateManager.OnGameReset += OnGameReset;

        GameStateManager.Instance.ResetGame();
    }

    public override void OnDisabled()
    {
        GameStateManager.Instance.UnregisterWait(GameState.GameSpaceAnimation, HasAppearGameSpaceAnimationFinished);
        GameStateManager.OnGameStateChange -= OnGameStateChange;

        CancelAnimation();
    }
}
