using UnityEngine;
using System;
using System.Collections.Generic;

public class Wall : MonoBehaviour
{
    public static Action<GameObject> OnHit;

    public static HashSet<Wall> Instances = new();

    private void Awake()
        => Instances.Add(this);

    public void Hit()
        => OnHit?.Invoke(gameObject);
}
