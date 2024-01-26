using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkAttk : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public float detectionRadius = 5f;
    public float attackDuration = 2f;  // Duration to fly towards the player
    public float resumeRotationDelay = 2f;  // Delay before resuming circular pattern


    private Vector2 center;
    private float angle = 0f;

    private Hiding hiding;
    private DuckCounter duckcounter;
    private Transform target;

    private void Start()
    {
        
        center = transform.position;
        duckcounter = GameObject.Find("DuckCounter").GetComponent<DuckCounter>();
    }

    private void Update()
    {
        FlyInPattern();

        if (DetectPlayer() && (!hiding.GetHiding()))
        {
            if (duckcounter.GetNumDucks() > 0)
            {
                Debug.Log("Found Ducks");
                AttackDuck();
            }
            else
            {
                AttackPlayer();
            }
        }

    }

    
    private void FlyInPattern()
    {
        angle += rotationSpeed * Time.deltaTime;

        // Update the position based on a circular pattern
        float x = center.x + Mathf.Cos(angle) * detectionRadius;
        float y = center.y + Mathf.Sin(angle) * detectionRadius;

        transform.position = new Vector2(x, y);
    }
    

    private bool DetectPlayer()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                target = collider.gameObject.GetComponent<Transform>();
                hiding = target.gameObject.GetComponent<Hiding>();
                return true;
            }

        }

        return false;
    }

    
    private void AttackPlayer()
    {
        // Your attack logic here
        Debug.Log("Player detected! Attack!");

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Move towards the player's position in a straight line
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);

            // Check for overlap with player collider
            if (GetComponent<Collider2D>().OverlapPoint(player.transform.position))
            {
                // Destroy the player
                Destroy(player);
                PlayerDown playerDown = player.GetComponent<PlayerDown>();
                playerDown.LoadGameOverScene();
            }
        }
    }
    

    private void AttackDuck()
    {
        // Your attack logic for the Duck here
        Debug.Log("Duck detected! Attack!");

        // Check if the target GameObject is not null
        if (target != null)
        {
            // Try to get the PickupDuck component on the target
            PickupDuck pickupDuck = target.gameObject.GetComponent<PickupDuck>();

            // Check if the PickupDuck component is not null
            if (pickupDuck != null)
            {
                // Try to get the FollowParent component from the PickupDuck
                FollowParent followParent = pickupDuck.lastDuck;

                // Check if the FollowParent component is not null
                if (followParent != null)
                {
                    // Stop following and destroy the duck GameObject
                    followParent.StopFollowing();
                    Destroy(followParent.gameObject);
                    
                }
                else
                {
                    Debug.Log("FollowParent is null in PickupDuck script.");
                }
            }
            else
            {
                Debug.Log("PickupDuck is null on the target GameObject.");
            }
        }
        else
        {
            Debug.Log("Target GameObject is null.");
        }
    }

    // Draw the detection radius gizmo
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }

}
