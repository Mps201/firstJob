using System;
using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [Header("Конфигурация")]
    [SerializeField] private HealthConfig config;

    public float CurrentHealth { get; private set; }
    public float CurrentHunger { get; private set; }
    public float CurrentThirst { get; private set; }
    public float CurrentStamina { get; private set; }

    public event Action<float, float> OnHealthChanged;
    public event Action<float, float> OnHungerChanged;
    public event Action<float, float> OnThirstChanged;
    public event Action<float, float> OnStaminaChanged;
    public event Action OnDeath;
    
    void Awake()
    {
        if (config == null)
        {
            Debug.LogError($"HealthConfig не назначен на объекте {gameObject.name}");
            return;
        }

        CurrentHealth = config.MaxHealth;
        CurrentHunger = config.MaxHunger;
        CurrentThirst = config.MaxThirst;
        CurrentStamina = config.MaxStamina;
    }
    
    public void TakeDamage(float damage)
    {
        ChangeHealth(-damage);
    }
    
    public void Heal(float amount)
    {
        ChangeHealth(amount);
    }
    
    private void ChangeHealth(float amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, config.MaxHealth);
        OnHealthChanged?.Invoke(CurrentHealth, config.MaxHealth);
        
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }
    
    public void IncreaseHunger(float amount)
    {
        ChangeHunger(amount);
    }
    
    public void DecreaseHunger(float amount)
    {
        ChangeHunger(-amount);
    }
    
    private void ChangeHunger(float amount)
    {
        CurrentHunger = Mathf.Clamp(CurrentHunger + amount, 0, config.MaxHunger);
        OnHungerChanged?.Invoke(CurrentHunger, config.MaxHunger);
    }
    
    public void IncreaseThirst(float amount)
    {
        ChangeThirst(amount);
    }
    
    public void DecreaseThirst(float amount)
    {
        ChangeThirst(-amount);
    }
    
    private void ChangeThirst(float amount)
    {
        CurrentThirst = Mathf.Clamp(CurrentThirst + amount, 0, config.MaxThirst);
        OnThirstChanged?.Invoke(CurrentThirst, config.MaxThirst);
    }
    
    public void IncreaseStamina(float amount)
    {
        ChangeStamina(amount);
    }
    
    public void DecreaseStamina(float amount)
    {
        ChangeStamina(-amount);
    }
    
    private void ChangeStamina(float amount)
    {
        CurrentStamina = Mathf.Clamp(CurrentStamina + amount, 0, config.MaxStamina);
        OnStaminaChanged?.Invoke(CurrentStamina, config.MaxStamina);
    }
    
    private void Die()
    {
        OnDeath?.Invoke();
        Debug.Log($"{gameObject.name} умер");
    }
}