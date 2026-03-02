using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class Block : MonoBehaviour
{
    public static Action<GameObject, int, Vector3> OnHit;
    public static int Count => Instances.Count(x => x.gameObject.GetComponent<BoxCollider2D>().enabled);
    public static int TotalCount => Instances.Count;

    public static HashSet<Block> Instances = new();

    public static Action<Block, BlockState> OnBlockStateChange;
    private static Dictionary<BlockState, List<Func<bool>>> waitPerBlockState = new();

    public BlockState BlockState = BlockState.Enabled;

    private void Awake()
        => Instances.Add(this);

    private void Update()
    {
        if (BlockState is BlockState.Enabled)
            return;

        switch (BlockState)
        {
            case BlockState.DisappearAnimation:
                if (!waitPerBlockState.TryGetValue(BlockState, out List<Func<bool>> bsWaits))
                {
                    AdvanceState();
                    break;
                }

                if (bsWaits.Count == 0 || bsWaits.All(x => x()))
                    AdvanceState();

                break;
            case BlockState.Disabled:
                gameObject.SetActive(false);
                break;
        }
    }

    private void AdvanceState()
        => SetBlockState(++BlockState);

    private void RevertState()
        => SetBlockState(--BlockState);

    private void SetBlockState(BlockState state)
    {
        BlockState = state;
        OnBlockStateChange?.Invoke(this, state);
    }

    public void Reset()
    {
        SetBlockState(BlockState.Enabled);
        gameObject.GetComponent<BoxCollider2D>().enabled = true;
        gameObject.SetActive(true);
        Color color = BlockRefs.Instances[this].Renderer.color;
        color.a = 1f;
        BlockRefs.Instances[this].Renderer.color = color;
    }

    public void Hit(Vector3 ballVelocity)
    {
        gameObject.GetComponent<BoxCollider2D>().enabled = false;

        OnHit?.Invoke(gameObject, Count, ballVelocity);

        AdvanceState();
    }

    public static void RegisterWait(BlockState blockState, Func<bool> predicate)
    {
        if (!waitPerBlockState.TryGetValue(blockState, out List<Func<bool>> waits))
            waits = waitPerBlockState[blockState] = new();

        waits.Add(predicate);
    }

    public static void UnregisterWait(BlockState blockState, Func<bool> predicate)
    {
        if (!waitPerBlockState.TryGetValue(blockState, out List<Func<bool>> waits))
            return;

        waits.Remove(predicate);
    }
}

public enum BlockState
{
    Enabled,
    DisappearAnimation,
    Disabled
}
