using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class FollowParent : MonoBehaviour
{
    private Transform target;

    public float followSpeed;
    public float patrolSpeed = 1.0f; // Default patrol speed
    private FollowParent previousDuck;
    private FollowParent nextDuck;
    public float followDistance = 1.5f;
    public bool isFollowing = false;

    private Vector3 spawnPoint;
    private Vector3 randomTarget; // Store random target position within the box

    public float boxSize = 2.0f; // Default box size
    private float patrolTimer = 0.0f;
    public float patrolDuration = 1.0f; // Minimum time to patrol in one direction

    private FollowParent firstDuck;
    private int deletedDuckCount = 0;
    private DuckCounter duckCounter;

    // Update is called once per frame
    void Start()
    {
        spawnPoint = transform.position; // Set the spawn point
        randomTarget = GetRandomPositionInBox();
        duckCounter = GameObject.Find("DuckCounter").GetComponent<DuckCounter>();
    }


    void FixedUpdate()
    {
        if (target != null && isFollowing)
        {
            if (!target.CompareTag("Player"))
            {
                previousDuck.SetNextDuck(this);
            }

            // Follow the player within the defined distance
            Vector3 directionToPlayer = (target.position - transform.position).normalized;
            Vector3 targetPosition = target.position - directionToPlayer * followDistance;
            transform.position = Vector3.Lerp(transform.position, targetPosition, followSpeed * Time.deltaTime);

            // Reset patrol timer when following the player
            patrolTimer = 0.0f;
        }
        else
        {
            // If not following, move randomly within the adjustable box centered around the spawn point
            if (patrolTimer < patrolDuration)
            {
                // Move in the chosen direction for at least 1 second
                transform.position = Vector3.Lerp(transform.position, randomTarget, patrolSpeed * Time.deltaTime);
                patrolTimer += Time.deltaTime;
            }
            else
            {
                // If the patrol duration is reached, choose a new random target
                randomTarget = GetRandomPositionInBox();
                patrolTimer = 0.0f;
            }
        }
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

    public void StartFollowing(Transform newTarget)
    {
        PickupDuck pickupDuck = newTarget.GetComponent<PickupDuck>();
        if (duckCounter.GetNumDucks() == 0)
        {
            pickupDuck.firstDuck = this;
        }
        duckCounter.IncrementDuckCount();
        target = newTarget;
        isFollowing = true;
    }

    public void StopFollowing()
    {
        duckCounter.DecrementDuckCount();
        isFollowing = false;
        target = null;

        // Disconnect from the previous duck
        if (previousDuck != null)
        {
            previousDuck.nextDuck = null;
        }


        previousDuck = null;
    }

    // Method to free and delete all objects in the chain
    public void FreeAndDeleteAllDucks(FollowParent duck)
    {
        FollowParent currentDuck = duck;
        while (currentDuck != null)
        {
            FollowParent prevDuck = currentDuck.GetPreviousDuck();
            FollowParent nextDuck = currentDuck.GetNextDuck();

            currentDuck = prevDuck;
            Destroy(nextDuck.gameObject); // Destroy the GameObject associated with the FollowParent script
            deletedDuckCount++;
        }
    }


    // Method to set the first duck in the chain
    public void SetFirstDuck(FollowParent duck)
    {
        firstDuck = duck;
    }

    // Method to get the number of deleted ducks
    public int GetDeletedDuckCount()
    {
        return deletedDuckCount;
    }

    public void SetPreviousDuck(FollowParent duck)
    {
        previousDuck = duck;
    }

    public FollowParent GetPreviousDuck()
    {
        return previousDuck;
    }

    public void SetNextDuck(FollowParent duck)
    {
        nextDuck = duck;
    }

    public FollowParent GetNextDuck()
    {
        return nextDuck;
    }

    public bool IsFollowingPlayer()
    {
        return isFollowing;
    }
}