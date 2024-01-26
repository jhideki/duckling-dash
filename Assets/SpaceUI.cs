using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceUI : MonoBehaviour
{
    private Image buttonImage;
    private Color normalColor;
    private Color pressedColor = Color.grey; // Desired grey color

    public float fillDuration = 3.5f; // Adjust this value for the total duration of the fill effect

    private Coroutine fillCoroutine; // Coroutine reference
    private bool fillingInProgress = false; // Flag to track if filling is in progress

    private void Start()
    {
        // Get the Image component from the button
        buttonImage = GetComponent<Image>();

        // Save the normal color of the button
        normalColor = buttonImage.color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (fillingInProgress)
            {
                ResetColor(); // Reset color immediately
            }
            else
            {
                // Start a new fill coroutine only if not already filling
                fillCoroutine = StartCoroutine(FillButton());
            }
        }
    }

    private IEnumerator FillButton()
    {
        fillingInProgress = true; // Set flag to indicate filling is in progress

        float elapsedTime = 0f;
        Color startColor = pressedColor;
        Color targetColor = normalColor;

        while (elapsedTime < fillDuration)
        {
            // Calculate the lerp factor based on elapsed time and total duration
            float lerpFactor = elapsedTime / fillDuration;

            // Lerp between startColor and targetColor
            buttonImage.color = Color.Lerp(startColor, targetColor, lerpFactor);

            // Wait for the next frame
            yield return null;

            // Update elapsed time
            elapsedTime += Time.deltaTime;
        }

        // Ensure the final color is exactly the target color
        buttonImage.color = targetColor;

        fillingInProgress = false; // Reset the flag after filling is complete
    }

    // Separate function to reset the color to the original color
    private void ResetColor()
    {
        buttonImage.color = normalColor;
    }
}
