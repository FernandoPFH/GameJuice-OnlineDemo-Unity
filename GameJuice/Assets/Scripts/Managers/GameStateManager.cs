using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Linq;
using System.Collections.Generic;

public class GameStateManager : Singleton<GameStateManager>
{
    public static Action OnGameReset;
    public static Action<GameState> OnGameStateChange;

    public static GameState GameState { get; private set; } = GameState.GameSpaceAnimation;

    private Dictionary<GameState, List<Func<bool>>> waitPerGameState = new();

    private void Start()
    {
        Block.OnHit += OnBlockHit;
        LifeManager.OnLifeLost += OnLifeLost;
    }

    private void Update()
    {
        switch (GameState)
        {
            case GameState.GameSpaceAnimation:
                if (!waitPerGameState.TryGetValue(GameState, out List<Func<bool>> waits))
                {
                    AdvanceState();
                    break;
                }

                if (waits.Count == 0 || waits.All(x => x()))
                    AdvanceState();

                break;
        }
    }

    private void OnDestroy()
    {
        Block.OnHit -= OnBlockHit;
        LifeManager.OnLifeLost -= OnLifeLost;
    }

    private void OnBlockHit(GameObject block, int blocksLefted)
    {
        if (blocksLefted == 0)
            ResetGame();
    }

    private void OnLifeLost(int lifesLefted)
        => BallSpawner.SpawnBall();

    private void AdvanceState()
        => SetGameState(++GameState);

    private void RevertState()
        => SetGameState(--GameState);

    private void SetGameState(GameState state)
    {
        GameState = state;
        OnGameStateChange?.Invoke(state);
    }

    public void ResetGame()
    {

        Ball.Instance?.Reset();
        Bar.Instance?.Reset();
        if (Block.Instances.Count > 0)
            foreach (Block block in Block.Instances)
                block.Reset();

        OnGameReset?.Invoke();

        SetGameState(GameState.GameSpaceAnimation);
    }

    public void RegisterWait(GameState gameState, Func<bool> predicate)
    {
        if (!waitPerGameState.TryGetValue(gameState, out List<Func<bool>> waits))
            waits = waitPerGameState[gameState] = new();

        waits.Add(predicate);
    }

    public void UnregisterWait(GameState gameState, Func<bool> predicate)
    {
        if (!waitPerGameState.TryGetValue(gameState, out List<Func<bool>> waits))
            return;

        waits.Remove(predicate);
    }
}

public enum GameState
{
    GameSpaceAnimation,
    GameLoop
}
