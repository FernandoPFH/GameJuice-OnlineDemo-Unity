using UnityEngine;
using System;

public class LifeManager : Singleton<LifeManager>
{
    [SerializeField] private int initialLifes = 3;

    public static Action<int> OnLifeLost;
    private int lifes;

    private void Start()
    {
        DeathWall.OnHit += OnDeathWallHit;

        lifes = initialLifes;

        LifeUI.Instance.UpdateText(initialLifes);
    }

    private void OnDestroy()
        => DeathWall.OnHit -= OnDeathWallHit;


    private void OnDeathWallHit(GameObject deathWall)
    {
        LifeUI.Instance.UpdateText(--lifes);
        OnLifeLost?.Invoke(lifes);
    }
}
