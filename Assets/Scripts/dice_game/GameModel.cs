using UnityEngine;

public class GameModel : MonoBehaviour
{
    public int Target { get; private set; }
    
    public GameResult CurrentResult { get; private set; }
    
    public enum GameResult
    {
        None,
        Win,
        Draw,
        Lose
    }

    public void GenerateTarget(int diceCount)
    {
        int minScore = diceCount;
        int maxScore = diceCount * 6;

        Target = Random.Range(minScore, maxScore + 1);
    }

    public void DetermineResult(int totalScore)
    {
        if (totalScore > Target)
        {
            CurrentResult = GameResult.Win;
        }
        else if (totalScore < Target)
        {
            CurrentResult = GameResult.Lose;
        }
        else
        {
            CurrentResult = GameResult.Draw;
        }
    }
    
    public void ResetResult()
    {
        CurrentResult = GameResult.None;
    }
}