using System;
using UnityEngine;

public class GameModel : MonoBehaviour
{
    // События
    public event Action<int> OnTargetGenerated;
    public event Action OnThrowStarted;
    public event Action<GameResult, int> OnResultDetermined;
    
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
        Target = UnityEngine.Random.Range(minScore, maxScore + 1);
        
        OnTargetGenerated?.Invoke(Target);
    }

    public void NotifyThrowStarted()
    {
        OnThrowStarted?.Invoke();
    }
    
    public void DetermineResult(int totalScore)
    {
        if (totalScore > Target)
            CurrentResult = GameResult.Win;
        else if (totalScore < Target)
            CurrentResult = GameResult.Lose;
        else
            CurrentResult = GameResult.Draw;
        
        OnResultDetermined?.Invoke(CurrentResult, totalScore);
    }
    
    public void ResetResult()
    {
        CurrentResult = GameResult.None;
    }
}