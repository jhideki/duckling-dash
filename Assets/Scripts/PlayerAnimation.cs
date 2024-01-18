using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    private enum PlayerAnimationState { north, south, east, west, northwest, northeast, southwest, southeast };
    public List<Sprite> nSprites;
    public List<Sprite> neSprites;
    public List<Sprite> eSprites;
    public List<Sprite> sSprites;
    public List<Sprite> seSprites;
    private List<Sprite> selectedSprites;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public float frameRate;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectedSprites = eSprites;

    }

    // Update is called once per frame
    void Update()
    {
        if (!spriteRenderer.flipX && rb.velocity.x < 0f)
        {
            spriteRenderer.flipX = true;
        }
        else if (spriteRenderer.flipX && rb.velocity.x > 0f)
        {
            spriteRenderer.flipX = false;
        }

        if (rb.velocity.magnitude > 0.1f)
        {

            SetSprite();
        }


        spriteRenderer.sprite = selectedSprites[0];

    }

    void SetSprite()
    {
        if (rb.velocity.x != 0f && rb.velocity.y > 0f)
        {
            selectedSprites = neSprites;
        }
        else if (rb.velocity.x != 0 && rb.velocity.y < 0)
        {
            selectedSprites = seSprites;
        }
        else if (rb.velocity.y == 0)
        {
            selectedSprites = eSprites;
        }
        else if (rb.velocity.x == 0 && rb.velocity.y > 0)
        {
            selectedSprites = nSprites;
        }
        else if (rb.velocity.x == 0 && rb.velocity.y < 0)
        {
            selectedSprites = sSprites;
        }
    }
}

