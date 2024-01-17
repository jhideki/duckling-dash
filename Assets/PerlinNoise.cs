using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerlinNoise : MonoBehaviour
{
    public int width = 256;
    public int height = 256;

    public float scale = 20f;
    public float moveSpeed = 5f;

    public float offsetX = 100f;
    public float offsetY = 100f;

    private Rigidbody2D playerRb;

    void Start()
    {
        // Find the player GameObject and get its Rigidbody2D
        GameObject player = GameObject.FindWithTag("Player"); // Adjust tag or find method accordingly
        if (player != null)
        {
            playerRb = player.GetComponent<Rigidbody2D>();
        }
        else
        {
            Debug.LogError("Player GameObject not found or doesn't have a Rigidbody2D.");
        }

        offsetX = Random.Range(0f, 99999f);
        offsetY = Random.Range(0f, 99999f);
    }

    void Update()
    {
        //player move effect
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        offsetX -= horizontalInput * moveSpeed * Time.deltaTime;
        offsetY += verticalInput * moveSpeed * Time.deltaTime;

        //perlin 
        Renderer renderer = GetComponent<Renderer>();
        renderer.material.mainTexture = GenerateTexture();

        MovePlayer();
    }

    Texture2D GenerateTexture()
    {
        Texture2D texture = new Texture2D(width, height);
        for(int x = 0; x < width; x++)
        {
            for(int y = 0; y < height; y++)
            {
                Color color = CalculateColor(x, y);
                texture.SetPixel(x, y, color);
            }
        }

        texture.Apply();
        return texture;
    }

    Color CalculateColor (int x, int y)
    {
        float xCoord = (float)x / width * scale + offsetX;
        float yCoord = (float)y / height * scale + offsetY;

        float sample = Mathf.PerlinNoise(xCoord, yCoord);
        return new Color(sample, sample, sample);
    }

    void MovePlayer()
    {
        // Input handling
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // Calculate movement vector
        Vector2 movement = new Vector2(horizontalInput, verticalInput);

        // Normalize the movement vector to ensure consistent speed in all directions
        movement.Normalize();

        // Move the player using Rigidbody
        if (playerRb != null)
        {
            playerRb.velocity = movement * moveSpeed;
        }
    }
}
