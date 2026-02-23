using UnityEngine;
using System;
using System.Linq;
using System.Collections.Generic;

public class Block : MonoBehaviour
{
    public static Action<GameObject, int> OnHit;
    public static int Count => Instances.Count(x => x.gameObject.activeInHierarchy);

    public static HashSet<Block> Instances = new();

    private void Awake()
        => Instances.Add(this);

    public void Reset()
        => gameObject.SetActive(true);

    public void Hit()
    {
        gameObject.SetActive(false);

        OnHit?.Invoke(gameObject, Count);
    }
}
