using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOut : MonoBehaviour
{
    public float zoomOutSize = 5f;
    public float zoomInSize = 3f;
    public float zoomSpeed = 2f;

    private Camera mainCamera;
    private bool isZoomedOut = false;
    private Movement movement;
    public Transform player;

    void Start()
    {
        mainCamera = Camera.main;
        movement = player.GetComponent<Movement>();

    }

    void Update()
    {
        // Check for Tab key press
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Toggle zoom state
            isZoomedOut = !isZoomedOut;

            // Set the camera size based on the zoom state
            if (isZoomedOut)
            {
                StartCoroutine(ZoomCoroutine(zoomOutSize));
                movement.canMove = false;
            }
            else
            {
                StartCoroutine(ZoomCoroutine(zoomInSize));
                movement.canMove = true;

            }
        }
    }

    private System.Collections.IEnumerator ZoomCoroutine(float targetSize)
    {
        float startSize = mainCamera.orthographicSize;
        float elapsedTime = 0f;

        while (elapsedTime < 1f)
        {
            mainCamera.orthographicSize = Mathf.Lerp(startSize, targetSize, elapsedTime);
            elapsedTime += Time.deltaTime * zoomSpeed;
            yield return null;
        }

        mainCamera.orthographicSize = targetSize; // Ensure the final size is set exactly
    }
}
