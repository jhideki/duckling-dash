using System.Collections;
using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
    public float patrolSpeed = 2f;
    public float patrolRadius = 10f;
    public float visionRange = 3f;
    public float attackRange = 0.5f;
    public float attackCooldown = 5f;
    public float attackSpeed = 7f;

    private float lastAttackTime;

    private bool isAttacking;
    private bool isPatrolling = true;

    private Bush bush;

    private Vector2 originalPosition;
    private GameObject targetObject;
    private float startingAngle;

    private bool isOnCooldown;
    private Coroutine cooldownCoroutine;

    void Start()
    {
        // Initialize original position at the start
        originalPosition = transform.position;
        startingAngle = UnityEngine.Random.Range(0f, 360f);

        // Find the Bush script in the scene
        bush = FindObjectOfType<Bush>();
    }

    void Update()
    {
        if (bush.isHiding && isPatrolling && isAttacking)
        {
            // Player is hiding in a bush, resume patrolling
            isAttacking = false;
        }
        if (isAttacking && !bush.isHiding)
        {
            Attack();
        }
        else if (isPatrolling || bush.isHiding)
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
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, visionRange);
        foreach (Collider2D targetCollider in targets)
        {
            if ((targetCollider.CompareTag("Player") || targetCollider.CompareTag("Duck")) && !bush.isHiding)
            {
                StartAttack(targetCollider.gameObject);
                return;
            }
        }

        // Calculate circular movement around the original position if no target is found
        float angle = (Time.time * patrolSpeed + startingAngle) % 360f;
        float x = Mathf.Cos(Mathf.Deg2Rad * angle) * patrolRadius;
        float y = Mathf.Sin(Mathf.Deg2Rad * angle) * patrolRadius;

        // Set the new position without changing the rotation
        transform.position = originalPosition + new Vector2(x, y);
    }

    void StartAttack(GameObject target)
    {   
        // Set the target object
        targetObject = target;

        isAttacking = true;
    }

    void Attack()
    {
        // Move towards the target object with attack speed
        if (targetObject != null)
        {
            if (bush.isHiding)
            {
                // Player or duck is hiding in the bush, continue patrolling
                StopAttack();
            }
            else
            {
                Vector2 direction = (targetObject.transform.position - transform.position).normalized;

                // Move towards the target object with attack speed
                transform.Translate(direction * attackSpeed * Time.deltaTime);

                // Check if the enemy is close to the target object to stop attacking
                if (Vector2.Distance(transform.position, targetObject.transform.position) < attackRange)
                {
                    Destroy(targetObject);
                    StopAttack();
                }
            }
        }
        else
        {
            // If the target object is null (destroyed or no longer visible), stop attacking
            StopAttack();
        }
    }

    void StopAttack()
    {
        // Transition from attacking to patrolling
        isAttacking = false;
        isPatrolling = true;
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
        // Store the initial position
        Vector2 initialPosition = transform.position;

        // Wait for the cooldown duration
        yield return new WaitForSeconds(attackCooldown);

        // Allow attacking again after the cooldown
        isOnCooldown = false;

        // If the hawk has moved away during the cooldown, smoothly move back to the initial position
        float startTime = Time.time;
        float elapsedTime = 0f;
        float journeyLength = Vector2.Distance(transform.position, initialPosition);

        while (elapsedTime < 1.0f) // Smooth return over 1 second (adjust as needed)
        {
            elapsedTime = (Time.time - startTime) / attackCooldown;
            float fractionOfJourney = elapsedTime;
            transform.position = Vector2.Lerp(transform.position, initialPosition, fractionOfJourney);
            yield return null;
        }

        // Ensure exact position at the end of the lerp
        transform.position = initialPosition;

        // Resume patrolling
        isPatrolling = true;
    }

    /*IEnumerator AttackCooldown()
    {
        // Store the initial position
        Vector2 initialPosition = transform.position;

        // Wait for the cooldown duration
        yield return new WaitForSeconds(attackCooldown);

        // Allow attacking again after the cooldown
        isOnCooldown = false;

        // Calculate the distance to the initial position
        float distanceToInitial = Vector2.Distance(transform.position, initialPosition);

        // If the hawk has moved away during the cooldown, smoothly move back to the initial position
        if (distanceToInitial > 0.1f)
        {
            float startTime = Time.time;
            float journeyLength = distanceToInitial;

            while (Time.time < startTime + 1.0f) // 1.0f is the duration for the smooth return
            {
                float distCovered = (Time.time - startTime) * patrolSpeed;
                float fractionOfJourney = distCovered / journeyLength;
                transform.position = Vector2.Lerp(transform.position, initialPosition, fractionOfJourney);
                yield return null;
            }

            // Ensure exact position at the end of the lerp
            transform.position = initialPosition;
        }

        // Resume patrolling
        isPatrolling = true;
    }*/
}