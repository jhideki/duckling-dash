using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Transform player;
    public float patrolSpeed = 1f;
    public float patrolRadius = 5f;
    public float visionRange = 3f;
    public float attackCooldown = 5f;
    public float attackSpeed = 7f;

    private bool isAttacking;
    private float lastAttackTime;

    private bool isPatrolling;
    private Vector2 originalPosition;

    void Start()
    {
        // Initialize original position at the start
        originalPosition = transform.position;
        isPatrolling = true;
    }

    void Update()
    {
        if (isAttacking)
        {
            Attack();
        }
        else if (isPatrolling)
        {
            PatrolCircle();
        }
    }

    void OnDrawGizmos()
    {
        // Visualize vision range as a wire sphere in the Scene view and during runtime
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f); // Red with transparency
        Gizmos.DrawWireSphere(transform.position, visionRange);

        // Draw a hard red circle around the enemy during runtime
        DrawHardRedCircle();
    }

    void DrawHardRedCircle()
    {
        Gizmos.color = Color.red;

        int segments = 360;
        float angleIncrement = 360f / segments;

        for (int i = 0; i < segments; i++)
        {
            float angle = i * angleIncrement;
            float x = Mathf.Cos(Mathf.Deg2Rad * angle) * visionRange;
            float y = Mathf.Sin(Mathf.Deg2Rad * angle) * visionRange;

            Vector3 start = transform.position + new Vector3(x, y, 0f);
            angle += angleIncrement;
            x = Mathf.Cos(Mathf.Deg2Rad * angle) * visionRange;
            y = Mathf.Sin(Mathf.Deg2Rad * angle) * visionRange;
            Vector3 end = transform.position + new Vector3(x, y, 0f);

            Gizmos.DrawLine(start, end);
        }
    }

    void PatrolCircle()
    {
        // Calculate circular movement around the original position
        float angle = Time.time * patrolSpeed;
        float x = Mathf.Cos(angle) * patrolRadius;
        float y = Mathf.Sin(angle) * patrolRadius;

        Vector2 offset = new Vector2(x, y);
        transform.position = originalPosition + offset;

        // Check if the player is within vision range
        if (Vector2.Distance(transform.position, player.position) < visionRange)
        {
            StartAttack();
        }
    }

    void StartAttack()
    {
        // Transition from patrolling to attacking
        isPatrolling = false;
        isAttacking = true;
    }

    void Attack()
    {
        // Move towards the player with attack speed
        Vector2 direction = (player.position - (Vector3)transform.position).normalized;
        transform.Translate(direction * attackSpeed * Time.deltaTime);

        // Check if the player is no longer within vision range
        if (Vector2.Distance(transform.position, player.position) > visionRange)
        {
            StopAttack();
        }
    }

    void StopAttack()
    {
        // Transition from attacking to patrolling
        isAttacking = false;
        isPatrolling = true;
    }
}