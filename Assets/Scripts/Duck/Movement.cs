using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f; // Adjust this to set the default movement speed
    public bool isMoving;
    public bool isHiding;
    public bool canMove;

    private Rigidbody2D rb;
    private float horizontalInput;
    private float verticalInput;

    void Start()
    {
        canMove = true;
        rb = GetComponent<Rigidbody2D>();

        Bush bush = FindObjectOfType<Bush>();
        if (bush != null)
        {
            bush.OnHidingStateChanged += HandleHidingStateChanged;
        }
    }

    void Update()
    {
        // Input handling
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        isMoving = rb.velocity.magnitude > 0.1f;
    }

    void FixedUpdate()
    {
        // Calculate movement vector
        Vector2 movement = new Vector2(horizontalInput, verticalInput);

        // Normalize the movement vector to ensure consistent speed in all directions
        movement.Normalize();

        // Adjust movement speed based on hiding status
        float currentMoveSpeed = isHiding ? moveSpeed * 0.5f : moveSpeed;

        // Move the player
        if (canMove)
        {
            rb.velocity = movement * currentMoveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    private void HandleHidingStateChanged(bool isHiding)
    {
        this.isHiding = isHiding;
    }
}