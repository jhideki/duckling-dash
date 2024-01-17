using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;
    private enum PlayerAnimationState { Idle, Moving }
    private PlayerAnimationState currentState;
    private Movement movement;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Vector2 direction;
    private float angle;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        movement = GetComponent<Movement>();
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if (movement.isMoving)
        {
            currentState = PlayerAnimationState.Moving;

        }
        else
        {
            currentState = PlayerAnimationState.Idle;
        }

        RotateSprite();
        anim.SetInteger("State", (int)currentState);

    }

    void RotateSprite()
    {

        direction = new Vector2(rb.velocity.x, rb.velocity.y).normalized;
        angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (movement.isMoving)
        {
            angle += 90f;
        }
        Debug.Log(angle);
        spriteRenderer.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }

}
