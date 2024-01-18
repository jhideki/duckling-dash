using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DuckAnimation : MonoBehaviour
{
    public List<Sprite> nSprites;
    public List<Sprite> neSprites;
    public List<Sprite> eSprites;
    public List<Sprite> sSprites;
    public List<Sprite> seSprites;
    private List<Sprite> selectedSprites;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Vector3 lastPosition;
    private Vector3 direction;
    private float changeX;
    public float changeCutoff;
    private float changeY;

    public float frameRate;

    // Start is called before the first frame update
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectedSprites = eSprites;
        lastPosition = transform.position;
        rb = GetComponent<Rigidbody2D>();
    }


    // Update is called once per frame
    void Update()
    {

        if (!spriteRenderer.flipX && changeX < -1f * changeCutoff)
        {
            spriteRenderer.flipX = true;
        }
        else if (spriteRenderer.flipX && changeX > changeCutoff)
        {
            spriteRenderer.flipX = false;
        }

        if (direction.magnitude > 0.1f)
        {

            SetSprite();
        }


        spriteRenderer.sprite = selectedSprites[0];
    }
    void FixedUpdate()
    {

        if (transform.position != lastPosition)
        {
            changeX = transform.position.x - lastPosition.x;
            changeY = transform.position.y - lastPosition.y;
            direction = (transform.position - lastPosition).normalized;
        }

        Debug.Log("changeX: " + changeX + "changeY: " + changeY);

        lastPosition = transform.position;
    }

    void SetSprite()
    {
        if ((changeX > changeCutoff || changeX < -1f * changeCutoff) && changeY > changeCutoff)
        {
            selectedSprites = neSprites;
        }
        else if ((changeX > changeCutoff || changeX < -1f * changeCutoff) && changeY < -1f * changeCutoff)
        {
            selectedSprites = seSprites;
        }
        else if (changeY < changeCutoff && changeY > -1f * changeCutoff && (changeX > changeCutoff || changeX < -1f * changeCutoff))
        {
            selectedSprites = eSprites;
        }
        else if (changeX < changeCutoff && changeX > -1f * changeCutoff && changeY > changeCutoff)
        {
            selectedSprites = nSprites;
        }
        else if ((changeX > changeCutoff || changeX < -1f * changeCutoff) && changeY < -1f * changeCutoff)
        {
            selectedSprites = sSprites;
        }
    }
}
