using UnityEngine;

[CreateAssetMenu(fileName = "HealthConfig", menuName = "Survival/Health Config")]
public class HealthConfig : ScriptableObject
{
    [Header("Здоровье")]
    [SerializeField] private float maxHealth = 100f;
    
    [Header("Голод")]
    [SerializeField] private float maxHunger = 100f;
    
    [Header("Жажда")]
    [SerializeField] private float maxThirst = 100f;
    
    [Header("Выносливость")]
    [SerializeField] private float maxStamina = 100f;
    
    
    public float MaxHealth => maxHealth;
    public float MaxHunger => maxHunger;
    public float MaxThirst => maxThirst;
    public float MaxStamina => maxStamina;
}