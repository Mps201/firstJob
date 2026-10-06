using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    private List<Dice> diceList = new List<Dice>();
    private int totalScore = 0;
    
    public void SetDiceList(List<Dice> dice)
    {
        diceList = dice;
    }
    
    public void CalculateScore()
    {
        totalScore = 0;
        
        foreach (var dice in diceList)
        {
            totalScore += dice.GetTopFaceValue();
        }
    }
    
    public int GetLastScore() => totalScore;
}