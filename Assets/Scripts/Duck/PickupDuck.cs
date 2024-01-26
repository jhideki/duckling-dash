using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupDuck : MonoBehaviour
{
    public FollowParent lastDuck;
    public FollowParent firstDuck;
    private DuckCounter duckCounter;
    public AudioSource audioSource;
    // Start is called before the first frame update

    void Start()
    {
        lastDuck = null;

        duckCounter = GameObject.Find("DuckCounter").GetComponent<DuckCounter>();
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

            audioSource.Play();
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

            lastDuck = followScript;

        }


    }

    public void ChangeLast()
    {
        if (lastDuck == null)
        {
            // Handle the case where lastDuck is null
            Debug.Log("Last Duck is null. Handle this case appropriately.");
            return;
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

            lastDuck = currentDuck.GetComponent<FollowParent>();
        }
    }

    public FollowParent GetLastDuck()
    {
        ChangeLast();
        return lastDuck;
    }


}