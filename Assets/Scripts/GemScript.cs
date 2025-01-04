using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GemScript : MonoBehaviour
{
    public float floatAmplitude = 0.5f;
    public float floatFrequency = 1f;
    public float spinSpeed = 50f;
    public Transform playerTransform; 

    private Vector3 startPosition;
    private Vector3 tempPos;
    
    public AudioClip coinSound; 
    private AudioSource audioSource;

    private void Start()
    {
        startPosition = transform.position;
        
        startPosition = transform.position;
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the gem!");
        }
    }

    private void Update()
    {
        if (IsPlayerLookingAtGem())
        {
            FloatAndSpin();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            GameManager.instance.CollectGem(); 
            audioSource.PlayOneShot(coinSound); 
            
            AudioSource.PlayClipAtPoint(coinSound, transform.position);
            Destroy(gameObject); 
        }
    }

    private void FloatAndSpin()
    {
        // Floating effect
        tempPos = startPosition;
        tempPos.y += Mathf.Sin(Time.fixedTime * Mathf.PI * floatFrequency) * floatAmplitude;
        transform.position = tempPos;

        // Spinning effect
        transform.Rotate(Vector3.up, spinSpeed * Time.deltaTime, Space.World);
    }
    private bool IsPlayerLookingAtGem()
    {
        Vector3 cameraForward = Camera.main.transform.forward;
        Vector3 directionToGem = (transform.position - Camera.main.transform.position).normalized;
        float angle = Vector3.Angle(cameraForward, directionToGem);

        return angle < 60f; //adjust when moving cinemachine camera
    }



}

