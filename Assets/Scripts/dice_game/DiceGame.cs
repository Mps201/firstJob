using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiceGame : MonoBehaviour
{
    [Header("Настройки кубиков")]
    [SerializeField] private Dice dicePrefab;
    [SerializeField] private float throwForceMin = 5f;
    [SerializeField] private float throwForceMax = 10f;
    
    [Header("Ссылки")]
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private ScoreCalculator scoreCalculator;
    [SerializeField] private GameModel gameModel;
    [SerializeField] private GameUI gameUI;
    
    private List<Dice> diceList = new List<Dice>();
    private bool isDiceRolling = false;
    private Vector3 throwPosition;
    
    private const float VelocityThreshold = 0.05f;
    private const float AngularVelocityThreshold = 0.05f;
    private const int StableFramesRequired = 10;
    
    void Start()
    {
        throwPosition = transform.position;
        inputHandler.OnDiceThrowPressed += ThrowDice;

        StartNewRound(2);
    }

    public void StartNewRound(int diceCount)
    {
        if (isDiceRolling) return;
        
        gameModel.GenerateTarget(diceCount);

        CreateDice(diceCount);
        ThrowDice();
    }
    
    void CreateDice(int diceCount)
    {
        foreach (var dice in diceList)
        {
            Destroy(dice.gameObject);
        }
        diceList.Clear();
        
        for (int i = 0; i < diceCount; i++)
        {
            Dice diceInstance = Instantiate(dicePrefab, throwPosition, Quaternion.identity);
            diceList.Add(diceInstance);
        }
        
        scoreCalculator.SetDiceList(diceList);
    }
    
    void ThrowDice()
    {
        if (isDiceRolling) return;
        
        isDiceRolling = true;
        gameModel.ResetResult();
        gameModel.NotifyThrowStarted(); 
        
        foreach (var dice in diceList)
        {
            dice.transform.position = throwPosition;
            dice.transform.rotation = Quaternion.identity;
            dice.Rb.isKinematic = false;
            
            float force = UnityEngine.Random.Range(throwForceMin, throwForceMax);
            Vector3 throwDirection = new Vector3(
                UnityEngine.Random.Range(-1f, 1f),
                1f,
                UnityEngine.Random.Range(-1f, 1f)
            ).normalized;
            
            dice.Rb.AddForce(throwDirection * force, ForceMode.Impulse);
            
            Vector3 randomRotation = new Vector3(
                UnityEngine.Random.Range(100f, 500f),
                UnityEngine.Random.Range(100f, 500f),
                UnityEngine.Random.Range(100f, 500f)
            );
            dice.Rb.AddTorque(randomRotation, ForceMode.Impulse);
        }
        
        StartCoroutine(CheckDiceStopped());
    }
    
    IEnumerator CheckDiceStopped()
    {
        yield return new WaitForSeconds(1f);
        int stableFrames = 0;
        
        while (stableFrames < StableFramesRequired)
        {
            bool allStopped = true;
            
            foreach (var dice in diceList)
            {
                if (dice.Rb.linearVelocity.magnitude > VelocityThreshold || 
                    dice.Rb.angularVelocity.magnitude > AngularVelocityThreshold)
                {
                    allStopped = false;
                    break;
                }
            }
            
            if (allStopped) stableFrames++;
            else stableFrames = 0;
            
            yield return new WaitForSeconds(0.05f);
        }
        
        yield return new WaitForSeconds(0.3f);
        
        scoreCalculator.CalculateScore();
        int score = scoreCalculator.GetLastScore();
        
        gameModel.DetermineResult(score);
        
        isDiceRolling = false;
    }
    
    void OnDestroy()
    {
        inputHandler.OnDiceThrowPressed -= ThrowDice;
    }
}