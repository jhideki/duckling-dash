using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HawkAttk : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public float detectionRadius = 5f;

    private Vector2 center;
    private float angle = 0f;

    private Hiding hiding;

    private Transform target;

    private void Start()
    {
        center = transform.position;
    }

    private void Update()
    {
        FlyInPattern();

        if (DetectPlayer() && (!hiding.GetHiding()))
        {
            if (DetectDuck())
            {
                AttackDuck();
            }
            else
            {
                AttackPlayer();
            }
        }
    }

    private void FlyInPattern()
    {
        angle += rotationSpeed * Time.deltaTime;

        // Update the position based on a circular pattern
        float x = center.x + Mathf.Cos(angle) * detectionRadius;
        float y = center.y + Mathf.Sin(angle) * detectionRadius;

        transform.position = new Vector2(x, y);
    }

    private bool DetectPlayer()
    {

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (Collider2D collider in colliders)
        {
            if (collider.CompareTag("Player"))
            {
                target = collider.gameObject.GetComponent<Transform>();
                hiding = target.gameObject.GetComponent<Hiding>();
                return true;
            }

        }

        return false;
    }

    private bool DetectDuck()
    {
        Collider2D[] colls = Physics2D.OverlapCircleAll(transform.position, detectionRadius);

        foreach (Collider2D collider in colls)
        {
            if(collider.CompareTag("Duck"))
            {
                target = collider.gameObject.GetComponent<Transform>();
                return true;
            }
        }

        return false;
    }

    private void AttackPlayer()
    {
        // Your attack logic here
        Debug.Log("Player detected! Attack!");
        
        // Replace the line below with your actual attack logic.
        Destroy(GameObject.FindGameObjectWithTag("Player"));
    }

    private void AttackDuck()
    {
        // Your attack logic for the Duck here
        Debug.Log("Duck detected! Attack!");

        PickupDuck pickupDuck = GameObject.FindGameObjectWithTag("Player").GetComponent<PickupDuck>();
        FollowParent followParent = pickupDuck.GetLastDuck();

        followParent.StopFollowing();
        Destroy(followParent.gameObject);
    }

    // Draw the detection radius gizmo
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
