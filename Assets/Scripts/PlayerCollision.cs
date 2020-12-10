using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCollision : MonoBehaviour
{

    float timeInteracted;

    //Handle collisions with buildings and transferring JobItems
    void OnCollisionStay2D(Collision2D collision)
    {
        //"Interact" button
        Debug.Log("Colliding");
        if (Input.GetKey("space") && timeInteracted + 0.25f < Time.time && collision.gameObject.tag != "Ignore")
        {
            timeInteracted = Time.time;
            Debug.Log("Working");
            //If Player is not currently carrying a job and building does have one
            if (gameObject.GetComponentInChildren<JobItem>() == null)
            {
                if (collision.gameObject.GetComponentInChildren<JobItem>())
                {
                    collision.gameObject.GetComponentInChildren<JobItem>().transform.SetParent(transform);
                    if( !FindObjectOfType<GameManager>().Over && (!PlayerPrefs.HasKey("Mute") || PlayerPrefs.GetInt("Mute") == 0))
                    FindObjectOfType<AudioManager>().Play("Pickup");
                }
            }
            else
            {
                if (collision.gameObject.GetComponentInChildren<JobItem>() == null)
                {
                    gameObject.GetComponentInChildren<JobItem>().transform.SetParent(collision.transform);
                    if( !FindObjectOfType<GameManager>().Over && (!PlayerPrefs.HasKey("Mute") || PlayerPrefs.GetInt("Mute") == 0))
                    FindObjectOfType<AudioManager>().Play("Place");
                }
            }

        }

    }
}
