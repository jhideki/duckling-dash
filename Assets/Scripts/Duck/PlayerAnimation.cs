using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    public List<Sprite> nSprites;
    public List<Sprite> eSprites;
    public List<Sprite> sSprites;
    public List<Sprite> sIdleSprites;
    public List<Sprite> eIdleSprites;
    public List<Sprite> nIdleSprites;

    private List<Sprite> selectedSprites;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public float frameRate;
    private int facing = 1;// 1 for east, 2 for north, 3 for south
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

        // setsprite if moving else set idle sprite
        if (rb.velocity.magnitude > 0.1f)
        {

            SetSprite();
        }
        else
        {
            if (facing == 1)
            {
                selectedSprites = eIdleSprites;
            }
            else if (facing == 2)
            {
                selectedSprites = nIdleSprites;
            }
            else if (facing == 3)
            {
                selectedSprites = sIdleSprites;
            }
        }
        int frame = (int)((Time.time * frameRate) % 3);


        spriteRenderer.sprite = selectedSprites[frame];

    }

    void SetSprite()
    {
        if (Mathf.Abs(rb.velocity.x) > Mathf.Abs(rb.velocity.y))
        {
            selectedSprites = eSprites;
            facing = 1;
        }
        else if (Mathf.Abs(rb.velocity.x) < Mathf.Abs(rb.velocity.y) && rb.velocity.y > 0)
        {
            selectedSprites = nSprites;
            facing = 2;
        }
        else if (Mathf.Abs(rb.velocity.x) < Mathf.Abs(rb.velocity.y) && rb.velocity.y < 0)
        {
            selectedSprites = sSprites;
            facing = 3;
        }
    }
}

