using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DiceGame : MonoBehaviour
{
    [Header("Настройки кубиков")]
    [SerializeField] private GameObject dicePrefab;
    [SerializeField] private int diceCount = 2;
    [SerializeField] private float throwForceMin = 5f;
    [SerializeField] private float throwForceMax = 10f;
    
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI statusText;
    
    [Header("Ссылки")]
    [SerializeField] private InputHandler inputHandler;
    [SerializeField] private ScoreCalculator scoreCalculator;

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
        
        SetStatusText("Нажми Space для броска");
        CreateDice();
    }
    
    void SetStatusText(string text)
    {
        if (statusText != null) statusText.text = text;
    }
    
    void CreateDice()
    {
        foreach (var dice in diceList)
        {
            Destroy(dice.gameObject);
        }
        diceList.Clear();
        
        for (int i = 0; i < diceCount; i++)
        {
            GameObject diceObj = Instantiate(dicePrefab, throwPosition, Quaternion.identity);
            Dice diceComponent = diceObj.GetComponent<Dice>();
            diceList.Add(diceComponent);
        }
        
        scoreCalculator.SetDiceList(diceList);
    }
    
    void ThrowDice()
    {
        if (isDiceRolling) return;
        
        isDiceRolling = true;
        SetStatusText("Rolling...");
        
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
        SetStatusText($"Счёт: {score}");
        
        isDiceRolling = false;
    }
    
    void OnDestroy()
    {
        inputHandler.OnDiceThrowPressed -= ThrowDice;
    }
}