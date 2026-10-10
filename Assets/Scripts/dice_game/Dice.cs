using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Dice : MonoBehaviour
{
    public Rigidbody Rb { get; private set; }

    void Awake()
    {
        Rb = GetComponent<Rigidbody>();

        Rb.mass = 1f;
        Rb.linearDamping = 0.1f;
        Rb.angularDamping = 0.5f;
        Rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }

    public int GetTopFaceValue()
    {
        Vector3 right = transform.right;
        Vector3 up = transform.up;
        Vector3 forward = transform.forward;
        Vector3 worldUp = Vector3.up;

        float dotRight = Vector3.Dot(right, worldUp);
        float dotUp = Vector3.Dot(up, worldUp);
        float dotForward = Vector3.Dot(forward, worldUp);

        float maxAbs = Mathf.Max(Mathf.Abs(dotRight), Mathf.Abs(dotUp), Mathf.Abs(dotForward));

        if (Mathf.Abs(dotUp) == maxAbs) return dotUp > 0 ? 1 : 6;
        if (Mathf.Abs(dotForward) == maxAbs) return dotForward > 0 ? 2 : 5;
        if (Mathf.Abs(dotRight) == maxAbs) return dotRight > 0 ? 3 : 4;

        return 1;
    }
}