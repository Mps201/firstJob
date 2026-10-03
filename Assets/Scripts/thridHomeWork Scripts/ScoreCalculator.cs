using System.Collections.Generic;
using UnityEngine;

public class ScoreCalculator : MonoBehaviour
{
    private List<GameObject> diceList = new List<GameObject>();
    private int totalScore = 0;

    private const float FaceThreshold = 0.7f;
    
    public void SetDiceList(List<GameObject> dice)
    {
        diceList = dice;
    }
    
    public void CalculateScore()
    {
        totalScore = 0;
        
        foreach (var dice in diceList)
        {
            int faceValue = GetTopFaceValue(dice);
            totalScore += faceValue;
        }
    }
    
    public int GetLastScore() => totalScore;
    
    int GetTopFaceValue(GameObject dice)
    {
        Vector3 right = dice.transform.right;
        Vector3 up = dice.transform.up;
        Vector3 forward = dice.transform.forward;

        Vector3 worldUp = Vector3.up;

        float dotRight = Vector3.Dot(right, worldUp);
        float dotUp = Vector3.Dot(up, worldUp);
        float dotForward = Vector3.Dot(forward, worldUp);

        float maxAbs = Mathf.Max(
            Mathf.Abs(dotRight),
            Mathf.Abs(dotUp),
            Mathf.Abs(dotForward)
        );

        if (Mathf.Abs(dotUp) == maxAbs)
        {
            return dotUp > 0 ? 1 : 6;
        }
        if (Mathf.Abs(dotForward) == maxAbs)
        {
            return dotForward > 0 ? 2 : 5;
        }
        if (Mathf.Abs(dotRight) == maxAbs)
        {
            return dotRight > 0 ? 3 : 4;
        }

        return 1;
    }
}