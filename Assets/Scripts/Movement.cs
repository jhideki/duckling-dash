using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust this to set the movement speed
    public bool isMoving;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Input handling
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        isMoving = rb.velocity.magnitude > 0.1f;
        // Calculate movement vector
        Vector2 movement = new Vector2(horizontalInput, verticalInput);

        // Normalize the movement vector to ensure consistent speed in all directions
        movement.Normalize();

        // Move the player
        rb.velocity = movement * moveSpeed;
    }
}
