using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobNotification : MonoBehaviour
{
    public float playerOffset;
    public Sprite[] jobNotificationProgressSprites;
    Transform progressBar;
    Transform clothing;
    public JobController controller;

    private void Update()
    {
        if (!controller)
            Destroy(gameObject);
    }

    private void Awake()
    {
        progressBar = transform.Find("Progress Bar");
        clothing = transform.Find("Clothing");
    }

    public void UpdateJobNotification(Sprite clothingSprite, float progress)
    {
        progressBar.GetComponent<SpriteRenderer>().sprite =
            jobNotificationProgressSprites[Mathf.Max(0, Mathf.RoundToInt((progress * jobNotificationProgressSprites.Length) - 1))];
        clothing.GetComponent<SpriteRenderer>().sprite = clothingSprite;
    }
    public void UpdateJobNotification(Sprite clothingSprite)
    {    
       clothing.GetComponent<SpriteRenderer>().sprite = clothingSprite;
    }

    public void DisableBar()
    {
        progressBar.gameObject.SetActive(false);
    }
    public void EnableBar()
    {
        progressBar.gameObject.SetActive(true);
    }
    public void ParentToObject(Transform transform, float offset)
    {
        this.transform.parent = transform;
        this.transform.position = new Vector2(transform.position.x, transform.position.y + offset);
    }
}
