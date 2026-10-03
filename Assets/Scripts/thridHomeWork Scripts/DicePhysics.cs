using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class DicePhysics : MonoBehaviour
{
    private Rigidbody rb;
    
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        
        rb.mass = 1f;
        rb.linearDamping = 0.1f;
        rb.angularDamping = 0.1f;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
}