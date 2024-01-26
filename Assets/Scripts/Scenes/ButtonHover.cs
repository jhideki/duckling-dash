using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHover : MonoBehaviour
{
    private Button button;
    private Text buttonText;
    private int originalFontSize;

    void Start()
    {
        // Get the Button component attached to the GameObject
        button = GetComponent<Button>();

        // Get the Text component of the Button
        buttonText = button.GetComponentInChildren<Text>();

        // Store the original font size of the button text
        originalFontSize = buttonText.fontSize;

        // Add EventTrigger component if not already present
        if (button.gameObject.GetComponent<EventTrigger>() == null)
        {
            button.gameObject.AddComponent<EventTrigger>();
        }

        // Attach the PointerEnter and PointerExit events to methods
        EventTrigger trigger = button.gameObject.GetComponent<EventTrigger>();
        EventTrigger.Entry entryEnter = new EventTrigger.Entry { eventID = EventTriggerType.PointerEnter };
        entryEnter.callback.AddListener((data) => { OnPointerEnter(); });
        trigger.triggers.Add(entryEnter);

        EventTrigger.Entry entryExit = new EventTrigger.Entry { eventID = EventTriggerType.PointerExit };
        entryExit.callback.AddListener((data) => { OnPointerExit(); });
        trigger.triggers.Add(entryExit);
    }

    void OnPointerEnter()
    {
        // Increase the font size when the mouse hovers over the button
        buttonText.fontSize = (int)(originalFontSize * 1.4f);
    }

    void OnPointerExit()
    {
        // Reset the font size when the mouse exits
        buttonText.fontSize = originalFontSize;
    }
}
