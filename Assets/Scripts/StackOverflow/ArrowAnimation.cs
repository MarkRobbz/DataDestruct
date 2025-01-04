using UnityEngine;

public class ArrowAnimation : MonoBehaviour
{
    public float delta = 1.5f;  // Amount to move by
    public float speed = 2.0f; 
    private Vector3 startPos;

    //True for which direction to move
    public bool animateHorizontal = true; 
    public bool animateVertical = false; 
    public bool animateForward = false; 

    void Start()
    {
        startPos = transform.position;
    }

    void Update()
    {
        Vector3 v = startPos;

        if (animateHorizontal)
        {
            v.x += delta * Mathf.Sin(Time.time * speed);
        }

        if (animateVertical)
        {
            v.y += delta * Mathf.Sin(Time.time * speed);
        }

        if (animateForward)
        {
            v.z += delta * Mathf.Sin(Time.time * speed);
        }

        transform.position = v;
    }
}
