using UnityEngine;
using System;

using FernandoPFH_Essentials_Runtime;

public class DeathWall : Singleton<DeathWall>
{
    public static Action<GameObject, Vector3> OnHit;

    public void Hit(Vector3 ballPosition)
        => OnHit?.Invoke(gameObject, ballPosition);
}
