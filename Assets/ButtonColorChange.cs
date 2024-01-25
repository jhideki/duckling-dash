using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonColorChange : MonoBehaviour
{
    private Image buttonImage;
    private Color normalColor;
    private Color pressedColor = Color.grey; // Change this to the desired grey color

    private void Start()
    {
        // Get the Image component from the button
        buttonImage = GetComponent<Image>();

        // Save the normal color of the button
        normalColor = buttonImage.color;
    }

    private void Update()
    {
        // Check if the Tab key is pressed
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Change the button colors based on its current state
            ChangeButtonColors();
        }
    }

    private void ChangeButtonColors()
    {
        // Toggle between pressed and normal colors
        if (buttonImage.color == normalColor)
        {
            // Set to pressed color
            buttonImage.color = pressedColor;
        }
        else
        {
            // Set to normal color
            buttonImage.color = normalColor;
        }
    }
}
