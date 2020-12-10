using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildHighlighter : MonoBehaviour
{
    public Material temp;
    public Material normal;

    void Update()
    {
        foreach (BuildingType type in System.Enum.GetValues(typeof(BuildingType)))
        {
            GameObject[] buildings = GameObject.FindGameObjectsWithTag(type.ToString());
            foreach (GameObject go in buildings)
            {
                if (go.GetComponent<SpriteRenderer>())
                    go.GetComponent<SpriteRenderer>().material = normal;
            }
        }
        JobItem currentItem = GetComponentInChildren<JobItem>();
        if (currentItem)
        {
            if (currentItem.buildingState == BuildingType.Client)
            {
                currentItem.controller.client.GetComponent<SpriteRenderer>().material = temp;
            }
            else
            {
                Debug.Log(currentItem.buildingState);
                GameObject[] buildings = GameObject.FindGameObjectsWithTag(currentItem.buildingState.ToString());
                foreach (GameObject go in buildings)
                {
                    if (go.GetComponent<SpriteRenderer>())
                        go.GetComponent<SpriteRenderer>().material = temp;
                }
            }
        }

    }
}
