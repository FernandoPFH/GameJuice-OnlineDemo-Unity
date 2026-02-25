using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "DisappearBlockOnHit_EffectSO", menuName = "EffectSO/DisappearBlockOnHit")]
public class DisappearBlockOnHit : EffectSO
{
    [SerializeField] private Effect effect;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private float pushMultiplier = 1f;
    [SerializeField] private float animationTime = 0.5f;
    [SerializeField] private LeanTweenType easingMode = LeanTweenType.easeInOutExpo;

    private bool hasAnimationStarted;
    private int numOfAnimationsToFinish;
    private Dictionary<Block, Vector3> velocityOfHitPerBlock = new();

    private void UpdateBlockAlpha(Block block, float alpha)
    {
        SpriteRenderer renderer = BlockRefs.Instances[block].Renderer;
        Color color = renderer.color;
        color.a = alpha;
        renderer.color = color;
    }

    private void StartAnimation(Block block)
    {
        hasAnimationStarted = true;
        SpriteRenderer renderer = BlockRefs.Instances[block].Renderer;

        switch (effect)
        {
            case Effect.Scale:
                numOfAnimationsToFinish++;
                renderer.transform.LeanScale(Vector3.zero, animationTime).setEase(easingMode).setOnComplete(() => { numOfAnimationsToFinish--; renderer.transform.localScale = Vector3.zero; });
                break;
            case Effect.Fall:
                numOfAnimationsToFinish += 2;
                renderer.transform.LeanMoveLocal(positionOffset, animationTime).setEase(easingMode).setOnComplete(() => { numOfAnimationsToFinish--; renderer.transform.localPosition = Vector3.zero; });
                LeanTween.value(renderer.gameObject, (float alpha) => UpdateBlockAlpha(block, alpha), 1f, 0f, animationTime).setEase(easingMode).setOnComplete(() => { numOfAnimationsToFinish--; UpdateBlockAlpha(block, 0f); });
                break;
            case Effect.Push:
                numOfAnimationsToFinish += 2;
                renderer.transform.LeanMoveLocal(-velocityOfHitPerBlock[block].normalized * pushMultiplier, animationTime).setEase(easingMode).setOnComplete(() => { numOfAnimationsToFinish--; renderer.transform.localPosition = Vector3.zero; });
                LeanTween.value(renderer.gameObject, (float alpha) => UpdateBlockAlpha(block, alpha), 1f, 0f, animationTime).setEase(easingMode).setOnComplete(() => { numOfAnimationsToFinish--; UpdateBlockAlpha(block, 0f); });
                break;
        }
    }

    private void CancelAnimation()
    {
        foreach (Block block in Block.Instances)
            LeanTween.cancelAll(BlockRefs.Instances[block].Renderer.transform);
    }

    private void OnBlockStateChange(Block block, BlockState state)
    {
        switch (state)
        {
            case BlockState.Enabled:
                hasAnimationStarted = false;
                velocityOfHitPerBlock.Clear();
                break;
            case BlockState.DisappearAnimation:
                StartAnimation(block);
                break;
        }
        if (state is BlockState.DisappearAnimation)
            StartAnimation(block);
    }

    private void OnBlockHit(GameObject block, int count, Vector3 ballVelocity)
        => velocityOfHitPerBlock[block.GetComponent<Block>()] = ballVelocity;

    private bool HasDisappearBlockAnimationFinished()
    {
        if (!isEnabled)
            return true;

        if (!hasAnimationStarted)
            return false;

        if (numOfAnimationsToFinish > 0)
            return false;

        return true;
    }


    public override void OnEnabled()
    {
        Block.OnBlockStateChange += OnBlockStateChange;
        Block.OnHit += OnBlockHit;
        Block.RegisterWait(BlockState.DisappearAnimation, HasDisappearBlockAnimationFinished);
    }

    public override void OnDisabled()
    {
        CancelAnimation();
        Block.OnBlockStateChange -= OnBlockStateChange;
        Block.OnHit -= OnBlockHit;
        Block.UnregisterWait(BlockState.DisappearAnimation, HasDisappearBlockAnimationFinished);
    }

    private enum Effect
    {
        Scale,
        Fall,
        Push
    }
}
