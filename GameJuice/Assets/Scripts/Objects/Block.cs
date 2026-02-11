using UnityEngine;
using System;

public class Block : MonoBehaviour
{
    public static Action<GameObject, int> OnHit;
    public static int Count = 0;

    private void Start()
        => Count++;

    public void Hit()
    {
        OnHit?.Invoke(gameObject, --Count);

        Destroy(gameObject);
    }
}
