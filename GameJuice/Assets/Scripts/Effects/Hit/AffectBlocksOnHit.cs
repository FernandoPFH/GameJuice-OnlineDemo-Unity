using UnityEngine;
using System.Linq;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "AffectBlocksOnHit_EffectSO", menuName = "EffectSO/AffectBlocksOnHit")]
public class AffectBlocksOnHit : EffectSO
{
    [SerializeField] private Effect effect;
    [SerializeField] private float scaleMultiplier = 1.1f;
    [SerializeField] private Vector3 positionOffset;
    [SerializeField] private float animationTime = 0.2f;
    [SerializeField] private LeanTweenType easingMode = LeanTweenType.easeInOutExpo;

    private void StartAnimation(GameObject block)
    {
        List<SpriteRenderer> renderersToAffect = Block.Instances.Where(x => x.gameObject != block).Select(x => BlockRefs.Instances[x].Renderer).ToList();

        foreach (SpriteRenderer renderer in renderersToAffect)
        {
            switch (effect)
            {
                case Effect.Scale:
                    LTSeq seqSca = LeanTween.sequence();
                    seqSca.append(renderer.transform.LeanScale(Vector3.one * scaleMultiplier, animationTime / 2f));
                    seqSca.append(renderer.transform.LeanScale(Vector3.one, animationTime / 2f));
                    break;
                case Effect.Shake:
                    LTSeq seqSha = LeanTween.sequence();
                    seqSha.append(renderer.transform.LeanMoveLocal(positionOffset, animationTime / 4f));
                    seqSha.append(renderer.transform.LeanMoveLocal(-positionOffset, animationTime / 2f));
                    seqSha.append(renderer.transform.LeanMoveLocal(Vector3.zero, animationTime / 4f));
                    break;
            }
        }
    }

    private void CancelAnimation()
    {
        foreach (Block block in Block.Instances)
            LeanTween.cancelAll(BlockRefs.Instances[block].Renderer.transform);
    }

    private void OnBlockHit(GameObject block, int count, Vector3 ballVelocity)
        => StartAnimation(block);

    public override void OnEnabled()
        => Block.OnHit += OnBlockHit;

    public override void OnDisabled()
    {
        CancelAnimation();
        Block.OnHit -= OnBlockHit;
    }

    private enum Effect
    {
        Shake,
        Scale
    }
}
