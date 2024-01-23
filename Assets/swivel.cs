using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swivel : MonoBehaviour
{
    public float trackingRadius = 5f; // Adjust the tracking radius as needed
    public float rotationSpeed = 5f; // Adjust the rotation speed as needed

    //private Transform player;
    private Transform target;
    public float rotationModifier;
    
    void Start()
    {
        // Find the closest target (duck or player) initially
        target = FindClosestTarget();
    }

    void Update()
    {
        // Check if there is a target within the tracking radius
        if (target != null && Vector2.Distance(transform.position, target.position) <= trackingRadius)
        {
            // Calculate the direction to the target
            Vector2 direction = target.position - transform.position;

            // Calculate the rotation angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - rotationModifier;

            // Smoothly rotate the mob towards the target
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
        
            // If there is no target within the radius, find the closest target
            target = FindClosestTarget();
        
    }

    Transform FindClosestTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] ducks = GameObject.FindGameObjectsWithTag("Duck");

        if (player == null && ducks.Length == 0)
        {
            return null;
        }

        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        if (player != null)
        {
            float playerDistance = Vector2.Distance(transform.position, player.transform.position);
            if (playerDistance < closestDistance)
            {
                closestTarget = player.transform;
                closestDistance = playerDistance;
            }
        }

        foreach (var duck in ducks)
        {
            float duckDistance = Vector2.Distance(transform.position, duck.transform.position);
            if (duckDistance < closestDistance)
            {
                closestTarget = duck.transform;
                closestDistance = duckDistance;
            }
        }

        return closestTarget;
    }
}
