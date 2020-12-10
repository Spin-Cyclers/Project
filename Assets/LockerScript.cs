using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockerScript : MonoBehaviour
{

    public Sprite closed;
    public Sprite open;

   
    void Update()
    {
        if(transform.childCount > 0)
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = closed;
        }
        else
        {
            gameObject.GetComponent<SpriteRenderer>().sprite = open;
        }
    }
}
