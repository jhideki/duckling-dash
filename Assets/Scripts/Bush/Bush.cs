using System.Collections;
using UnityEngine;
using System;

public class Bush : MonoBehaviour
{
    public float playerOpacityInBush = 0.5f; // Opacity when player is in the bush
    public bool isHiding = true;
    private GameObject player;
    private Hiding hiding;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Duck"))
        {
            isHiding = true;
            player = other.gameObject;
            hiding = player.GetComponent<Hiding>();
            hiding.SetHiding(true);
            if (player != null)
            {

                StartCoroutine(AdjustPlayerOpacity(player, playerOpacityInBush));
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") || other.CompareTag("Duck"))
        {
            isHiding = false;
            player = other.gameObject;
            hiding = player.GetComponent<Hiding>();
            hiding.SetHiding(false);
            if (player != null)
            {

                StartCoroutine(AdjustPlayerOpacity(player, 1f));
            }
        }
    }

    IEnumerator AdjustPlayerOpacity(GameObject player, float targetOpacity)
    {
        if (player == null)
        {
            yield break;
        }
        SpriteRenderer playerRenderer = player.GetComponent<SpriteRenderer>();
        Color currentColor = playerRenderer.color;
        Color targetColor = new Color(currentColor.r, currentColor.g, currentColor.b, targetOpacity);

        float duration = 0.5f; // You can adjust the fade duration
        float elapsedTime = 0f;

        while (elapsedTime < duration && player != null)
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
