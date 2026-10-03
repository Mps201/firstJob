using System.Collections.Generic;
using UnityEngine;

public class CubeRotator : MonoBehaviour
{
    [Header("Настройки вращения")]
    [Min(0.1f)]
    public float radius = 3f;
    
    public float speed = 50f;
    public bool clockwise = true;
    
    [Header("Настройки кубиков")]
    [Min(1)]
    public int cubeCount = 5;
    public bool evenlyDistributed = true;
    
    [Header("Префаб")]
    public GameObject cubePrefab;
    
    private List<GameObject> cubes = new List<GameObject>();
    private float currentAngle = 0f;
    
    private float _lastRadius;
    private float _lastSpeed;
    private bool _lastClockwise;
    private bool _lastEvenlyDistributed;
    private int _lastCubeCount;
    
    void Awake()
    {
        CacheValues();
        CreateCubes();
    }
    
    void OnValidate()
    {
        bool positionsNeedUpdate = false;
        
        if (_lastRadius != radius)
        {
            _lastRadius = radius;
            positionsNeedUpdate = true;
        }
        
        if (_lastSpeed != speed)
        {
            _lastSpeed = speed;
        }
        
        if (_lastClockwise != clockwise)
        {
            _lastClockwise = clockwise;
        }
        
        if (_lastEvenlyDistributed != evenlyDistributed)
        {
            _lastEvenlyDistributed = evenlyDistributed;
            positionsNeedUpdate = true;
        }
        
        if (_lastCubeCount != cubeCount)
        {
            _lastCubeCount = cubeCount;
            CreateCubes();
            return;
        }
        
        if (positionsNeedUpdate)
        {
            RecalculatePositions();
        }
    }
    
    void CacheValues()
    {
        _lastRadius = radius;
        _lastSpeed = speed;
        _lastClockwise = clockwise;
        _lastEvenlyDistributed = evenlyDistributed;
        _lastCubeCount = cubeCount;
    }
    
    void CreateCubes()
    {
        foreach (GameObject cube in cubes)
        {
            Destroy(cube);
        }
        cubes.Clear();
        
        for (int i = 0; i < cubeCount; i++)
        {
            GameObject cube = Instantiate(cubePrefab, transform);
            cubes.Add(cube);
        }
        
        RecalculatePositions();
    }
    
    void RecalculatePositions()
    {
        for (int i = 0; i < cubes.Count; i++)
        {
            UpdateCubePosition(i, 0f);
        }
    }
    
    void RotateCubes()
    {
        float direction = clockwise ? 1f : -1f;
        currentAngle += speed * direction * Time.deltaTime;
        
        for (int i = 0; i < cubes.Count; i++)
        {
            float totalAngle = CalculateBaseAngle(i) + currentAngle;
            UpdateCubePosition(i, totalAngle);
        }
    }
    
    void UpdateCubePosition(int index, float angleOffset)
    {
        float baseAngle = CalculateBaseAngle(index);
        float totalAngle = baseAngle + angleOffset;
        float angleRad = totalAngle * Mathf.Deg2Rad;
        
        float x = Mathf.Cos(angleRad) * radius;
        float z = Mathf.Sin(angleRad) * radius;
        
        cubes[index].transform.localPosition = new Vector3(x, 0, z);
    }
    
    float CalculateBaseAngle(int index)
    {
        if (evenlyDistributed)
        {
            return (360f / cubeCount) * index;
        }
        else
        {
            return 30f * index;
        }
    }
    
    void Update()
    {
        RotateCubes();
    }
}