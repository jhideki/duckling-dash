using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target; // The target to follow
    public float smoothness = 250f; // Smoothing factor for camera movement

    void Update()
    {
        if (target != null)
        {
            // Calculate the desired position for the camera
            Vector3 targetPosition = new Vector3(target.position.x, target.position.y, transform.position.z);

            // Smoothly move the camera towards the target position
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothness * Time.deltaTime);
        }
    }
}