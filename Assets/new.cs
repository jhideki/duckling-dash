using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StashDuck : MonoBehaviour
{

    private PickupDuck pickupDuck;
    private FollowParent followParent;
    public int numCollider = 2;
    private int Count = 1;
    private bool Isreset = false;
    /*
    void Start()
    {
        Debug.Log("script");
    }
    */

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Debug.Log("OnTriggerEnter called!");
        //Debug.Log("COUNT" + Count);

        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player entered the trigger!");

            if(Count >= numCollider)
            {
                MultiCollide(other.gameObject);
                Isreset = true;
            }

            Count++;
        }

        if (Isreset)
        {
            Count = 1;
            Isreset = false;
        }

    }

    private void MultiCollide(GameObject playr)
    {
        Debug.Log("Multi");
        // Player entered the trigger, try to find the FollowParent script dynamically
        pickupDuck = playr.GetComponent<PickupDuck>();
        followParent = playr.GetComponent<FollowParent>();

        if (pickupDuck != null)
        {
            if (followParent != null)
            {

                // Call a method in PickupDuck to update the last duck reference
                FollowParent lastDuck = pickupDuck.GetLastDuck();
                Debug.Log("Last Duck: " + (lastDuck != null ? lastDuck.name : "null"));

                // Debug statement to check if FreeAndDeleteAllDucks is called
                Debug.Log("Calling FreeAndDeleteAllDucks");
                followParent.FreeAndDeleteAllDucks(lastDuck);
            }
            else
            {

            }
        }
        else
        {
            // Log a warning if FollowParent script is not found
            Debug.LogWarning("FollowParent script not found on the GameObject or its children.");
        }
    }
}
