using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirdAnimatin : MonoBehaviour
{
    public List<Sprite> nSprites;
    public List<Sprite> eSprites;
    public List<Sprite> sSprites;
    private List<Sprite> selectedSprites;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    public float frameRate;
    private float changeX;
    public float changeCutoff;
    private float changeY;

    private Vector3 lastPosition;
    private Vector3 direction;
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
        if (!spriteRenderer.flipX && changeX < 0f)
        {
            spriteRenderer.flipX = true;
        }
        else if (spriteRenderer.flipX && changeX > 0f)
        {
            spriteRenderer.flipX = false;
        }

        if (direction.magnitude > 0.1f)
        {
            SetSprite();
        }

    }

    void FixedUpdate()
    {

        if (transform.position != lastPosition)
        {
            changeX = transform.position.x - lastPosition.x;
            changeY = transform.position.y - lastPosition.y;
            direction = (transform.position - lastPosition).normalized;
        }

        lastPosition = transform.position;

        int frame = (int)(Time.time * frameRate % 4);

        spriteRenderer.sprite = selectedSprites[frame];
    }
    void SetSprite()
    {
        if (Mathf.Abs(changeY) > Mathf.Abs(changeX) && changeY > 0)
        {
            selectedSprites = nSprites;

        }
        else if (Mathf.Abs(changeY) > Mathf.Abs(changeX) && changeY < 0)
        {
            selectedSprites = sSprites;
        }
        else if (Mathf.Abs(changeY) < Mathf.Abs(changeX))
        {
            selectedSprites = eSprites;
        }
    }
}
