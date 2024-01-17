using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float movementSpeed = 5f;
    public float detectionRange = 10f;

    public Transform player;

    void Start()
    {
    }

    void FixedUpdate()
    {
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);

            if (distanceToPlayer < detectionRange)
            {
                // Move towards the player
                Vector3 directionToPlayer = (player.position - transform.position).normalized;
                transform.Translate(directionToPlayer * movementSpeed * Time.fixedDeltaTime);

                // Debugging information
                Debug.Log("Player in range. Distance: " + distanceToPlayer);
            }
            else
            {
                // Debugging information
                Debug.Log("Player out of range. Distance: " + distanceToPlayer);
            }
        }
    }
}