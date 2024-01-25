using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupDuck : MonoBehaviour
{
    private FollowParent lastDuck;
    // Start is called before the first frame update

    void Start()
    {
        lastDuck = null;

    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Duck"))
        {
            GetDuck(other.gameObject);
        }
    }

    
    private void GetDuck(GameObject duck)
    {
        FollowParent followScript = duck.GetComponent<FollowParent>();
        if (followScript != null && !followScript.isFollowing)
        {
            if (lastDuck == null)
            {
                followScript.StartFollowing(transform);

            }
            else
            {

                followScript.StartFollowing(lastDuck.transform);
            }
            followScript.SetPreviousDuck(lastDuck);

             // Add debug statements to check the linked list
            //Debug.Log("Current Duck: " + duck.name);
            //Debug.Log("Previous Duck: " + (lastDuck != null ? lastDuck.name : "null"));
           // Debug.Log("Next Duck: " + (followScript.GetNextDuck() != null ? followScript.GetNextDuck().name : "null"));

            lastDuck = followScript;
        }


    }

    public void ChangeLast()
    {
        if(lastDuck == null)
        {

        }
        else
        {
            GameObject currentDuck = gameObject;  // Start from the player or whatever your starting point is

            while (currentDuck != null)
            {
                FollowParent followScript = currentDuck.GetComponent<FollowParent>();

                if (followScript != null)
                {
                    FollowParent nextDuck = followScript.GetNextDuck();

                    if (nextDuck != null)
                    {
                        currentDuck = nextDuck.gameObject;
                    }
                    else
                    {
                        // Handle the case where nextDuck is null
                        Debug.LogError("Next Duck is null. Handle this case appropriately.");
                        break;  // Break out of the loop or add other logic as needed
                    }
                }
                else
                {
                    // Handle the case where followScript is null
                    Debug.LogError("FollowParent script is null. Handle this case appropriately.");
                    break;  // Break out of the loop or add other logic as needed
                }
            }
        }
    }

    public FollowParent GetLastDuck()
    {
        ChangeLast();
        return lastDuck;
    }


}