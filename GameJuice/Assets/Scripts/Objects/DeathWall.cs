using UnityEngine;
using System;

public class DeathWall : MonoBehaviour
{
    public static Action<GameObject> OnHit;

    public void Hit()
        => OnHit?.Invoke(gameObject);
}
