using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TrashItem : MonoBehaviour
{
    public static List<int> usedIndices = new List<int>();
    public int trashIndex;
    public Sprite image;
    public GameObject floatingTextPrefab;
    private GameObject floatingText;


    private void Awake()
    {
        // If trashIndex is not set, then randomize it
        if (trashIndex == -1)
        {
            int totalSlots = FindObjectOfType<ArrayAlleywayUI>().arraySlots.Length;
            List<int> availableIndices = new List<int>();
            for (int i = 0; i < totalSlots; i++)
            {
                if (!usedIndices.Contains(i))
                {
                    availableIndices.Add(i);
                }
            }

            if (availableIndices.Count > 0)
            {
                trashIndex = availableIndices[Random.Range(0, availableIndices.Count)];
                usedIndices.Add(trashIndex);
            }
            else
            {
                Debug.LogError("No indices left for TrashItems!");
            }
        }
    }

    private void Start()
    {
        floatingText = Instantiate(floatingTextPrefab, transform.position + new Vector3(0, 1, 0), Quaternion.identity, transform);
        UpdateFloatingText();

      
    }

    private void UpdateFloatingText()
    {
 

        if (floatingText)
        {
            Debug.Log("Code triggered");
            TextMeshPro tmp = floatingText.GetComponent<TextMeshPro>();
            if (tmp != null)
            {
                tmp.text = "TrashObjectArray[" + trashIndex + "]";
            }
            else
            {
                Debug.LogError("TextMeshPro component not found on FloatingText.");
            }
        }
    }
}


