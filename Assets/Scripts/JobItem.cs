using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JobItem : MonoBehaviour
{
    public BuildingType buildingState;
    public JobController controller;

    private void Update()
    {
        if (!controller)
            Destroy(gameObject);        
    }       

}
