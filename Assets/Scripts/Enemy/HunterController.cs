using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    //public float trackingRadius = 5f; // Adjust the tracking radius as needed

    public List<Sprite> nSprites;
    public List<Sprite> eSprites;
    public List<Sprite> sSprites;
    public List<Sprite> sIdleSprites;
    public List<Sprite> eIdleSprites;
    public List<Sprite> nIdleSprites;
    public float buffer = 2f;

    private List<Sprite> selectedSprites;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Transform target;

    public float frameRate = 5f;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        selectedSprites = eSprites;
    }

    private void Update()
    {
        // Check if there is a target within the tracking radius
        if (target != null)
        {
            // Play the appropriate animation based on the direction of the duck
            SetSprite();
        }
        else
        {
            // If there is no target within the radius, find the closest target
            target = FindClosestTarget();
            SetSprite();
        }

        int frame = (int)((Time.time * frameRate) % 4);
        spriteRenderer.sprite = selectedSprites[frame];
    }


    private void SetSprite()
    {
        // Find the nearest duck
        Transform nearestDuck = FindClosestTarget();

        if (nearestDuck != null)
        {
            Vector2 duckDirection = nearestDuck.position - transform.position;

            // Adjust the SpriteRenderer's flipX property based on duckDirection
            spriteRenderer.flipX = (duckDirection.x < 0);

            // Adjust Bulletpos based on the selected sprites
            if (Mathf.Abs(duckDirection.x) > Mathf.Abs(duckDirection.y) + buffer)
            {
                selectedSprites = eSprites;
                // Adjust BulletPos based on whether the sprites are flipped
                if (spriteRenderer.flipX)
                {
                    AdjustBulletPos(new Vector2(-1, 0));
                }
                else
                {
                    AdjustBulletPos(new Vector2(1, 0));
                }
            }
            else if (duckDirection.y >= 0)
            {
                selectedSprites = nSprites;
                AdjustBulletPos(new Vector2(0, 0));
            }
            else if (duckDirection.y <= 0)
            {
                selectedSprites = sSprites;
                AdjustBulletPos(new Vector2(0, 0));
            }
        }
    }

    private void AdjustBulletPos(Vector2 newPos)
    {
        // Assuming Bulletpos is a child of the GameObject with this script
        Transform bulletPos = transform.Find("Bulletpos");

        if (bulletPos != null)
        {
            bulletPos.localPosition = newPos;
        }
    }

    private Transform FindClosestTarget()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        GameObject[] ducks = GameObject.FindGameObjectsWithTag("Duck");

        if (player == null && ducks.Length == 0)
        {
            return null;
        }

        Transform closestTarget = null;
        float closestDistance = float.MaxValue;

        if (player != null)
        {
            float playerDistance = Vector2.Distance(transform.position, player.transform.position);
            if (playerDistance < closestDistance)
            {
                closestTarget = player.transform;
                closestDistance = playerDistance;
            }
        }

        foreach (var duck in ducks)
        {
            float duckDistance = Vector2.Distance(transform.position, duck.transform.position);
            if (duckDistance < closestDistance)
            {
                closestTarget = duck.transform;
                closestDistance = duckDistance;
            }
        }

        return closestTarget;
    }
}
