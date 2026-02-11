using UnityEngine;

public class EndGameTitle : MonoBehaviour
{
    public void ResetGame()
        => GameStateManager.Instance.ResetGame();
}
