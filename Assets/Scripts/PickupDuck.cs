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
            Debug.Log("Duck is following " + lastDuck.name);
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

            lastDuck = followScript;
        }


    }
}
