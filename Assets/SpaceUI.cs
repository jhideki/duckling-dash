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
            // Start fill effect coroutine
            StartCoroutine(FillButton());
        }
    }

    private IEnumerator FillButton()
    {
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
    }
}
