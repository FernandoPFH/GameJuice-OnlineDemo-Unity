using UnityEngine;
using System;
using System.Collections.Generic;

public class Block : MonoBehaviour
{
    public static Action<GameObject, int> OnHit;
    public static int Count = 0;

    public static HashSet<Block> Instances = new();

    private void Awake()
        => Instances.Add(this);

    private void Start()
    {
        Count++;
        GameStateManager.OnGameReset += ResetAwake;
    }

    private void ResetAwake()
        => Instances.Remove(this);

    public void Hit()
    {
        OnHit?.Invoke(gameObject, --Count);

        gameObject.SetActive(false);
    }
}
