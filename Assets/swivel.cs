using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class swivel : MonoBehaviour
{
    public float trackingRadius = 5f; // Adjust the tracking radius as needed
    public float rotationSpeed = 5f; // Adjust the rotation speed as needed

    private Transform player;
    public float rotationModifier;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
      
    }

    void Update()
    {
        // Check if the player is within the tracking radius
        if (Vector2.Distance(transform.position, player.position) <= trackingRadius)
        {
            // Calculate the direction to the player
            Vector2 direction = player.position - transform.position;

            // Calculate the rotation angle
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - rotationModifier;

            // Smoothly rotate the mob towards the player
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }
}
