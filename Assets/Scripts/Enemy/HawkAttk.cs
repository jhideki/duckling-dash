using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkAttk : MonoBehaviour
{
    public float moveSpeed = 5f;

    public float detectionRadius = 5f;

    private Hiding hiding;
    private DuckCounter duckcounter;
    private Transform target;
    private Vector3 spawnPoint;
    private Vector3 randomTarget; // Store random target position within the box

    public float boxSize = 2.0f; // Default box size
    private float patrolTimer = 0.0f;
    public float patrolDuration = 1.0f; // Minimum time to patrol in one direction

    private void Start()
    {

        spawnPoint = transform.position;
        duckcounter = GameObject.Find("DuckCounter").GetComponent<DuckCounter>();
        randomTarget = GetRandomPositionInBox();
    }

    private void Update()
    {

        if (DetectPlayer() && (!hiding.GetHiding()))
        {
            if (duckcounter.GetNumDucks() > 0)
            {
                AttackDuck();
            }
            else
            {
                AttackPlayer();
            }
        }
        else
        {
            FlyInPattern();
        }

    }


    private void FlyInPattern()
    {
        // If not following, move randomly within the adjustable box centered around the spawn point
        if (patrolTimer < patrolDuration)
        {
            // Move in the chosen direction for at least 1 second
            transform.position = Vector3.Lerp(transform.position, randomTarget, moveSpeed * Time.deltaTime);
            patrolTimer += Time.deltaTime;
        }
        else
        {
            // If the patrol duration is reached, choose a new random target
            randomTarget = GetRandomPositionInBox();
            patrolTimer = 0.0f;
        }
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
    private Vector3 GetRandomPositionInBox()
    {
        float x = Random.Range(spawnPoint.x, spawnPoint.x + boxSize);
        float y = Random.Range(spawnPoint.y, spawnPoint.y + boxSize);

        return new Vector3(x, y, 0);
    }

    public void SetBoxSize(float newSize)
    {
        boxSize = Mathf.Max(newSize, 0.1f); // Ensure that box size is at least 0.1 to prevent division by zero
    }


    private void AttackPlayer()
    {

        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            // Your attack logic here
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