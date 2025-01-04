using UnityEngine;

public class FloatyEffect : MonoBehaviour
{
    public float amplitude = 0.5f;
    public float frequency = 1f;

    // Positions 
    Vector3 posOffset = new Vector3();
    Vector3 tempPos = new Vector3();

    //initialization
    void Start()
    {
        // Store  starting pos and rotation
        posOffset = transform.position;
    }

    
    void Update()
    {
        // Float up/down with Sin()
        tempPos = posOffset;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * frequency) * amplitude;

        transform.position = tempPos;
    }
}