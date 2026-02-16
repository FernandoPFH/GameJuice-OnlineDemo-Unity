using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameStateManager : Singleton<GameStateManager>
{
    [SerializeField] private GameObject winScreen;
    [SerializeField] private GameObject loseScreen;

    public static Action OnGameReset;

    private void Start()
    {
        Block.OnHit += OnBlockHit;
        LifeManager.OnLifeLost += OnLifeLost;
    }

    private void OnDestroy()
    {
        Block.OnHit -= OnBlockHit;
        LifeManager.OnLifeLost -= OnLifeLost;
    }

    private void OnBlockHit(GameObject block, int blocksLefted)
    {
        if (blocksLefted == 0)
            EndGame(true);
    }

    private void OnLifeLost(int lifesLefted)
    {
        if (lifesLefted == 0)
        {
            EndGame(false);
            return;
        }

        BallSpawner.SpawnBall();
    }

    private void EndGame(bool hasWon)
    {
        if (hasWon)
            WinGame();
        else
            LoseGame();
    }

    private void WinGame()
        => winScreen.SetActive(true);

    private void LoseGame()
        => loseScreen.SetActive(true);

    public void ResetGame()
    {
        OnGameReset?.Invoke();

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
