using System.Collections.Generic;
using UnityEngine;

public class HealthHeartBar : MonoBehaviour
{
    public GameObject heartPrefab;
    public PlayerHealth playerHealth;

    List<HealthHeart> hearts = new List<HealthHeart>();



    private void Start()
    {
        DrawHearts();
    }



    private void OnEnable()
    {
        PlayerHealth.OnPlayerDamaged += DrawHearts;  //Call drawhearts each time player takes damage
    }

    private void OnDisable()
    {
        PlayerHealth.OnPlayerDamaged -= DrawHearts;
    }

    public void DrawHearts()
    {
        ClearHeartsBar();
        int heartsToMake = playerHealth.maxHealth;
        for (int i = 0; i < heartsToMake; i++)
        {
            CreateEmptyHeart();
        }


        for (int i = 0; i < playerHealth.currentHealth; i++)
        {
            hearts[i].SetHeartImage(HeartStatus.Full);
        }
    }

    public void CreateEmptyHeart()
    {
        GameObject newHeart = Instantiate(heartPrefab);
        newHeart.transform.SetParent(transform, false);

        HealthHeart heartComponent = newHeart.GetComponent<HealthHeart>();

        heartComponent.SetHeartImage(HeartStatus.Empty);
        hearts.Add(heartComponent);

    }

    public void ClearHeartsBar()
    {
        foreach (Transform transform in transform) //Clears the hearts bar
        {
            Destroy(transform.gameObject);
        }
        hearts = new List<HealthHeart>();

    }
}
