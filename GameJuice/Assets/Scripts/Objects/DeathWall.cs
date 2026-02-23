using UnityEngine;
using System;

public class DeathWall : Singleton<DeathWall>
{
    public static Action<GameObject> OnHit;

    public void Hit()
        => OnHit?.Invoke(gameObject);
}
