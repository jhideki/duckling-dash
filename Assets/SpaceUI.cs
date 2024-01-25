using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpaceUI : MonoBehaviour
{
    private Image buttonImage;
    private Color normalColor;
    private Color pressedColor = Color.grey; // Change this to the desired grey color

    public float cooldownDuration = 1.0f; // Adjust this value for the cooldown duration
    private bool isOnCooldown = false;

    private void Start()
    {
        // Get the Image component from the button
        buttonImage = GetComponent<Image>();

        // Save the normal color of the button
        normalColor = buttonImage.color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isOnCooldown)
        {
            // Change the button colors based on its current state
            ChangeButtonColors();

            // Start cooldown
            StartCoroutine(Cooldown());
        }
    }

    private void ChangeButtonColors()
    {
        // Set to pressed color
        buttonImage.color = pressedColor;
    }

    private IEnumerator Cooldown()
    {
        // Set cooldown flag to true
        isOnCooldown = true;

        // Wait for the specified cooldown duration
        yield return new WaitForSeconds(cooldownDuration);

        // Set to normal color
        buttonImage.color = normalColor;

        // Set cooldown flag to false
        isOnCooldown = false;
    }
}
