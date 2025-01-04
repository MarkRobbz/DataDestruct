using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthHeart : MonoBehaviour
{
    public Sprite emptyHeart, fullHeart;
    Image heartImg;

    private void Awake()
    {
        heartImg = GetComponent<Image>();

    }

    public void SetHeartImage(HeartStatus status)
    {
        switch(status)
        {
            case HeartStatus.Empty: 
                heartImg.sprite = emptyHeart;
                break;
            case HeartStatus.Full:
                heartImg.sprite = fullHeart;
                break;     
        }
    }

}

public enum HeartStatus
{
    Empty=0,
    Full=1
 
}
