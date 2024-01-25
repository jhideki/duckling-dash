using JetBrains.Annotations;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool canMove = true;

    private Rigidbody2D rb;
    private float horizontalInput;
    private float verticalInput;

    public Bush bush;

    void Start()
    {
        canMove = true; 
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        // Input handling
        horizontalInput = Input.GetAxis("Horizontal");
        verticalInput = Input.GetAxis("Vertical");

        Vector2 movement = new Vector2(horizontalInput, verticalInput); 

        rb.velocity = movement * moveSpeed;

        if (canMove)
        {
            movement.Normalize();   
            rb.velocity = movement * moveSpeed;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }
}