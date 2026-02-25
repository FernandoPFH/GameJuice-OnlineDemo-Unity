using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Block))]
public class BlockRefs : MonoBehaviour
{
    public static Dictionary<Block, BlockRefs> Instances = new();

    private void Awake()
        => Instances.Add(GetComponent<Block>(), this);

    public SpriteRenderer Renderer;

#if UNITY_EDITOR
    private void OnValidate()
    {
        Renderer = transform.Find("Renderer").GetComponent<SpriteRenderer>();
    }
#endif
}
