using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{
    [Header("Настройки управления")]
    [SerializeField] private InputActionReference throwAction;
    
    public event Action OnDiceThrowPressed;
    
    void OnEnable()
    {
        throwAction.action.performed += OnThrowPerformed;
        throwAction.action.Enable();
    }
    
    void OnDisable()
    {
        throwAction.action.performed -= OnThrowPerformed;
        throwAction.action.Disable();
    }
    
    void OnThrowPerformed(InputAction.CallbackContext context)
    {
        OnDiceThrowPressed?.Invoke();
    }
}