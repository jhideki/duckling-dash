using System.Collections;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float patrolSpeed = 1f;
    public float patrolRadius = 5f;
    public float visionRange = 3f;
    public float attackRange = 0.5f;
    public float attackCooldown = 3f;
    public float attackSpeed = 7f;

    private bool isAttacking;
    private float lastAttackTime;

    private bool isPatrolling;
    private Vector2 originalPosition;
    private GameObject targetObject;

    private bool isOnCooldown;
    private Coroutine cooldownCoroutine;

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
        // Visualize vision range and attack range as wire spheres in the Scene view and during runtime
        Gizmos.color = new Color(1f, 0f, 0f, 0.2f); // Red with transparency

        // Vision range
        Gizmos.DrawWireSphere(transform.position, visionRange);

        // Attack range
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.2f); // Orange with transparency
        Gizmos.DrawWireSphere(transform.position, attackRange);

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
        // Check if the player or any object with the tag "Duck" is within vision range
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, visionRange);
        foreach (Collider2D targetCollider in targets)
        {
            if (targetCollider.CompareTag("Player") || targetCollider.CompareTag("Duck"))
            {
                StartAttack(targetCollider.gameObject);
                return; // Stop patrolling if at least one target is found
            }
        }

        // Calculate circular movement around the original position if no target is found
        float angle = Time.time * patrolSpeed;
        float x = Mathf.Cos(angle) * patrolRadius;
        float y = Mathf.Sin(angle) * patrolRadius;

        Vector2 offset = new Vector2(x, y);
        transform.position = Vector2.Lerp(transform.position, originalPosition + offset, patrolSpeed * Time.deltaTime);
    }

    void StartAttack(GameObject target)
    {
        // Transition from patrolling to attacking
        isPatrolling = false;
        isAttacking = true;

        // Set the target object
        targetObject = target;
    }

    void Attack()
    {
        // Move towards the target object with attack speed
        if (targetObject != null)
        {
            Vector2 direction = (targetObject.transform.position - transform.position).normalized;
            transform.Translate(direction * attackSpeed * Time.deltaTime);

            // Check if the enemy is close to the target object to stop attacking
            if (Vector2.Distance(transform.position, targetObject.transform.position) < attackRange)
            {
                Destroy(targetObject);
                StopAttack();
            }
        }
    }

    void StopAttack()
    {
        // Transition from attacking to patrolling
        isAttacking = false;
        targetObject = null;

        // Start a cooldown before resuming patrolling
        StartAttackCooldown();
    }

    void StartAttackCooldown()
    {
        if (!isOnCooldown)
        {
            // Start cooldown only if not already on cooldown
            isOnCooldown = true;
            cooldownCoroutine = StartCoroutine(AttackCooldown());
        }
    }

    IEnumerator AttackCooldown()
    {
        // Move back to patrol area during cooldown
        while (Vector2.Distance(transform.position, originalPosition) > 0.1f)
        {
            transform.position = Vector2.Lerp(transform.position, originalPosition, patrolSpeed * Time.deltaTime);
            yield return null;
        }

        // Ensure exact position at the end of the lerp
        transform.position = originalPosition;

        // Resume patrolling immediately after returning to patrol area
        isPatrolling = true;

        // Wait for the cooldown duration
        yield return new WaitForSeconds(attackCooldown);

        // Allow attacking again after a 2-second delay
        isOnCooldown = false; // Reset cooldown flag for immediate attacks
    }
}