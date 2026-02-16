using UnityEngine;
using System;
using System.Collections.Generic;

public class Wall : MonoBehaviour
{
    public static Action<GameObject> OnHit;

    public static HashSet<Wall> Instances = new();

    private void Awake()
        => Instances.Add(this);

    private void Start()
        => GameStateManager.OnGameReset += ResetAwake;

    private void ResetAwake()
        => Instances.Remove(this);

    public void Hit()
        => OnHit?.Invoke(gameObject);
}
