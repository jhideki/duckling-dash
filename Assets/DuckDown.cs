using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DuckDown : MonoBehaviour
{
    // OnTriggerEnter2D is called when a 2D collider enters a trigger collider
    void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the collider belongs to an object with the tag "Bullet"
        if (other.CompareTag("Bullet"))
        {
            // Bullet hit the duck
            BulletHit(other.gameObject);
        }
    }

    void BulletHit(GameObject bullet)
    {
        // Check if the bullet has a reference to the duck
        FollowParent hitDuck = GetComponent<FollowParent>();
        if (hitDuck != null)
        {
            // Detach the followers from the hit duck
            DetachFollowers(hitDuck);

            // Destroy the hit duck
            Destroy(hitDuck.gameObject);
        }

        // Destroy the bullet
        Destroy(bullet);

        // Call the UpdateLastDuck function in the PickupDuck script
        PickupDuck pickupDuckScript = FindObjectOfType<PickupDuck>();
        if (pickupDuckScript != null)
        {
            pickupDuckScript.ChangeLast();
        }
    }

    void DetachFollowers(FollowParent hitDuck)
    {
        // Start at the end of the chain
        FollowParent currentDuck = hitDuck;

        while (currentDuck != null)
        {
            Debug.Log("Current Duck: " + currentDuck.name);

            // Stop the current duck from following
            currentDuck.StopFollowing();

            // Move to the previous duck
            currentDuck = currentDuck.GetNextDuck();

            if (currentDuck != null)
            {
                Debug.Log("Next Duck: " + currentDuck.name);
            }
        }
    }
}