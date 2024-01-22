using System.Collections;
using UnityEngine;
using System;

public class Bush : MonoBehaviour
{
    public float playerOpacityInBush = 0.5f; // Opacity when player is in the bush
    public bool isHiding;
    private GameObject player;

    // Event to notify when the hiding state changes
    public event Action<bool> OnHidingStateChanged;

    public bool IsHiding
    {
        get { return isHiding; }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isHiding = true;
            player = other.gameObject;
            StartCoroutine(AdjustPlayerOpacity(player, playerOpacityInBush));
            NotifyHidingStateChanged(true);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isHiding = false;
            player = other.gameObject;
            StartCoroutine(AdjustPlayerOpacity(player, 1f)); // Reset opacity to 100%
            NotifyHidingStateChanged(false);
        }
    }

    private void NotifyHidingStateChanged(bool newState)
    {
        // Notify subscribers about the hiding state change
        OnHidingStateChanged?.Invoke(newState);
    }

    IEnumerator AdjustPlayerOpacity(GameObject player, float targetOpacity)
    {
        SpriteRenderer playerRenderer = player.GetComponent<SpriteRenderer>();
        Color currentColor = playerRenderer.color;
        Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, targetOpacity);

        float duration = 0.5f; // You can adjust the fade duration
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            playerRenderer.color = Color.Lerp(currentColor, targetColor, elapsedTime / duration);
            elapsedTime += Time.deltaTime;

            // Allow the player to move while inside the bush
            Vector2 movement = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
            movement.Normalize();
            player.transform.Translate(movement * Time.deltaTime);

            yield return null;
        }

        playerRenderer.color = targetColor; // Ensure exact color at the end of the lerp
    }
}