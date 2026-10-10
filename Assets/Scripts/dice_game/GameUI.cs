using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [Header("Элементы отображения")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI resultText;
    [SerializeField] private TextMeshProUGUI targetText;
    
    [Header("Элементы ввода")]
    [SerializeField] private TMP_InputField diceCountInput;
    
    [Header("Кнопки")]
    [SerializeField] private Button rollButton;
    
    [SerializeField] private DiceGame diceGame;
    [SerializeField] private GameModel gameModel;
    
    void Awake()
    {
        InitializeUI();
        
        if (rollButton != null)
        {
            rollButton.onClick.AddListener(OnRollButtonClicked);
        }
    }
    
    void InitializeUI()
    {
        UpdateScoreDisplay(0);
        UpdateResultDisplay("Введи количество кубиков и нажми кнопку");
        UpdateTargetDisplay(0);
        
        if (diceCountInput != null) diceCountInput.text = "2";
    }
    
    public void UpdateScoreDisplay(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = $"Выпало: {score}";
        }
    }
    
    public void UpdateResultDisplay(string result)
    {
        if (resultText != null)
        {
            resultText.text = result;
        }
    }
    
    public void UpdateTargetDisplay(int target)
    {
        if (targetText != null)
        {
            targetText.text = $"Цель: {target}";
        }
    }
    
    void OnRollButtonClicked()
    {
        int diceCount = ParseInput(diceCountInput, 2);
        
        if (diceGame != null)
        {
            diceGame.StartNewRound(diceCount);
        }
    }
    
    int ParseInput(TMP_InputField inputField, int defaultValue)
    {
        if (inputField != null && int.TryParse(inputField.text, out int value))
        {
            return Mathf.Clamp(value, 1, 10);
        }
        return defaultValue;
    }
    
    void OnDestroy()
    {
        if (rollButton != null)
        {
            rollButton.onClick.RemoveListener(OnRollButtonClicked);
        }
    }
}